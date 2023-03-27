using E_CommTask.Models;
using E_CommTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_CommTask.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductService _productService;

        public ProductController( IProductService productService )
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("api/products")]
        public async Task<dynamic> GetAllProducts()
        {
            var allProducts = await _productService.GetAll();
            return allProducts.Select( p => new ProductResponseModel( p ) );
        }

        [HttpPost]
        [Route("api/products")]
        public async Task<dynamic> CreateProduct( ProductRequestModel request )
        {
            if( request == null )
            {
                return BadRequest();
            }

            if( !request.ValidateRequest( out var error ) )
            {
                return BadRequest( new
                {
                    Message = "There are one or more errors with the request",
                    Error = error
                } );
            }

            var newProduct = request.MapToProduct();

            await _productService.Insert( newProduct );

            return Ok();
        }
    }
}
