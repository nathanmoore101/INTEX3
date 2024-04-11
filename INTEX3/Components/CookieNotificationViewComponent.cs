using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace INTEX3.Components
{
    public class CookieNotificationViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieNotificationViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            // Check if the cookie indicating notification acceptance exists
            if (!httpContext.Request.Cookies.ContainsKey("cookieNotificationAccepted"))
            {
                // If not, set a cookie with a longer expiration time
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(1),
                    HttpOnly = true
                };
                httpContext.Response.Cookies.Append("cookieNotificationAccepted", "true", cookieOptions);
                return View();
            }
            return Content(""); // Return an empty content if the notification has been accepted
        }
    }
}