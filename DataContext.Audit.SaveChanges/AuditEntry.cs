using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Audit.SaveChanges
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
        public string UserName { get; set; }

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditEntity ToAudit()
        {
            var audit = new AuditEntity();
            audit.TableName = TableName;
            audit.DateTime = DateTime.UtcNow;
            audit.UserName = string.IsNullOrEmpty(UserName) ? "Anonimo" : UserName;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues); // In .NET Core 3.1+, you can use System.Text.Json instead of Json.NET
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            return audit;
        }
    }
}
