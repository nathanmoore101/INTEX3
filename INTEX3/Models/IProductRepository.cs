using INTEX3.Models;

namespace INTEX3.Models
{
    public interface IProductRepository
    {
        //Product GetProductById(int productId);
        void AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);
        public IQueryable<Product> Products { get; } // CHANGE THIS WITH UPDATED MODEL

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Order> GetAllOrders();
        IEnumerable<Customer> GetAllCustomers();

        Task<Product> GetProductById(int productId);
    }
}
