using E_CommTask.Data.Orders;

namespace E_CommTask.Models
{
    public class ProductRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public bool ValidateRequest( out List<string> errors )
        {
            errors = new List<string>();
            var isValid = true;

            if( string.IsNullOrEmpty( Name ) )
            {
                errors.Add( "No product name provided" );
                isValid = false;
            }
            if( string.IsNullOrEmpty( Description ) )
            {
                errors.Add( "No product description provided" );
                isValid = false;
            }
            if( Price <= 0 )
            {
                errors.Add( "Product has invalid price" );
                isValid = false;
            }

            return isValid;
        }

        public Product MapToProduct()
        {
            return new Product
            {
                Name = Name,
                Description = Description,
                Price = Price
            };
        }
    }
}
