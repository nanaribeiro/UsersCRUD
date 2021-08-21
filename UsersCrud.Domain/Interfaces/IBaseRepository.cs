using System.Collections.Generic;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        void Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(int id);

        IList<TEntity> Select();

        TEntity Select(int id);

        void SaveChanges();
    }
}
