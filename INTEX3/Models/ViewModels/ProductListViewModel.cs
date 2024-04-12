using INTEX3.Models;

namespace INTEX3.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } // CHANGE THIS WITH UPDATED MODEL

        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductCategory { get; set; }

        public string? CurrentProductColor { get; set; }


    }
}
