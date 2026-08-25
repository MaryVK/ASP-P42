using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class UserController : Controller
    {
        // Аутенфикация - проверка логина и пароля
        public IActionResult BasicAuth()
        {
            // обратные действия к стандарту RFC 7617 'Basic' HTTP Authentication 
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if(authHeader == String.Empty)
            {
                return Unauthorized("Missing Authorization header");
            }
            String scheme = "Basic ";
            if(!authHeader.StartsWith(scheme))
            {
                return Unauthorized("Authorization scheme must be 'Basic'");
            }
            return Json(authHeader);
        }
    }
}
