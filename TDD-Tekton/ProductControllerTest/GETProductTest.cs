
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StackExchange.Redis;
using Tekton_Challenge.Controllers;
using Tekton_Challenge.Dto;
using Tekton_Challenge.Entity;
using Tekton_Challenge.Repository;
using Tekton_Challenge.Service;
using Tekton_Challenge.Utilities;
using Xunit;

namespace TDD_Tekton.ProductControllerTest
{
    public class GETProductTest
    {
        [Fact]
        public void GetTestNotFound()
        {
            var IProductServiceMock = new Mock<IProductService>();
            var IMapperMock = new Mock<IMapper>();
            var productController = new ProductController(IProductServiceMock.Object, IMapperMock.Object);

            var result = productController.Get(1);

            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result.Result);
            Assert.Contains("Product not found", result.Result.Message);
            //response
            //product not exists
        }

        // Esta prueba unitaria funcionaria si es que si se registra previamente un producto
        //[Fact]
        //public void GetTestFoundProduct()
        //{
        //    var IProductServiceMock = new Mock<IProductService>();
        //    var IMapperMock = new Mock<IMapper>();
        //    var productController = new ProductController(IProductServiceMock.Object, IMapperMock.Object);

        //    var result = productController.Get(1);

        //    Assert.NotNull(result);
        //    Assert.IsType<ProductResponse>(result.Result);
        //    Assert.Contains("Correct Product", result.Result.Message);

        //}

        [Fact]
        public void GetTestGetCacheByKey()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);

            var result = productService.GetCacheByKey("1");

            Assert.NotNull(result);
            Assert.Empty(result.Result.ToString());
            //response
            //redis result not exits
        }

        [Fact]
        public void GetExternalServiceMockapiTest()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productMock = new Mock<Product>();
            productMock.Object.ProductId = 1;
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            
            var result = productService.GetDiscountMockapiAsync(productMock.Object);

            Assert.NotNull(result);
            Assert.Contains("%",result.Result.Discount);
            Assert.NotEmpty(result.Result.FinalPrice.ToString());

            //response
            //result correct mockapi
            //only use Product ID from 1 to 10
        }
    }
}