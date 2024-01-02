using Tekton_Challenge.Entity;

namespace Tekton_Challenge.Repository
{
    public interface IProductRepository
    {
        Task RegisterProduct(Product product);
        Task<Product?> GetProductById(Int32 idProduct);
        Task<Product?> UpdateProduct(Product productDto);
        Task<Decimal> ConvertToDecimalDiscount(String discountString);
    }
}
