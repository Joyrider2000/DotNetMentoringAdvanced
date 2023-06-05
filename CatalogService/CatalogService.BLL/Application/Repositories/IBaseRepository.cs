namespace CatalogService.BLL.Application.Repositories
{
    public interface IBaseRepository<T>
    {
        public Task<IEnumerable<T>> List();
        public Task<T?> Get(int id);
        public Task<T?> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(int id);
    }
}
