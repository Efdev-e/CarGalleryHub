using CarGalleryHub.Application.DTOs.Auth;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Security.Claims;
using static CarGalleryHub.MVC.Services.ApiClient;

namespace CarGalleryHub.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _apiclient;
        public AccountController(ApiClient apiclient)
        {
            _apiclient = apiclient;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            if (HasExistingToken())
                return Redirect("/");


            return View();
        }



        [HttpPost]
        [Route("Login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto dto) 
        {
            if (HasExistingToken())
                return Redirect("/");

            if (!ModelState.IsValid)
                return View(dto);

            var response = await _apiclient.PostAsync<AuthResponseDto>("api/Auth/login", dto);

            if (response is null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş. Tekrar Deneyin");
                return View(dto);
            }

            if (!response.Success || response.Data is null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş. Tekrar Deneyin");
                return View(dto);
            }

            HttpContext.Session.SetString("JwtToken", response.Data.Token);
            HttpContext.Session.SetString("UserEmail", response.Data.Email);
            HttpContext.Session.SetString("UserName", response.Data.FullName);
            HttpContext.Session.SetString("UserId", response.Data.UserId.ToString());
            HttpContext.Session.SetString("UserRole", response.Data.Role.ToString());


            return Redirect("/");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register() 
        {
            if (HasExistingToken())
                return Redirect("/");


            return View();
        }

        [HttpPost]
        [Route("Register")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDto registerDto) 
        {
            if (HasExistingToken())
                return Redirect("/");

            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError(string.Empty, "Parametreler Hatalı");
                return View();
            }

            var response = await _apiclient.PostAsync<AuthResponseDto>("api/Auth/register",registerDto);

            if (response is null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş. Tekrar Deneyin");
                return View(registerDto);
            }

            if (!response.Success || response.Data is null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş. Tekrar Deneyin");
                return View(registerDto);
            }

            HttpContext.Session.SetString("JwtToken", response.Data.Token);
            HttpContext.Session.SetString("UserEmail", response.Data.Email);
            HttpContext.Session.SetString("UserName", response.Data.FullName);
            HttpContext.Session.SetString("UserId", response.Data.UserId.ToString());
            HttpContext.Session.SetString("UserRole", response.Data.Role.ToString());



            return Redirect("/");
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
        }

        public bool HasExistingToken() 
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));
        }
    }
}
