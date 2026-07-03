using CarGalleryHub.Application.DTOs.Auth;
using CarGalleryHub.Application.DTOs.User.Profile;
using CarGalleryHub.Application.DTOs.User.Security;
using CarGalleryHub.MVC.Models.DTOs.User;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection.Metadata.Ecma335;
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

        #region Funcs
        public bool HasExistingToken()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));
        }

        public IActionResult ToLogout() => RedirectToAction("Logout");
        public IActionResult ToLogin() => RedirectToAction("Login");
        public IActionResult ToMain() => RedirectToAction("Index",controllerName:"Home");
        public IActionResult ToProfile() => RedirectToAction("Profile");

        #endregion

        #region Login
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
        #endregion

        #region Register
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
        #endregion

        #region Logout
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
        }
        #endregion

        #region Profile
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Profile() 
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.GetAsync<ProfileViewData>("api/User/ViewProfile");

            if (response is null) 
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success && response.Data is null) 
            {
                TempData["ErrorMessage"] = "Başarısız";
                return ToLogout();
            }

            return View(response.Data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangeProfile(ProfileViewData profileViewData) 
        {
            if (!HasExistingToken())
                return ToLogin();

            var Body = new UserProfileUpdateRequest() 
            {
                FirstName = profileViewData.FirstName,
                ImageUrl = profileViewData.ImageUrl,
                LastName = profileViewData.LastName, 
            };

            var response = await _apiclient.PostAsync<bool>("api/User/ChangeProfile", Body);

            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToProfile();
            }

            if (!response.Success && response.Data is false)
            {
                TempData["ErrorMessage"] = "Başarısız";
                return ToProfile();
            }
            return ToProfile();
        }
        #endregion

        #region Security

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Security() 
        {
            if (!HasExistingToken()) 
            { return ToLogin(); }

            var response = await _apiclient.GetAsync<UserSecurityPageView>("api/User/ViewSecurity");

            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success && response.Data is null)
            {
                TempData["ErrorMessage"] = "Başarısız";
                return ToLogout();
            }

            return View(response.Data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateSecurity(UserSecurityPageView userSecurityPageView)
        {
            if (!HasExistingToken())
            { return ToLogin(); }

            var response = await _apiclient.PostAsync<bool[]>("api/User/UpdateSecurity",userSecurityPageView);

            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success && response.Data is null)
            {
                TempData["ErrorMessage"] = "Başarısız";
                return ToLogout();
            }

            switch (response.Data)
            {
                case [true, true]:
                    TempData["SuccessMessage"] = "Şifreniz ve e-posta adresiniz başarıyla güncellendi.";
                    break;

                case [false, true]:
                    TempData["SuccessMessage"] = "Şifreniz güncellendi ancak e-posta güncellenmedi.";
                    break;

                case [true, false]:
                    TempData["SuccessMessage"] = "E-posta adresiniz güncellendi ancak şifreniz güncellenmedi.";
                    break;

                case [false, false]:
                    TempData["ErrorMessage"] = "Güncelleme işlemleri başarısız oldu. Mevcut şifrenizi kontrol edin.";
                    return View(userSecurityPageView);

                case null:
                    TempData["ErrorMessage"] = "Sistemden geçersiz bir yanıt alındı.";
                    return ToMain();
            }

            return ToMain();
        }

        #endregion
    }
}
