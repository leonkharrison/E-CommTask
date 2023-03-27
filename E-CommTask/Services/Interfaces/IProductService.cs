using E_CommTask.Data.Orders;

namespace E_CommTask.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll();
        public List<Product> ProductsCache { get; }
        public Task Insert( Product product );
    }
}
