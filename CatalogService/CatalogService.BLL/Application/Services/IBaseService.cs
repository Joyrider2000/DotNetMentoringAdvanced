using CatalogService.BLL.Domain.Entities;

namespace CatalogService.BLL.Application.Services
{
    public interface IBaseService<T> where T : IdEntity
    {
        public Task<IEnumerable<T>> List();
        public Task<T?> Get(int id);
        public Task<T?> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(int id);
        public void Print(T entity);
        public void Print(IEnumerable<T> entityList);
    }
}
