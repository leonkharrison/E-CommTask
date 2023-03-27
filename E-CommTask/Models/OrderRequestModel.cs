using E_CommTask.Data.Orders;

namespace E_CommTask.Models
{
    public class OrderRequestModel
    {
        public string Name { get; set; }
        public List<int> ProductIds { get; set; }

        public bool ValidateRequest( out List<string> errors )
        {
            bool isValid = true;
            errors = new List<string>();

            if( string.IsNullOrEmpty( Name ) )
            {
                errors.Add( "Customer name must not be empty" );
                isValid = false;
            }

            if( ProductIds == null || ProductIds.Count == 0 )
            {
                errors.Add( "There are no products in this order" );
                isValid = false;
            }

            return isValid;
        }

        public Order MapFromRequest()
        {
            return new Order
            {
                Name = this.Name,
                Products = new List<ProductOrders>()
            };
        }
    }
}
