using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsersCrud.Domain.Entities;
using UsersCrud.Domain.Interfaces;
using UsersCrud.Infra.Data.Contexts;

namespace UsersCrud.Infra.Data.Repository
{
    /// <summary>
    /// Classe base para todos os repositórios contendo os métodos necessários
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DataContext _postgresContext;

        public BaseRepository(DataContext postgresContext)
        {
            _postgresContext = postgresContext;
        }

        public void Insert(TEntity obj)
        {
            _postgresContext.Set<TEntity>().Add(obj);
        }

        public void Update(TEntity obj)
        {
            _postgresContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;            
        }

        public void Delete(Guid id)
        {
            _postgresContext.Set<TEntity>().Remove(Select(id));
        }

        public IEnumerable<TEntity> Select() =>
            _postgresContext.Set<TEntity>().ToList();

        public TEntity Select(Guid id) =>
            _postgresContext.Set<TEntity>().Find(id);

        public TEntity SelectWhere(Func<TEntity, bool> predicate) =>
            _postgresContext.Set<TEntity>().Where(predicate).FirstOrDefault();

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _postgresContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
