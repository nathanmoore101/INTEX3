using INTEX3.Data;
using INTEX3.Data.Repositories;
using INTEX3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



public class RecProductRepository : IRecProductRepository
{
    private readonly Intex2Context _context;

    public RecProductRepository(Intex2Context context)
    {
        _context = context;
    }

    //public IQueryable<ItemRecommendation> ItemRecommendations => _context.ItemRecommendations;

    // public ProductDetailsViewModel getRecommendations(int productId){}

    public ItemRecommendation GetItemRecommendationForProductId(int productId)
    {
        var itemRec = _context.ItemRecommendations.Where(p => p.ProductId == productId);
        var ite = itemRec.FirstOrDefault();
        return ite;
    }

    public List<Product> getProductsForItemRecommendation(ItemRecommendation ir)
    {
        List<int> listOfIds = turnItemRecommendationIntoListOfIntegers(ir);

        // var products = _context.Products.Where(p => listOfIds.Contains(ir.ProductId));

        var products = _context.Products.ToList();

        List<Product> filteredProducts = new List<Product>();

        foreach (var product in products)
        {
            if (listOfIds.Contains(product.ProductId)) { 
                filteredProducts.Add(product);
            }
        }

        return filteredProducts;
    }

    public List<int> turnItemRecommendationIntoListOfIntegers(ItemRecommendation recommendation)
    {
        List<int> productIds = new List<int>();

        // Add each recommendation ID to the list if it is not null
        if (recommendation.Rec1ProductId.HasValue)
            productIds.Add(recommendation.Rec1ProductId.Value);
        if (recommendation.Rec2ProductId.HasValue)
            productIds.Add(recommendation.Rec2ProductId.Value);
        if (recommendation.Rec3ProductId.HasValue)
            productIds.Add(recommendation.Rec3ProductId.Value);
        if (recommendation.Rec4ProductId.HasValue)
            productIds.Add(recommendation.Rec4ProductId.Value);
        if (recommendation.Rec5ProductId.HasValue)
            productIds.Add(recommendation.Rec5ProductId.Value);

        return productIds;
    }

    /*
    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }

    public async Task<List<Product>> GetRecommendationsByProductIdAsync(int productId)
    {
        var recommendations = await _context.ItemRecommendations
            .Where(r => r.ProductId == productId)
            .SelectMany(r => _context.Products.Where(p => p.ProductId == r.Rec1ProductId || p.ProductId == r.Rec2ProductId))
            .ToListAsync();

        return recommendations;
    }

    public async Task<ItemRecommendation> GetItemRecommendationByProductIdAsync(int productId)
    {
        return await 
    }
    */
}
