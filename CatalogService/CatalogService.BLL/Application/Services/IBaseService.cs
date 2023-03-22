using CatalogService.BLL.Domain.Entities;

namespace CatalogService.BLL.Application.Services
{
    public interface IBaseService<T> where T : IdEntity
    {
        public IEnumerable<T> List();
        public T? Get(int id);
        public T? Add(T entity);
        public void Update(T entity);
        public void Delete(int id);
        public void Print(T entity);
        public void Print(IEnumerable<T> entityList);
    }
}
