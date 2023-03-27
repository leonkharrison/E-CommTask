using E_CommTask.Data.Orders;

namespace E_CommTask.Services.Interfaces
{
    public interface IOrdersService
    {
        public Task<List<Order>> GetAll();
        public Task<Order> GetById( int id );
        public Task<List<Order>> GetByName( string name );
        public Task<int> Insert( Order order );
        public Task<int> Update( Order order );
        public Task Delete( int id );
    }
}
