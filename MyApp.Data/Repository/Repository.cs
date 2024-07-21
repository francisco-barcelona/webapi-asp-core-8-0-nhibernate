using MyApp.Data.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ISessionFactory _sessionFactory;

        public Repository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public T GetByIdWithEagerLoading(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            using var session = _sessionFactory.OpenSession();
            var query = session.Query<T>().Where(x => x.Id == id);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Fetch(includeProperty);
            }

            return query.SingleOrDefault();
        }

        public T Get(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public IEnumerable<T> GetAllWithEagerLoading(params Expression<Func<T, object>>[] includeProperties)
        {
            using var session = _sessionFactory.OpenSession();
            var query = session.Query<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Fetch(includeProperty);
            }

            return query.ToList();
        }

        public IEnumerable<T> GetAll()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<T>().ToList();
            }
        }

        public void Add(T entity)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        public void Update(T entity)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }

        public void Delete(T entity)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }
    }
}
