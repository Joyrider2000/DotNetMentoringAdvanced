using AutoMapper;
using Microsoft.Data.Sqlite;
using CatalogService.BLL.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Domain.Entities;
using CatalogService.DAL.SQLiteDb.Base.Entities;
using CatalogService.DAL.SQLiteDb.Base.Contexts;

namespace CatalogService.DAL.SQLiteDb.Base.Repositories
{
    public abstract class BaseRepository<TDbEntity, TEntity> : IBaseRepository<TEntity>
        where TEntity : IdEntity
        where TDbEntity : IdDbEntity
    {
        protected readonly int SQLITE_CONSTRAINT = 19;

        protected readonly IMapper _mapper;

        protected string _connectionString { get; set; }

        public BaseRepository(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }

        protected DbContext GetContext()
        {
            return new BaseDbContext(_connectionString);
        }

        public async Task<IEnumerable<TEntity>> List()
        {
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
                    Console.WriteLine($"Successfully added {typeof(TDbEntity)} #{result.Entity.Id}");
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
                Console.WriteLine($"Error adding {typeof(TDbEntity)}: {ex}");
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
                Console.WriteLine($"Error getting {typeof(TDbEntity)} #{id}: {ex}");
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

                    Console.WriteLine($"Successfully updated {typeof(TDbEntity)} #{entity.Id}");
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
                Console.WriteLine($"Error updating {typeof(TDbEntity)} #{entity.Id}: {ex}");
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

                    Console.WriteLine($"Successfully deleted {typeof(TDbEntity)} #{id}");
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
                Console.WriteLine($"Error deleting {typeof(TDbEntity)} #{id}: {ex}");
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
