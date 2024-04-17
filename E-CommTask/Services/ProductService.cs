using E_CommTask.Data.Orders;
using E_CommTask.DataBase;
using E_CommTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_CommTask.Services
{
    public class ProductService : BaseRepo<Product>, IProductRepo
    {
        public ProductService( ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
