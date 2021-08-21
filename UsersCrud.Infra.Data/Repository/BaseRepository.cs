﻿using System.Collections.Generic;
using System.Linq;
using UsersCrud.Domain.Entities;
using UsersCrud.Domain.Interfaces;
using UsersCrud.Infra.Data.Contexts;

namespace UsersCrud.Infra.Data.Repository
{
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

        public void Delete(int id)
        {
            _postgresContext.Set<TEntity>().Remove(Select(id));
        }

        public IList<TEntity> Select() =>
            _postgresContext.Set<TEntity>().ToList();

        public TEntity Select(int id) =>
            _postgresContext.Set<TEntity>().Find(id);

        public void SaveChanges()
        {
            _postgresContext.SaveChanges();
        }
    }
}
