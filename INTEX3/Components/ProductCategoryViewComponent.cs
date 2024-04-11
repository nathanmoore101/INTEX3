using Microsoft.AspNetCore.Mvc;
using INTEX3.Models;

namespace INTEX3.Components
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private IProductRepository _productRepository;
        public ProductCategoryViewComponent(IProductRepository temp)
        {
            _productRepository = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductCategory = RouteData?.Values["productCategory"];

            var productCategory = _productRepository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(productCategory);
        }
    }
}
