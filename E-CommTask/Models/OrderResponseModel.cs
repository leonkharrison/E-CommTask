using E_CommTask.Data.Orders;

namespace E_CommTask.Models
{
    public class OrderResponseModel
    {
        public OrderResponseModel( Order order )
        {
            Id = order.Id;
            Name = order.Name;
            TotalPrice = order.TotalPrice;
            CreatedAt = order.CreatedAt;

            Products = new List<ProductResponseModel>();

            foreach( var product in order.Products )
            {
                Products.Add( new ProductResponseModel( product.Product ) );
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductResponseModel> Products { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
