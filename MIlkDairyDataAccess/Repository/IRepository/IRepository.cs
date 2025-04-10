using System.Linq.Expressions;


namespace MilkDairy.DataAccess.Repository.IRepository
{
    public  interface IRepository<T> where T : class
    {
       public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

       public T Get(Expression<Func<T,bool>> filter,  bool tracked = false, string? includeProperties = null);
        public void Add(T entity);
        public void Remove(T entity);
        public void RemoveRange(IEnumerable<T> entity);

    }
}
