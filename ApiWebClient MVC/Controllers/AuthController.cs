using ApiWebClient.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult LoginWithGoogle()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/Auth/GoogleResponse" }, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest(); // Handle error response here

            // Extract user info from result.Principal
            // Implement your logic here (e.g., creating a user session)

            return Redirect("");
        }


        public string errorMessage = string.Empty;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid) return View(userRegisterDTO);
            
            var result = await _authService.Register(userRegisterDTO);
            if(result.Success)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            if(!ModelState.IsValid) return View(userLoginDTO);

            var result = await _authService.Login(userLoginDTO);
            if(result.Success)
            {
                Response.Cookies.Append("accessToken", result.Data, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict, 
                });
                return Redirect("/Home/Index");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("accessToken");
            return Redirect("/Home/Index");
        }
    }
}
