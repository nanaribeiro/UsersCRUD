using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Audit.SaveChanges
{
    public abstract class DbContextWithAuditing: DbContext
    {
        public DbContextWithAuditing()
        {
        }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="options">Opções do contexto.</param>
        public DbContextWithAuditing(DbContextOptions options) : base(options)
        {
        }

        public string UserName { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var auditEntries = OnBeforeSaveChanges(UserName);
                var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await OnAfterSaveChanges(auditEntries);
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private List<AuditEntry> OnBeforeSaveChanges(string userName)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditEntity || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Metadata.GetTableName(); // EF Core 3.1: entry.Metadata.GetTableName();
                auditEntry.UserName = userName;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (entry.Metadata.IsOwned())
                            {
                                auditEntry.OldValues[propertyName] = entry.GetDatabaseValues().GetValue<object>(propertyName).ToString();
                            }
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = entry.GetDatabaseValues().GetValue<object>(propertyName).ToString();
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                this.Add(auditEntry.ToAudit());
            }

            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }
        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                this.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();
        }
    }
}
