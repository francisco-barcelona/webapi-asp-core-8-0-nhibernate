﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWithEagerLoading(params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}