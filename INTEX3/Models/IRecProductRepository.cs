using INTEX3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INTEX3.Data.Repositories
{
    public interface IRecProductRepository
    {
        //public IQueryable<ItemRecommendation> ItemRecommendations { get; }
        // Task<Product> GetProductByIdAsync(int productId);
        // Task<List<Product>> GetRecommendationsByProductIdAsync(int productId);
        //Task<ItemRecommendation> GetItemRecommendationByProductIdAsync(int productId);
        ItemRecommendation GetItemRecommendationForProductId(int productId);
        List<Product> getProductsForItemRecommendation(ItemRecommendation ir);
        List<Product> getProductsIfItemRecIsNull();
    }
}