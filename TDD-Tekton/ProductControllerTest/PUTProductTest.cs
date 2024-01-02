using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton_Challenge.Controllers;
using Tekton_Challenge.Dto;
using Tekton_Challenge.Repository;
using Tekton_Challenge.Service;

namespace TDD_Tekton.ProductControllerTest
{
    public class PUTProductTest
    {
        [Fact]
        public void PutTestStatusCode()
        {
            var IProductServiceMock = new Mock<IProductService>();
            var IMapperMock = new Mock<IMapper>();
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 1;
            productMock.Object.Name = "laptop";
            productMock.Object.Status = 0;
            productMock.Object.Description = "laptop test";
            productMock.Object.Price = 980;
            var productController = new ProductController(IProductServiceMock.Object, IMapperMock.Object);

            var result = productController.Put(productMock.Object);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result.Result);
        }

        [Fact]
        public void PutTestGetValidateRequest1()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 1;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 1;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = 750;
            productMock.Object.Stock = 1500;

            var result = productService.ValidateRequest(productMock.Object, null, 2).ToList();

            Assert.NotNull(result);
            Assert.Equal("The product you want to update does not exist, please enter a valid Product ID", result[0]);
            Assert.Equal(1, result.Count);

        }
        [Fact]
        public void PutTestGetValidateRequest2()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 0;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 1;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = 750;
            productMock.Object.Stock = 1500;

            var result = productService.ValidateRequest(productMock.Object, "created product", 2).ToList();

            Assert.NotNull(result);
            Assert.Equal("The ProductID must not be less than or equal to 0", result[0]);
            Assert.Equal(1, result.Count);

        }
        [Fact]
        public void PutTestGetValidateRequest3()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 1;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 3;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = 750;
            productMock.Object.Stock = 1500;
            var result = productService.ValidateRequest(productMock.Object, "created product", 2).ToList();

            Assert.NotNull(result);
            Assert.Equal("The Product Status can only be 0 or 1", result[0]);
            Assert.Equal(1, result.Count);

        }
        [Fact]
        public void PutTestGetValidateRequest4()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 1;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 1;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = -1;
            productMock.Object.Stock = 1500;
            var result = productService.ValidateRequest(productMock.Object, "created product", 2).ToList();

            Assert.NotNull(result);
            Assert.Equal("The Product price must be greater than 0", result[0]);
            Assert.Equal(1, result.Count);

        }
        [Fact]
        public void PutTestGetValidateRequest5()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 1;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 1;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = 500;
            productMock.Object.Stock = 0;
            var result = productService.ValidateRequest(productMock.Object, "created product", 2).ToList();

            Assert.NotNull(result);
            Assert.Equal("The product stock must be greater than or equal to 0", result[0]);
            Assert.Equal(1, result.Count);

        }
        [Fact]
        public void PutTestGetValidateRequest6()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 0;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 3;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = -1;
            productMock.Object.Stock = 0;

            var result = productService.ValidateRequest(productMock.Object, "created product", 2).ToList();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            //response
            //1.The ProductID must not be less than or equal to 0
            //2.The Product Status can only be 0 or 1
            //3.The Product price must be greater than 0
            //4The product stock must be greater than or equal to 0
        }
        [Fact]
        public void PutTestGetValidateRequest7()
        {
            var IRedisMock = new Mock<IConnectionMultiplexer>();
            var IProductRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(IRedisMock.Object, IProductRepositoryMock.Object);
            var productMock = new Mock<ProductDto>();
            productMock.Object.ProductId = 1;
            productMock.Object.Name = "computer";
            productMock.Object.Status = 1;
            productMock.Object.Description = "computer test";
            productMock.Object.Price = 750;
            productMock.Object.Stock = 1500;

            var result = productService.ValidateRequest(productMock.Object, "created product", 2).ToList();

            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            //response
            //correct validate
        }
    }
}
