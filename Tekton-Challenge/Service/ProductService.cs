using Serilog;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text.Json;
using Tekton_Challenge.Dto;
using Tekton_Challenge.Entity;
using Tekton_Challenge.Repository;
using Tekton_Challenge.Utilities;

namespace Tekton_Challenge.Service
{
    public class ProductService : IProductService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        //ProductRepository repository = new ProductRepository();
        IProductRepository _productRepository;

        public ProductService(IConnectionMultiplexer connectionMultiplexer, IProductRepository productRepository) 
        {
            _connectionMultiplexer = connectionMultiplexer;
            _productRepository = productRepository;
        }
        public async Task InsertProduct(Product product)
        {          
            await _productRepository.RegisterProduct(product);
        }
        public async Task<Product?> GetProduct(int id)
        {
           return await _productRepository.GetProductById(id);
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
             return await _productRepository.UpdateProduct(product);
        }
        public async Task AddCache(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value, TimeSpan.FromMinutes(5));
        }

        public async Task<RedisValue> GetCacheByKey(string key)
        {
            RedisValue response = new RedisValue();
            var db = _connectionMultiplexer.GetDatabase();
            if(db != null)
                response = await db.StringGetAsync(key);

            return response;
        }

        public List<String> ValidateRequest(ProductDto productDto, String cache, Int32 http)
        {
            List<String> ListResult = new List<String>();
            if (http == 1)
            {
                if(cache == null)
                {
                    if (productDto.ProductId <= 0)
                        ListResult.Add("The ProductID must not be less than or equal to 0");
                    if (productDto.Status < 0 || productDto.Status > 1)
                        ListResult.Add("The Product Status can only be 0 or 1");
                    if (productDto.Price <= 0)
                        ListResult.Add("The Product price must be greater than 0");
                    if (productDto.Stock < 1)
                        ListResult.Add("The product stock must be greater than or equal to 0");
                }
                else
                {
                    ListResult.Add("The product already exists, please enter another Product ID");
                }
            }
            else
            {
                if (cache != null)
                {
                    if (productDto.ProductId <= 0)
                        ListResult.Add("The ProductID must not be less than or equal to 0");
                    if (productDto.Status < 0 || productDto.Status > 1)
                        ListResult.Add("The Product Status can only be 0 or 1");
                    if (productDto.Price <= 0)
                        ListResult.Add("The Product price must be greater than 0");
                    if (productDto.Stock < 1)
                        ListResult.Add("The product stock must be greater than or equal to 0");
                }
                else
                {
                    ListResult.Add("The product you want to update does not exist, please enter a valid Product ID");
                }
            }
            return ListResult;
        }

        public async Task<Product> GetDiscountMockapiAsync(Product product)
        {
            String urlMockApi = "https://6590d4e28cbbf8afa96bbca3.mockapi.io/GetDiscountById"+"?ProductID="+product.ProductId.ToString();
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(urlMockApi);

                if (response.Result.IsSuccessStatusCode)
                {
                    var content = await response.Result.Content.ReadAsStringAsync();
                    var listDiscountProduct = JsonSerializer.Deserialize<List<ResponseMockapi>>(content);
                    foreach (var productDisc in listDiscountProduct)
                    {
                        if (Convert.ToInt32(productDisc.ProductID) == product.ProductId)
                        {
                            decimal discount = (decimal)productDisc.Discount / 100;
                            product.Discount = productDisc.Discount.ToString() +"%";
                            product.FinalPrice = product.Price - (product.Price * discount);
                            break;
                        }
                    }
                }
                else
                {
                    product.Discount = "0%";
                    product.FinalPrice = product.Price;
                }
            }
            return product;
        }
        public async Task LogRequest(Stopwatch stopwatch, string verb)
        {
            stopwatch.Stop();
            TimeSpan tiempoDeRespuesta = stopwatch.Elapsed;
            string ResponseTime = "[HTTP "+ verb + " REQUEST RESPONSE TIME]: " + tiempoDeRespuesta.TotalMilliseconds.ToString() + " ms";
            Log.Information(ResponseTime);
        }
    }
}
