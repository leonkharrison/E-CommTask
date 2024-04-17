namespace E_CommTask.Data.Orders
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<ProductOrders> Orders { get; set; }
    }
}
