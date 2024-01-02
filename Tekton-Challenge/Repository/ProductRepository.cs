using Tekton_Challenge.Entity;

namespace Tekton_Challenge.Repository
{
    public class ProductRepository: IProductRepository
    {
        private static List<Product> ProductList = new List<Product>();
        public async Task RegisterProduct(Product product)
        {
             ProductList.Add(product);
        }
        public async Task<Product?> GetProductById(Int32 idProduct)
        {
            var product = ProductList.Where(p => p.ProductId == idProduct).FirstOrDefault();
            return product;
        }

        public async Task<Product?> UpdateProduct(Product productDto)
        {
            var product = ProductList.Where(p => p.ProductId == productDto.ProductId).FirstOrDefault();
            if (product == null)
                return null;
            else
            {
                ProductList.Remove(product);
                Product newProduct = new Product();
                newProduct.ProductId = product.ProductId;
                newProduct.Name = productDto.Name;
                newProduct.Status = productDto.Status;
                newProduct.StatusName = productDto.Status == 1 ? "Active" : "Inactive";
                newProduct.Stock = productDto.Stock;
                newProduct.Description = productDto.Description;
                newProduct.Price = productDto.Price;
                newProduct.Discount = product.Discount;
                var discount = await ConvertToDecimalDiscount(product.Discount);
                newProduct.FinalPrice = newProduct.Price - (newProduct.Price * discount);
                newProduct.Category = product.Category;
                newProduct.RegistrationDate = DateTime.Now.ToString();
                newProduct.Customer = product.Customer;

                ProductList.Add(newProduct);
                return newProduct;
            }
        }
        public  async Task<Decimal> ConvertToDecimalDiscount(String discountString)
            {
            var discountLength = discountString.Length;
            var discountSubstring = discountString.Substring(0,discountLength - 1);
            var discount = (decimal)Convert.ToDecimal(discountSubstring) / 100;
            return discount;
        }
    }
}
