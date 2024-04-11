
using INTEX3.Models;
using Microsoft.EntityFrameworkCore;

namespace INTEX3.Models
{
    public class EFProductRepository : IProductRepository
    {
        private Intex2Context _context; // UPDATE WITH CONTEXT FILE
        public EFProductRepository(Intex2Context temp) {
            _context = temp;
        }
        public IQueryable<Product> Products => _context.Products;

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }
        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }
        //public Product GetProductById(int productId)
        //{
        //    return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        //}

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public async Task UpdateProduct(Product product)
        {
            // Your update logic here, for example:
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

            }
        }

