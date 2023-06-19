using FakeItEasy.Sample.Models;

namespace FakeItEasy.Sample.Services
{
    public interface IProductService
    {
        void AddProduct(Product product);
        void DeleteProduct(int id);
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void UpdateProduct(Product product);
    }
}
