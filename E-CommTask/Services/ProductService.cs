using E_CommTask.Data.Orders;
using E_CommTask.DataBase;
using E_CommTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_CommTask.Services
{
    public class ProductService : IProductService
    {
        ApplicationDbContext _DbContext;
        public List<Product> ProductsCache
        {
            get; private set;
        }

        public ProductService( ApplicationDbContext dbContext) 
        {
            _DbContext = dbContext;
            UpdateCache();
        }

        async void UpdateCache()
        {
            ProductsCache = await GetAll();
        }

        // I expect the front-end to have called this on start up to show the available products to the user
        public async Task<List<Product>> GetAll()
        {
            return await _DbContext.Products.ToListAsync();
        }

        public async Task Insert( Product product )
        {
            try
            {
                _DbContext.Products.Add( product );
                await _DbContext.SaveChangesAsync();
            }
            catch( Exception )
            {
                throw;
            }
        }
    }
}
