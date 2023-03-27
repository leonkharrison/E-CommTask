using E_CommTask.Data.Orders;
using E_CommTask.Models;
using E_CommTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace E_CommTask.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IOrdersService _ordersService;
        IProductService _productService;

        public OrdersController( IOrdersService ordersService, IProductService productService ) 
        {
            _ordersService = ordersService;
            _productService = productService;
        }

        [HttpPost]
        [Route("api/orders")]
        public async Task<dynamic> CreateOrder( OrderRequestModel orderRequest )
        {
            // Validate the request

            // If there are any errors return bad request and inform caller of them
            if( !orderRequest.ValidateRequest( out var errors ) )
            {
                return BadRequest
                    (
                        new
                        {
                            Message = "There is an error in your order",
                            Errors = errors
                        }    
                    );
            }

            // Map the request to an order object
            var order = orderRequest.MapFromRequest();

            // Get the products from the cache to add to the order
            foreach( var productId in orderRequest.ProductIds )
            {
                var product = _productService.ProductsCache.SingleOrDefault( p => p.Id == productId );

                if( product == null )
                {
                    return BadRequest( $"No product with Id {productId}" );
                }

                order.Products.Add( new ProductOrders() { Product = product } );
                order.TotalPrice += product.Price;
            }

            await _ordersService.Insert( order );

            return Ok( $"Order Complete! Total price: {order.TotalPrice}" );
        }

        [HttpGet]
        [Route("api/orders/{id:int}")]
        public async Task<dynamic> GetOrderById( int id )
        {
            var order = await _ordersService.GetById( id );

            if( order == null )
            {
                return BadRequest( $"No order with Id {id}" );
            }

            return Ok( new OrderResponseModel( order ));
        }

        [HttpGet]
        [Route("api/orders")]
        public async Task<dynamic> GetOrders()
        {
            var orders = await _ordersService.GetAll();
            return orders.Select( o => new OrderResponseModel( o ) );
        }
    }
}
