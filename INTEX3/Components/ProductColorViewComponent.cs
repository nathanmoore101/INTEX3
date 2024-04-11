using INTEX3.Models;
using Microsoft.AspNetCore.Mvc;

namespace INTEX3.Components
{
    public class ProductColorViewComponent : ViewComponent
    {
        private IProductRepository _productRepository;
        public ProductColorViewComponent(IProductRepository temp)
        {
            _productRepository = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductColor = RouteData?.Values["productColor"];

            var productColor = _productRepository.Products
                .Select(x => x.PrimaryColor)
                .Distinct()
                .OrderBy(x => x);

            return View(productColor);
        }
    }
}