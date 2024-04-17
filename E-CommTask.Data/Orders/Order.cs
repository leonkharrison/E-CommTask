namespace E_CommTask.Data.Orders
{
    public class Order : BaseEntity
    {
        public string Name { get; set; }
        public List<ProductOrders> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
