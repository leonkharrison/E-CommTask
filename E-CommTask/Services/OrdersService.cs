using E_CommTask.Data.Orders;
using E_CommTask.DataBase;
using E_CommTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_CommTask.Services
{
    public class OrdersService : IOrdersService
    {
        ApplicationDbContext _DbContext;

        public OrdersService( ApplicationDbContext dbContext ) 
        { 
            _DbContext = dbContext;
        }

        public async Task Delete( int id )
        {
            try
            {
                var order = await _DbContext.Orders.SingleOrDefaultAsync( s => s.Id == id );
                _DbContext.Orders.Remove( order );

                await _DbContext.SaveChangesAsync();
            }
            catch( NullReferenceException )
            {
                throw new NullReferenceException( $"No order with id {id}" );
            }
        }

        public async Task<List<Order>> GetAll()
        {
            return await _DbContext.Orders.Include( o => o.Products ).ToListAsync();
        }

        public async Task<Order> GetById( int id )
        {
            return await _DbContext.Orders.Include( o => o.Products ).FirstOrDefaultAsync( o => o.Id == id );
        }

        public async Task<List<Order>> GetByName( string name )
        {
            return await _DbContext.Orders.Where( o => string.Equals( o.Name , name, StringComparison.InvariantCultureIgnoreCase )).Include( o => o.Products ).ToListAsync();
        }

        public async Task<int> Insert( Order order )
        {
            try
            {
                _DbContext.Orders.Add( order );
                await _DbContext.SaveChangesAsync();
                return order.Id;
            }
            catch( Exception )
            {
                throw;
            }
        }

        public async Task<int> Update( Order order )
        {
            try
            {
                _DbContext.Orders.Update( order );
                await _DbContext.SaveChangesAsync();
                return order.Id;
            }
            catch( NullReferenceException )
            {
                throw new NullReferenceException( $"No order with id {order.Id}" );
            }
        }
    }
}
