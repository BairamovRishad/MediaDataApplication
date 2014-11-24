using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MediaDataApplication.WcfService.DAL.Repository {

    public interface IRepository<T> where T : class {
        void Add(T entity);

        bool Any(Expression<Func<object, bool>> func);

        IQueryable<T> AsQueryable();

        void Delete(T entity);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();

        T GetById(object id);

        T Single(Expression<Func<T, bool>> predicate);

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        void Update(T entity);
    }

}