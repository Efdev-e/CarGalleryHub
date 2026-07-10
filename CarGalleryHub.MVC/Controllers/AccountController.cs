using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Auth;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.Application.DTOs.Payment;
using CarGalleryHub.Application.DTOs.User.Profile;
using CarGalleryHub.Application.DTOs.User.Security;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.MVC.Models;
using CarGalleryHub.MVC.Models.DTOs.Advert;
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

        #region Address
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Address()
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.GetAsync<List<AddressDto>>("api/Address/List");

            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message ?? "Adresler yüklenemedi.";
                return ToMain();
            }

            return View(response.Data ?? new List<AddressDto>());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateAddress(CreateAddressDto addressDto)
        {
            if (!HasExistingToken())
                return ToLogin();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Adres bilgileri eksik veya hatalı.";
                return RedirectToAction(nameof(Address));
            }

            var response = await _apiclient.PostAsync<bool>("api/Address/create", addressDto);

            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = "Adres eklenemedi.";
                return RedirectToAction(nameof(Address));
            }

            TempData["SuccessMessage"] = "Adres başarıyla eklendi.";
            return RedirectToAction(nameof(Address));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.DeleteAsync<bool>($"api/Address/delete/{id}");

            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = "Adres silinemedi.";
                return RedirectToAction(nameof(Address));
            }

            TempData["SuccessMessage"] = "Adres silindi.";
            return RedirectToAction(nameof(Address));
        }
        #endregion

        #region Orders
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Orders()
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.PostNoBodyAsync<List<OrderDto>>("api/Order/GetUserOrders");

            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message ?? "Siparişler yüklenemedi.";
                return ToMain();
            }

            return View(response.Data ?? new List<OrderDto>());
        }
        #endregion

        #region Cart
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Cart()
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.GetAsync<CartDto>("api/Cart/GetCart");

            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message ?? "Sepet yüklenemedi.";
                return ToMain();
            }

            return View(response.Data ?? new CartDto { UserId = 0 });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddCartItem(int advertId, int quantity = 1)
        {
            if (!HasExistingToken())
                return ToLogin();

            if (advertId <= 0 || quantity <= 0)
            {
                TempData["ErrorMessage"] = "Geçersiz ilan veya miktar.";
                return RedirectToAction(nameof(Cart));
            }

            var response = await _apiclient.PostAsync<bool>("api/Cart/addItem", new CreateCartItemDto
            {
                AdvertId = advertId,
                Quantity = quantity
            });

            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = response?.Message ?? "Ürün adedi artırılamadı.";
                return RedirectToAction(nameof(Cart));
            }

            TempData["SuccessMessage"] = "Ürün adedi artırıldı.";
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.DeleteAsync<bool>($"api/Cart/removeItem/{cartItemId}");

            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = "Ürün sepetten çıkarılamadı.";
                return RedirectToAction(nameof(Cart));
            }

            TempData["SuccessMessage"] = "Ürün sepetten çıkarıldı.";
            return RedirectToAction(nameof(Cart));
        }
        #endregion

        #region Checkout
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Checkout()
        {
            if (!HasExistingToken())
                return ToLogin();

            var cartResponse = await _apiclient.GetAsync<CartDto>("api/Cart/GetCart");
            if (cartResponse is null || !cartResponse.Success || cartResponse.Data == null || !cartResponse.Data.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Sepetiniz boş olduğu için ödeme adımına geçilemez.";
                return RedirectToAction("Cart");
            }

            var addressesResponse = await _apiclient.GetAsync<List<AddressDto>>("api/Address/List");
            var savedAddresses = addressesResponse?.Data ?? new List<AddressDto>();

            var viewModel = new CheckoutViewModel
            {
                Cart = cartResponse.Data,
                SavedAddresses = savedAddresses
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!HasExistingToken())
                return ToLogin();

            var cartResponse = await _apiclient.GetAsync<CartDto>("api/Cart/GetCart");
            if (cartResponse is null || !cartResponse.Success || cartResponse.Data == null || !cartResponse.Data.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Sepetiniz boş.";
                return RedirectToAction("Cart");
            }

            var addressesResponse = await _apiclient.GetAsync<List<AddressDto>>("api/Address/List");
            var savedAddresses = addressesResponse?.Data ?? new List<AddressDto>();

            model.Cart = cartResponse.Data;
            model.SavedAddresses = savedAddresses;

            if (string.IsNullOrWhiteSpace(model.AddressFullName) ||
                string.IsNullOrWhiteSpace(model.AddressCity) ||
                string.IsNullOrWhiteSpace(model.AddressDistrict) ||
                string.IsNullOrWhiteSpace(model.AddressPostalCode) ||
                string.IsNullOrWhiteSpace(model.FullAddress) ||
                string.IsNullOrWhiteSpace(model.CardNumber) ||
                string.IsNullOrWhiteSpace(model.CardHolderName) ||
                string.IsNullOrWhiteSpace(model.ExpiryMonth) ||
                string.IsNullOrWhiteSpace(model.ExpiryYear) ||
                string.IsNullOrWhiteSpace(model.Cvv))
            {
                TempData["ErrorMessage"] = "Lütfen tüm adres ve ödeme bilgilerini eksiksiz doldurun.";
                return View(model);
            }

            // 1. Create the Order
            var createOrderDto = new CreateOrderDto
            {
                AddressFullName = model.AddressFullName,
                AddressCity = model.AddressCity,
                AddressDistrict = model.AddressDistrict,
                AddressPostalCode = model.AddressPostalCode,
                FullAddress = model.FullAddress
            };

            var orderResponse = await _apiclient.PostAsync<int>("api/Order/CreateOrder", createOrderDto);
            if (orderResponse is null || !orderResponse.Success || orderResponse.Data <= 0)
            {
                TempData["ErrorMessage"] = orderResponse?.Message ?? "Sipariş oluşturulamadı.";
                return View(model);
            }

            int orderId = orderResponse.Data;

            // 2. Process Payment
            var paymentRequestDto = new PaymentRequestDto
            {
                OrderId = orderId,
                CardNumber = model.CardNumber.Replace(" ", ""),
                CardHolderNumner = model.CardHolderName,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                Cvv = model.Cvv
            };

            var paymentResponse = await _apiclient.PostAsync<PaymentResultDto>("api/Payment/pay", paymentRequestDto);
            if (paymentResponse is null || !paymentResponse.Success || paymentResponse.Data?.IsSuccess != true)
            {
                TempData["ErrorMessage"] = paymentResponse?.Message ?? paymentResponse?.Data?.FailureReason ?? "Sipariş oluşturuldu fakat ödeme başarısız oldu. Siparişlerim sayfasından tekrar ödeme yapmayı deneyebilirsiniz.";
                return RedirectToAction(nameof(Orders));
            }

            TempData["SuccessMessage"] = "Siparişiniz başarıyla alındı ve ödemesi gerçekleştirildi!";
            return RedirectToAction(nameof(Orders));
        }
        #endregion

        #region PayOrder
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PayOrder(int orderId)
        {
            if (!HasExistingToken())
                return ToLogin();

            var orderResponse = await _apiclient.GetAsync<OrderInfoDto>($"api/Order/GetOrderById/{orderId}");
            if (orderResponse is null || !orderResponse.Success || orderResponse.Data == null)
            {
                TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                return RedirectToAction(nameof(Orders));
            }

            var order = orderResponse.Data;
            if (order.OrderStatus == Domain.Enum.OrderStatus.Paid || order.OrderStatus == Domain.Enum.OrderStatus.Completed)
            {
                TempData["ErrorMessage"] = "Bu sipariş için zaten ödeme yapılmıştır.";
                return RedirectToAction(nameof(Orders));
            }

            var model = new PayOrderViewModel
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                TotalCost = order.TotalCost
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PayOrder(PayOrderViewModel model)
        {
            if (!HasExistingToken())
                return ToLogin();

            if (string.IsNullOrWhiteSpace(model.CardNumber) ||
                string.IsNullOrWhiteSpace(model.CardHolderName) ||
                string.IsNullOrWhiteSpace(model.ExpiryMonth) ||
                string.IsNullOrWhiteSpace(model.ExpiryYear) ||
                string.IsNullOrWhiteSpace(model.Cvv))
            {
                TempData["ErrorMessage"] = "Lütfen tüm kart bilgilerini eksiksiz doldurun.";
                return View(model);
            }

            var paymentRequestDto = new PaymentRequestDto
            {
                OrderId = model.OrderId,
                CardNumber = model.CardNumber.Replace(" ", ""),
                CardHolderNumner = model.CardHolderName,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                Cvv = model.Cvv
            };

            var paymentResponse = await _apiclient.PostAsync<PaymentResultDto>("api/Payment/pay", paymentRequestDto);
            if (paymentResponse is null || !paymentResponse.Success || paymentResponse.Data?.IsSuccess != true)
            {
                TempData["ErrorMessage"] = paymentResponse?.Message ?? paymentResponse?.Data?.FailureReason ?? "Ödeme işlemi başarısız oldu. Lütfen tekrar deneyin.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Ödeme başarıyla gerçekleştirildi!";
            return RedirectToAction(nameof(Orders));
        }
        #endregion

        #region MyAdverts
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> MyAdverts(int? page, string? name)
        {
            if (!HasExistingToken())
                return ToLogin();

            if (!page.HasValue)
                page = 1;

            var query = $"api/Advert/GetMyAdverts/{page}";
            if (!string.IsNullOrWhiteSpace(name))
                query += $"?Name={Uri.EscapeDataString(name)}";

            var response = await _apiclient.GetAsync<List<AdvertView>>(query);
            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt boş";
                return ToMain();
            }

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message ?? "İlanlar yüklenemedi.";
                return ToMain();
            }

            return View(new AdvertPageViewDto { page = page ?? 1, Dtos = response.Data ?? new List<AdvertView>() });
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdvert()
        {
            if (!HasExistingToken())
                return ToLogin();

            var carResponse = await _apiclient.GetAsync<List<CarInfoDto>>("api/Car/GetAllCarms");
            var model = new AdvertCreateViewDto
            {
                createAdvertDto = new CreateAdvertDto
                {
                    AdvertTitle = string.Empty,
                    ImageUrl = string.Empty,
                    Description = string.Empty,
                    Warranty = false,
                    CarId = 0,
                    UnitPrice = 0
                },
                Cars = carResponse?.Data ?? new List<CarInfoDto>()
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateAdvert([Bind(Prefix = "createAdvertDto")] CreateAdvertDto dto)
        {
            if (!HasExistingToken())
                return ToLogin();

            if (dto is null)
            {
                TempData["ErrorMessage"] = "İlan bilgileri eksik.";
                return RedirectToAction(nameof(MyAdverts));
            }

            ModelState.Remove("SellerId");
            dto.SellerId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            if (!ModelState.IsValid)
            {
                var carResponse = await _apiclient.GetAsync<List<CarInfoDto>>("api/Car/GetAllCarms");
                var model = new AdvertCreateViewDto
                {
                    createAdvertDto = dto,
                    Cars = carResponse?.Data ?? new List<CarInfoDto>()
                };
                return View(model);
            }

            var advertDto = new AdvertDto
            {
                AdvertTitle = dto.AdvertTitle,
                Description = dto.Description,
                CarId = dto.CarId,
                SellerId = dto.SellerId,
                UnitPrice = dto.UnitPrice,
                Warranty = dto.Warranty,
                Thumbnails = new List<ImageDto>
                {
                    new ImageDto { ImageUrl = dto.ImageUrl, ImageType = ImageType.Thumbnail }
                }
            };

            var response = await _apiclient.PostAsync<bool>("api/Advert/create", advertDto);
            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = response?.Message ?? "İlan oluşturulamadı.";
                return RedirectToAction(nameof(MyAdverts));
            }

            TempData["SuccessMessage"] = "İlan başarıyla oluşturuldu.";
            return RedirectToAction(nameof(MyAdverts));
        }

        [HttpGet]
        public async Task<IActionResult> EditAdvert(int id)
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.GetAsync<AdvertDto>($"api/Advert/{id}");
            if (response is null || !response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = "İlan bulunamadı.";
                return RedirectToAction(nameof(MyAdverts));
            }

            var model = new AdvertUpdateModel
            {
                Id = response.Data.Id,
                AdvertTitle = response.Data.AdvertTitle,
                Description = response.Data.Description,
                UnitPrice = response.Data.UnitPrice,
                ImageUrl = response.Data.Thumbnails.FirstOrDefault()?.ImageUrl,
                CarId = response.Data.CarId
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditAdvert(int id, AdvertUpdateModel model)
        {
            if (!HasExistingToken())
                return ToLogin();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateDto = new UpdateAdvertDto
            {
                AdvertTitle = model.AdvertTitle,
                Description = model.Description,
                UnitPrice = model.UnitPrice,
                Thumbnails = new List<ImageDto> 
                {
                    new ImageDto { ImageUrl = model.ImageUrl ?? string.Empty, ImageType = ImageType.Thumbnail }
                },
                CarId = model.CarId
            };

            var response = await _apiclient.PostAsync<bool>($"api/Advert/update/{id}", updateDto);
            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = "İlan güncellenemedi.";
                return View(model);
            }

            TempData["SuccessMessage"] = "İlan başarıyla güncellendi.";
            return RedirectToAction(nameof(MyAdverts));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAdvert(int id)
        {
            if (!HasExistingToken())
                return ToLogin();

            var response = await _apiclient.DeleteAsync<bool>($"api/Advert/delete/{id}");
            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = "İlan silinemedi.";
                return RedirectToAction(nameof(MyAdverts));
            }

            TempData["SuccessMessage"] = "İlan başarıyla silindi.";
            return RedirectToAction(nameof(MyAdverts));
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
