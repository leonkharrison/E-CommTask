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
        ILogger<OrdersController> _logger;

        public OrdersController( IOrdersService ordersService, IProductService productService, ILogger<OrdersController> logger ) 
        {
            _ordersService = ordersService;
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/orders")]
        public async Task<dynamic> CreateOrder( OrderRequestModel orderRequest )
        {
            _logger.LogInformation( "Attempting to create a new order..." );
            // Validate the request

            // If there are any errors return bad request and inform caller of them
            if( !orderRequest.ValidateRequest( out var errors ) )
            {
                _logger.LogInformation( "One or more errors with order request" );
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
            var order = orderRequest.MapToOrder();

            // Get the products from the cache to add to the order
            foreach( var productId in orderRequest.ProductIds )
            {
                var product = _productService.ProductsCache.SingleOrDefault( p => p.Id == productId );

                if( product == null )
                {
                    return NotFound( $"No product with Id {productId}" );
                }

                order.Products.Add( new ProductOrders() { Product = product } );
                order.TotalPrice += product.Price;
            }

            await _ordersService.Insert( order );

            _logger.LogInformation( $"Order created successfully" );

            return Ok( $"Order Complete! Total price: {order.TotalPrice}" );
        }

        [HttpGet]
        [Route("api/orders/{id:int}")]
        public async Task<dynamic> GetOrderById( int id )
        {
            var order = await _ordersService.GetById( id );

            if( order == null )
            {
                return NotFound( $"No order with Id {id}" );
            }

            return Ok( new OrderResponseModel( order ));
        }

        [HttpGet]
        [Route("api/orders")]
        public async Task<dynamic> GetOrders()
        {
            _logger.LogInformation( "Attmepting to get all orders" );

            var orders = await _ordersService.GetAll();

            if( orders == null || orders.Count == 0 )
            {
                _logger.LogInformation( "No orders found" );
                return NotFound( "No orders have been placed" );
            }

            _logger.LogInformation( $"Found {orders.Count} orders" );
            return orders.Select( o => new OrderResponseModel( o ) );
        }
    }
}
