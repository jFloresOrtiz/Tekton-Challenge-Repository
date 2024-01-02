using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using System.Text.Json;
using Tekton_Challenge.Dto;
using Tekton_Challenge.Entity;
using Tekton_Challenge.Service;
using Tekton_Challenge.Utilities;

namespace Tekton_Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        private readonly IMapper _mapper;   
        public ProductController(IProductService productService, IMapper mapper) 
        {
            this._mapper = mapper;
            service = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductDto productDto)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var pruductCache = service.GetCacheByKey(productDto.ProductId.ToString());
            var listPossibleErrors = service.ValidateRequest(productDto, pruductCache.Result,1);
            if (listPossibleErrors != null && listPossibleErrors.Count > 0)
            {
                await service.LogRequest(stopwatch, "POST");
                return StatusCode(StatusCodes.Status400BadRequest, listPossibleErrors);
            }
            else
            {
                try
                {
                    var productMapped = _mapper.Map<Product>(productDto);
                    var product = await service.GetDiscountMockapiAsync(productMapped);
                    await service.AddCache(product.ProductId.ToString(), JsonSerializer.Serialize(product));
                    await service.InsertProduct(product);
                    await service.LogRequest(stopwatch, "POST");
                    return StatusCode(StatusCodes.Status201Created, "Created Product, ID=" + productMapped.ProductId);
                }
                catch (Exception ex) 
                {
                    await service.LogRequest(stopwatch, "POST");
                    return StatusCode(StatusCodes.Status409Conflict, ex.ToString());
                }               
            }          
        }

        [HttpGet("{id}")]
        public async Task<ProductResponse> Get(Int32 id)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ProductResponse response = new ProductResponse();
            var pruductCache = service.GetCacheByKey(id.ToString());
            if(pruductCache.Result.IsNull == false)
            {
                response.Product = JsonSerializer.Deserialize<Product>(pruductCache.Result.ToString());
                response.Error = false;
                response.Message = "Correct Product";
                response.Status = StatusCodes.Status200OK;
            }
            else
            {
                Product? validateProduct = await service.GetProduct(id);
                if (validateProduct == null)
                {
                    response.Product = null;
                    response.Error = true;
                    response.Message = "Product not found";
                    response.Status = StatusCodes.Status404NotFound;
                }
                else
                {
                    response.Product = validateProduct;
                    response.Error = false;
                    response.Message = "Correct Product";
                    response.Status = StatusCodes.Status200OK;
                }
            }
            await service.LogRequest(stopwatch, "GET");
            return response;
        }

        [HttpPut]
        public async Task<IActionResult> Put(ProductDto productDto)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var pruductCache = service.GetCacheByKey(productDto.ProductId.ToString());
            var listPossibleErrors = service.ValidateRequest(productDto, pruductCache.Result, 2);
            if (listPossibleErrors != null && listPossibleErrors.Count > 0)
            {
                await service.LogRequest(stopwatch, "PUT");
                return StatusCode(StatusCodes.Status400BadRequest, listPossibleErrors);
            }
            else
            {
                var productMapped = _mapper.Map<Product>(productDto);
                Product? validateProduct = await service.UpdateProduct(productMapped);
                if (validateProduct == null)
                {
                    await service.LogRequest(stopwatch, "PUT");
                    return StatusCode(StatusCodes.Status404NotFound, 
                        "The product you want to update does not exist, please enter a valid Product ID");
                }
                else
                {
                    await service.AddCache(validateProduct.ProductId.ToString(), JsonSerializer.Serialize(validateProduct));
                    await service.LogRequest(stopwatch, "PUT");
                    return StatusCode(StatusCodes.Status202Accepted, 
                        "Updated Product, ID ="+ productMapped.ProductId.ToString());
                }
            }          
        }
    }
}
