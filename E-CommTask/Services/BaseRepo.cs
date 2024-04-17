using E_CommTask.Data;
using E_CommTask.DataBase;
using E_CommTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_CommTask.Services
{
    public class BaseRepo<TEntity> : IBaseRepo<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ApplicationDbContext _dbContext;
        readonly bool _isPermanentEntity;

        public BaseRepo( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _isPermanentEntity = typeof( TEntity ).BaseType == typeof( PermanentEntity );
        }

        public async Task DeleteAsync( TEntity entity )
        {
            if( _isPermanentEntity )
            {
                ( entity as PermanentEntity )!.IsDeleted = true;
                _dbSet.Update( entity );
            }
            else
            {
                _dbSet.Remove( entity );
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync( IEnumerable<TEntity> entities )
        {
            if( _isPermanentEntity )
            {
                foreach( var entity in entities )
                {
                    ( entity as PermanentEntity )!.IsDeleted = true;
                }

                _dbSet.UpdateRange( entities );
            }
            else
            {
                _dbSet.RemoveRange( entities );
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync( int id )
        {
            return await _dbSet.AnyAsync( x => x.Id == id );
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetAsync( int id )
        {
            return await GetAsync( id, false );
        }

        public async Task<TEntity?> GetAsync( int id, bool lazyLoad )
        {
            if( !lazyLoad )
            {
                return await _dbSet.FirstOrDefaultAsync( x => x.Id == id );
            }

            IQueryable<TEntity> query = _dbSet;

            var navigationProperties = typeof( TEntity ).GetProperties().
                Where( p => p.GetGetMethod().IsVirtual &&
                !p.GetGetMethod().IsFinal &&
                typeof( BaseEntity ).IsAssignableFrom( p.PropertyType ) );

            foreach( var property in navigationProperties )
            {
                query = query.Include( property.Name );
            }

            return await query.FirstOrDefaultAsync( x => x.Id == id );
        }

        public async Task<TEntity> InsertAsync( TEntity entity )
        {
            entity.Id = 0;
            entity.CreatedAt = DateTime.UtcNow;
            entity.ModifiedAt = DateTime.UtcNow;

            var result = _dbSet.Add( entity );
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<TEntity>> InsertAsync( IEnumerable<TEntity> entities )
        {
            entities.ToList().ForEach( x =>
            {
                x.CreatedAt = DateTime.UtcNow;
                x.ModifiedAt = DateTime.UtcNow;
                x.Id = 0;
            } );

            _dbSet.AddRange( entities );
            await _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task<TEntity> UpdateAsync( TEntity entity )
        {
            entity.ModifiedAt = DateTime.UtcNow;

            _dbSet.Update( entity );
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> UpdateAsync( IEnumerable<TEntity> entities )
        {
            entities.ToList().ForEach( x => x.ModifiedAt = DateTime.UtcNow );

            _dbSet.UpdateRange( entities );
            await _dbContext.SaveChangesAsync();
            return entities;
        }
    }
}
