using StackExchange.Redis;
using System.Diagnostics;
using Tekton_Challenge.Dto;
using Tekton_Challenge.Entity;
using Tekton_Challenge.Utilities;

namespace Tekton_Challenge.Service
{
    public interface IProductService
    {
        Task InsertProduct(Product product);
        Task<Product?> GetProduct(int id);
        Task<Product?> UpdateProduct(Product product);
        Task AddCache(string key, string value);
        Task<RedisValue> GetCacheByKey(string key);
        Task<Product> GetDiscountMockapiAsync(Product product);
        List<String> ValidateRequest(ProductDto productDto, String cache, Int32 validateHttp);
        Task LogRequest(Stopwatch stopwatch, string verb);
    }
}

