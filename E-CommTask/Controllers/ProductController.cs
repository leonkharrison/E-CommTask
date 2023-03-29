﻿using E_CommTask.Models;
using E_CommTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_CommTask.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductService _productService;
        ILogger<ProductController> _logger;

        public ProductController( IProductService productService, ILogger<ProductController> logger )
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/products")]
        public async Task<dynamic> GetAllProducts()
        {
            _logger.LogInformation( "Trying to get all products..." );

            var allProducts = await _productService.GetAll();

            if( allProducts == null || allProducts.Count == 0 )
            {
                _logger.LogInformation( "No products found" );
                return NotFound( "No products" );
            }

            _logger.LogInformation( $"{allProducts.Count} products found" );
            return allProducts.Select( p => new ProductResponseModel( p ) );
        }

        [HttpPost]
        [Route("api/products")]
        public async Task<dynamic> CreateProduct( ProductRequestModel request )
        {
            _logger.LogInformation( "Trying to add product..." );

            if( request == null )
            {
                _logger.LogInformation( "Request body was null" );
                return BadRequest();
            }

            if( !request.ValidateRequest( out var error ) )
            {
                _logger.LogInformation( "One or more errors with request" );

                return BadRequest( new
                {
                    Message = "There are one or more errors with the request",
                    Error = error
                } );
            }

            var newProduct = request.MapToProduct();

            await _productService.Insert( newProduct );

            _logger.LogInformation( $"Product {newProduct.Name} added" );

            return Ok();
        }
    }
}
