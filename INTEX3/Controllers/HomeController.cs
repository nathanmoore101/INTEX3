using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INTEX3.Models;
using INTEX3.Models.ViewModels;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using INTEX3.Data;
using INTEX3.Data.Repositories;
using NuGet.Protocol.Core.Types;


namespace INTEX3.Controllers
{
    public class HomeController : Controller
    {

        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private IRecProductRepository _recRepository;
        public HomeController(IProductRepository productRepository, IOrderRepository orderRepository, IUserRepository userRepository, IRecProductRepository recRepository)

        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _recRepository = recRepository;
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminUsersPage()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUserPage(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUser(AspNetUser user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.UpdateUserAsync(user);
                return RedirectToAction("AdminUsersPage");
            }

            return View(user);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserConfirmation(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(user);
            return RedirectToAction("AdminUsersPage");
        }




        //ADMIN ORDERS PAGE
        [Authorize(Roles = "admin")]
        public IActionResult AdminOrdersPage(int pageNumber = 1, int pageSize = 100)
        {
            // Get total number of orders
            var totalOrders = _orderRepository.GetAllOrders().Count();

            // Calculate number of pages
            var totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);

            // Get sorted orders for the current page
            var orders = _orderRepository.GetOrdersSortedByDateDescending()
                                          .Skip((pageNumber - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToList();

            // Pass orders and pagination info to the view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            return View(orders);
        }

        // GET: Review Order
        [Authorize(Roles = "admin")]
        public IActionResult ReviewOrderPage(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View("ReviewOrderPage", order);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult EditOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.UpdateOrder(order);
                return RedirectToAction("AdminOrdersPage");
            }

            return View("ReviewOrderPage", order);
        }






        //ADMIN PRODUCTS PAGE + CRUD
        [Authorize(Roles = "admin")]
        public IActionResult AdminProductsPage()
        {
            // Get all products ordered by ProductId in ascending order
            var products = _productRepository.GetAllProducts().OrderBy(p => p.ProductId).ToList();

            return View(products);
        }

        // GET: Create Product
        [Authorize(Roles = "admin")]
        public IActionResult AddProductPage()
        {
            return View();
        }
        // POST: Create Product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                // Do not set ProductId here
                // product.ProductId = ???

                _productRepository.AddProduct(product);
                return RedirectToAction("AdminProductsPage");
            }
            return View(product);
        }


        // GET: Edit Product
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProductPage(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // POST: Edit Product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateProduct(product);
                return RedirectToAction("AdminProductsPage");
            }
            return View(product);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProductConfirmation(int id)
        {
            // Retrieve the product details from the repository or database asynchronously
            Product product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                // Handle case where product is not found
                return NotFound();
            }

            // Pass the product object to the view
            return View(product);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product != null)
            {
                // Pass the product to the view
                return View("DeleteProductConfirmation", product);
            }
            // Handle error or redirect as needed
            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the product from the repository
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                // Handle case where product is not found
                return NotFound();
            }

            // Delete the product
            _productRepository.DeleteProduct(product);

            // Redirect the user back to the AdminProductsPage
            return RedirectToAction("AdminProductsPage");
        }






        //ADMIN USERS PAGE + CRUD
        [Authorize(Roles = "admin")]
        public IActionResult AdminCustomersPage()
        {
            // Retrieve the list of products from your repository
            var customers = _productRepository.GetAllCustomers(); // Adjust this method name as per your repository implementation

            // Pass the list of products to the view
            return View(customers);
        }





        //HOME PAGE


        public IActionResult HomePage()
        {
            List<Product> products;

            if (User.Identity.IsAuthenticated)
            {
                // Generate 5 random product IDs from 1 to 36
                var random = new Random();
                var randomProductIds = Enumerable.Range(1, 36).OrderBy(x => random.Next()).Take(5).ToList();

                // Fetch products with these random IDs
                products = _productRepository.Products
                                              .Where(p => randomProductIds.Contains(p.ProductId))
                                              .ToList();
            }
            else
            {
                // Define specific product IDs to fetch
                var specificProductIds = new List<int> { 27, 33, 34, 37, 24 };

                // Fetch products with specific IDs
                products = _productRepository.Products
                                              .Where(p => specificProductIds.Contains(p.ProductId))
                                              .ToList();
            }

            // Create a view model instance
            var model = new ProductListViewModel
            {
                Products = products,
                PaginationInfo = new PaginationInfo
                {
                    CurrntPage = 1,
                    ItemsPerPage = products.Count,
                    TotalItems = products.Count
                },
                // Not using filters for home page
                CurrentProductCategory = null,
                CurrentProductColor = null
            };

            return View(model);
        }



        //PRODUCT DETAIL PAGE
        public async Task<IActionResult> ProductDetailPage(int id)
        {
            var reqi = _recRepository.GetItemRecommendationForProductId(id);

            if (reqi != null)
            {
                 var recommendedProducts = _recRepository.getProductsForItemRecommendation(reqi);

                 ViewBag.recommendedProducts = recommendedProducts;

            }

            else
            {
                var recommendedProducts = _recRepository.getProductsIfItemRecIsNull();

                ViewBag.recommendedProducts = recommendedProducts;

            }

           




            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound(); // Or handle the case where the product is not found
            }

            return View(product);
        }



        /*
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _recRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var recommendations = await _recRepository.GetRecommendationsByProductIdAsync(product.ProductId);

            var viewModel = new ProductDetailsViewModel
            {
                Product = product
            };

            return View(viewModel);
        }
        */






        //PRODUCT LIST PAGE
        public IActionResult ProductListPage(int? pageNum, string? productCategory, string? productColor, int? pageSize)
        {
            // Set default page size if not specified
            pageSize ??= 5;

            // Ensure pageNum is not null and non-negative, otherwise default to 1
            pageNum ??= 1;
            pageNum = Math.Max(pageNum.Value, 1);

            var query = _productRepository.Products.AsQueryable();

            // Apply category filter if specified
            if (!string.IsNullOrEmpty(productCategory))
            {
                query = query.Where(x => x.Category == productCategory);
            }

            // Apply color filter if specified
            if (!string.IsNullOrEmpty(productColor))
            {
                query = query.Where(x => x.PrimaryColor == productColor);
            }

            var totalItems = query.Count();

            var blah = new ProductListViewModel
            {
                Products = query.OrderBy(x => x.Name)
                               .Skip((int)((pageNum.Value - 1) * pageSize))
                               .Take((int)pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrntPage = pageNum.Value,
                    ItemsPerPage = pageSize.Value,
                    TotalItems = totalItems
                },

                CurrentProductCategory = productCategory,
                CurrentProductColor = productColor
            };

            return View(blah);
        }



        //MISC STATIC PAGES
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUsPage()
        {
            return View();
        }

        [Authorize(Roles = "admin")]


        public IActionResult AdminHomePage()
        {
            return View();
        }
    }
}
