using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace MediaDataApplication.WcfService.DAL.Repository {

    public class EfRepository<T> : IRepository<T> where T : class {
        public EfRepository(DbContext dbContext) {
            if (dbContext == null) {
                throw new ArgumentNullException("DbContext is null");
            }

            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<T>();
        }

        #region IRepository<T> implementation

        public virtual IQueryable<T> AsQueryable() {
            return this.DbSet.AsQueryable();
        }

        public IEnumerable<T> GetAll() {
            return this.DbSet;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) {
            return this.DbSet.Where(predicate);
        }

        public T Single(Expression<Func<T, bool>> predicate) {
            return this.DbSet.Single(predicate);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return this.DbSet.SingleOrDefault(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return this.DbSet.FirstOrDefault(predicate);
        }

        public bool Any(Expression<Func<object, bool>> func) {
            return this.DbSet.Any(func);
        }

        public T GetById(object id) {
            return this.DbSet.Find(id);
        }

        public void Add(T entity) {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached) {
                dbEntityEntry.State = EntityState.Added;
            }
            else {
                this.DbSet.Add(entity);
            }
        }

        public void Delete(T entity) {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted) {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public void Update(T entity) {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached) {
                this.DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

        #endregion

        protected DbSet<T> DbSet { get; set; }

        protected DbContext DbContext { get; set; }
    }

}