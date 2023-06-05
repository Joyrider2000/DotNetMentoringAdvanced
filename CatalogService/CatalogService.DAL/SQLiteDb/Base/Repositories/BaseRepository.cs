using AutoMapper;
using Microsoft.Data.Sqlite;
using CatalogService.BLL.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Domain.Entities;
using CatalogService.DAL.SQLiteDb.Base.Entities;
using CatalogService.DAL.SQLiteDb.Base.Contexts;
using CatalogService.DAL.Configuration.Options;
using Microsoft.Extensions.Logging;

namespace CatalogService.DAL.SQLiteDb.Base.Repositories
{
    public abstract class BaseRepository<TDbEntity, TEntity> : IBaseRepository<TEntity>
        where TEntity : IdEntity
        where TDbEntity : IdDbEntity
    {
        protected readonly int SQLITE_CONSTRAINT = 19;
        private readonly AppOptions _appOptions;
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseRepository<TDbEntity, TEntity>> _logger;

        public BaseRepository(AppOptions appOptions, IMapper mapper, ILogger<BaseRepository<TDbEntity, TEntity>> logger)
        {
            _appOptions = appOptions;
            _mapper = mapper;
            _logger = logger;
        }

        protected DbContext GetContext()
        {
            return new BaseDbContext(_appOptions.DbConnectionString);
        }

        public async Task<IEnumerable<TEntity>> List()
        {
            _logger.LogInformation("Listing all in repository");
            using (var db = GetContext())
            {
                return _mapper.Map<List<TEntity>>(await GetAll(db));
            }
        }

        public async Task<TEntity?> Add(TEntity entity)
        {
            try
            {
                using (var db = GetContext())
                {
                    var mappedEntity = _mapper.Map<TDbEntity>(entity);
                    var result = db.Set<TDbEntity>().Add(mappedEntity);
                    await db.SaveChangesAsync();
                    _logger.LogInformation($"Successfully added {typeof(TDbEntity).Name} #{result.Entity.Id}");
                    return _mapper.Map<TEntity>(result.Entity);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqliteException)
                {
                    var sqliteException = (SqliteException)ex.InnerException;
                    if (sqliteException.SqliteErrorCode == SQLITE_CONSTRAINT)
                    {
                        throw new ConstraintViolationException();
                    }
                }
                _logger.LogInformation($"Error adding {typeof(TDbEntity).Name}: {ex}");
                return null;
            }
        }

        public async Task<TEntity?> Get(int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    return await GetById(db, id);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqliteException)
                {
                    var sqliteException = (SqliteException)ex.InnerException;
                    if (sqliteException.SqliteErrorCode == SQLITE_CONSTRAINT)
                    {
                        throw new ConstraintViolationException();
                    }
                }
                _logger.LogInformation($"Error getting {typeof(TDbEntity).Name} #{id}: {ex}");
                return null;
            }
        }

        public async Task<bool> Update(TEntity entity)
        {
            try
            {
                using (var db = GetContext())
                {
                    db.Set<TDbEntity>().Update(_mapper.Map<TDbEntity>(entity));
                    await db.SaveChangesAsync();

                    _logger.LogInformation($"Successfully updated {typeof(TDbEntity).Name} #{entity.Id}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqliteException)
                {
                    var sqliteException = (SqliteException)ex.InnerException;
                    if (sqliteException.SqliteErrorCode == SQLITE_CONSTRAINT)
                    {
                        throw new ConstraintViolationException();
                    }
                }
                _logger.LogInformation($"Error updating {typeof(TDbEntity).Name} #{entity.Id}: {ex}");
                return false;
            }
        }

        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    db.Set<TDbEntity>().Where(c => c.Id == id).ExecuteDelete();
                    await db.SaveChangesAsync();

                    _logger.LogInformation($"Successfully deleted {typeof(TDbEntity).Name} #{id}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqliteException)
                {
                    var sqliteException = (SqliteException)ex.InnerException;
                    if (sqliteException.SqliteErrorCode == SQLITE_CONSTRAINT)
                    {
                        throw new ConstraintViolationException();
                    }
                }
                _logger.LogInformation($"Error deleting {typeof(TDbEntity).Name} #{id}: {ex}");
                return false;
            }
        }

        protected virtual async Task<TEntity> GetById(DbContext db, int id)
        {
            return _mapper.Map<TEntity>(await db.Set<TDbEntity>().FirstOrDefaultAsync(c => c.Id == id));
        }

        protected virtual async Task<IEnumerable<TDbEntity>> GetAll(DbContext db)
        {
            var res = await db.Set<TDbEntity>().ToListAsync();
            return res;
        }
    }
}
