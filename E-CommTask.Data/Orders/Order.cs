namespace E_CommTask.Data.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductOrders> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
