using INTEX3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INTEX3.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Roles = "customer,admin")]
        public ViewResult Checkout() => View(new Order());
    }
}
