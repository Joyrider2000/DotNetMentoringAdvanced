using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Domain.Entities;
using CatalogService.BLL.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CatalogService.BLL.Application.Services
{
    public class BaseService<T> : IBaseService<T> where T : IdEntity
    {
        protected readonly IBaseRepository<T> _repository;
        protected readonly ILogger<BaseService<T>> _logger;

        public BaseService(IBaseRepository<T> repository, ILogger<BaseService<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> List()
        {
            _logger.LogInformation("Listing all in service");
            return await _repository.List();
        }

        public async Task<T?> Add(T entity)
        {
            if (entity.isValid())
            {
                try
                {
                    return await _repository.Add(entity);
                }
                catch (ConstraintViolationException)
                {
                    _logger.LogInformation($"{typeof(T)} #{entity.Id} constraint violation during item creation.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Unexpected exception happened when creating {typeof(T)} #{entity.Id}: {ex}");
                    throw;
                }
            }
            else
            {
                _logger.LogInformation($"{typeof(T)} #{entity.Id} data is not valid: {JsonSerializer.Serialize(entity)}");
                throw new ValidationException();
            }
        }

        public async Task<T?> Get(int id)
        {
            try
            {
                return await _repository.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unexpected exception happened when getting {typeof(T)} #{id}: {ex}");
                throw;
            }
        }

        public virtual async Task Publish(T entity)
        {
            await Task.FromResult(0);
        }

        public async Task<bool> Update(T entity)
        {
            if (entity.isValid())
            {
                try
                {
                    bool result = await _repository.Update(entity);
                    await Publish(entity);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Unexpected exception happened when updating {typeof(T)} #{entity.Id}: {ex}");
                    throw;
                }
            }
            else
            {
                _logger.LogInformation($"{typeof(T)} #{entity.Id} data is not valid: {JsonSerializer.Serialize(entity)}");
                throw new ValidationException();
            }
        }

        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                return await _repository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unexpected exception happened when updating {typeof(T)} #{id}: {ex}");
                throw;
            }
        }

        public void Print(T entity)
        {
            _logger.LogInformation(entity != null ? JsonSerializer.Serialize(entity) : "null");
        }

        public void Print(IEnumerable<T> entityList)
        {
            _logger.LogInformation(JsonSerializer.Serialize(entityList));
        }
    }
}
