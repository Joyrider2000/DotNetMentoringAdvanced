namespace CatalogService.BLL.Application.Repositories
{
    public interface IBaseRepository<T>
    {
        public IEnumerable<T> List();
        public T? Get(int categoryId);
        public T? Add(T category);
        public void Update(T category);
        public void Delete(int categoryId);
    }
}
