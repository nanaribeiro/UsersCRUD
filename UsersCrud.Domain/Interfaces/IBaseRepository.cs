using System;
using System.Collections.Generic;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Domain.Interfaces
{
    /// <summary>
    /// Interface base para todos os repositórios
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        void Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(Guid id);

        IEnumerable<TEntity> Select();

        TEntity Select(Guid id);

        void SaveChanges();

        TEntity SelectWhere(Func<TEntity, bool> predicate);
    }
}
