using E_CommTask.Data.Orders;

namespace E_CommTask.Models
{
    public class ProductResponseModel
    {
        public ProductResponseModel( Product product )
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
