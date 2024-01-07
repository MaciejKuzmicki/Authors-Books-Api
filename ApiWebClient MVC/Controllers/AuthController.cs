using ApiWebClient.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

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
                return BadRequest(); 
            var principal = result.Principal;
            var claims = principal.Identities.FirstOrDefault()?.Claims;
            if (claims is null)
                return BadRequest(); 

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var password = generatePassword();
            var username = email?.Split('@')[0]; 

            var userRegisterDto = new UserRegisterDTO
            {
                Email = email,
                Password = password,
                ConfirmPassword = password,
                Username = username
            };
            var userLoginDto = new UserLoginDTO
            {
                Email = email,
                Password = password,
            };

            var log = await _authService.Login(userLoginDto);
            await Console.Out.WriteLineAsync("STOP1");
            if (!log.Success)
            {
                var reg = await _authService.Register(userRegisterDto);
                log = await _authService.Login(userLoginDto);
                await Console.Out.WriteLineAsync("STOP2");

            }
            if (log.Success)
            {
                await Console.Out.WriteLineAsync("STOP3");

                Response.Cookies.Append("accessToken", log.Data, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                });

            }


            return Redirect("/Home/Index");



        }


        public string generatePassword()
        {
            return "123456789";
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
