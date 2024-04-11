using INTEX3.Models;
using Microsoft.AspNetCore.Mvc;

namespace INTEX3.Controllers
{

    public class OrderController : Controller
    {

        public ViewResult Checkout() => View(new Order());
    }
}