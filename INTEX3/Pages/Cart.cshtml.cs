using INTEX3.Infrastructure;
using INTEX3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using INTEX3.Models;

namespace INTEX3.Pages
{
    public class CartModel : PageModel
    {
        private IProductRepository _productRepository;

        public Cart Cart { get; set; }

        public CartModel(IProductRepository temp, Cart cartService)
        {
            _productRepository = temp;
            Cart = cartService;
        }

        public string ReturnUrl { get; set; } = "/";
        public void OnGet()
        {
            ReturnUrl = ReturnUrl ?? "/";
        }

        public IActionResult OnPost(int ProductId, string ReturnUrl) 
        {
            Product prod = _productRepository.Products
                .FirstOrDefault(x =>x.ProductId == ProductId);

            if (prod != null)
            {
                Cart.AddItem(prod, 1);
            }

            return RedirectToPage(new {returnUrl = ReturnUrl});
        }

        public IActionResult OnPostRemove(int ProductId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == ProductId).Product);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
