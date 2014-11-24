using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.Tests.Fake.Repository {

    public class FakeRepository<T> : IRepository<T> where T : class {
        private readonly ICollection<T> collection;

        public FakeRepository(ICollection<T> collection) {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            this.collection = collection;
        }

        #region IRepository<T> Members

        public IQueryable<T> AsQueryable() {
            return this.collection.AsQueryable();
        }

        public IEnumerable<T> GetAll() {
            return this.collection;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) {
            return this.collection.Where(predicate.Compile());
        }

        public T Single(Expression<Func<T, bool>> predicate) {
            return this.collection.Single();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return this.collection.SingleOrDefault(predicate.Compile());
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return this.collection.FirstOrDefault(predicate.Compile());
        }

        public bool Any(Expression<Func<object, bool>> func) {
            return this.collection.Any(func.Compile());
        }

        public T GetById(object id) {
            Type type = typeof(T);

            var keyProperties = from prop in type.GetProperties()
                                where prop.Name.Equals("Id") || prop.Name.Equals(type.Name + "Id") || prop.GetCustomAttributes(true).Any(x => x.GetType().Name.Contains("Key"))
                                select prop;

            return (from instance in this.collection
                    from keyProperty in keyProperties.AsQueryable()
                    let propValue = keyProperty.GetValue(instance, null)
                    where propValue.Equals(id)
                    select instance).FirstOrDefault();
        }

        public void Add(T entity) {
            this.collection.Add(entity);
        }

        public void Delete(T entity) {
            this.collection.Remove(entity);
        }

        public void Update(T entity) {
            Type type = typeof(T);
            object id = (from prop in type.GetProperties()
                         where prop.Name.Equals("Id") || prop.Name.Equals(type.Name + "Id") || prop.GetCustomAttributes(true).Any(x => x.GetType().Name.Contains("Key"))
                         select prop.GetValue(entity)).FirstOrDefault();

            var element = this.GetById(id);
            this.collection.Remove(element);
            this.collection.Add(entity);
        }

        #endregion
    }

}