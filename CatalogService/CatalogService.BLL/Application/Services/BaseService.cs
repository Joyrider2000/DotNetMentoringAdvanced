using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Domain.Entities;
using CatalogService.BLL.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CatalogService.BLL.Application.Services
{
    public class BaseService<T> : IBaseService<T> where T : IdEntity
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> List()
        {
            return _repository.List();
        }

        public T? Add(T entity)
        {
            if (entity.isValid())
            {
                try
                {
                    return _repository.Add(entity);
                }
                catch (ConstraintViolationException)
                {
                    Console.WriteLine($"{typeof(T)} #{entity.Id} constraint violation during item creation.");
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected exception happened when creating {typeof(T)} #{entity.Id}: {ex}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"{typeof(T)} #{entity.Id} data is not valid: {JsonSerializer.Serialize(entity)}");
                throw new ValidationException();
            }
        }

        public T? Get(int id)
        {
            try
            {
                return _repository.Get(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception happened when getting {typeof(T)} #{id}: {ex}");
                return null;
            }
        }

        public void Update(T entity)
        {
            if (entity.isValid())
            {
                try
                {
                    _repository.Update(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected exception happened when updating {typeof(T)} #{entity.Id}: {ex}");
                }
            }
            else
            {
                Console.WriteLine($"{typeof(T)} #{entity.Id} data is not valid: {JsonSerializer.Serialize(entity)}");
                throw new ValidationException();
            }
        }

        public void Delete(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception happened when updating {typeof(T)} #{id}: {ex}");
            }
        }

        public void Print(T entity)
        {
            Console.WriteLine(entity != null ? JsonSerializer.Serialize(entity) : "null");
        }

        public void Print(IEnumerable<T> entityList)
        {
            Console.WriteLine(JsonSerializer.Serialize(entityList));
        }
    }
}
