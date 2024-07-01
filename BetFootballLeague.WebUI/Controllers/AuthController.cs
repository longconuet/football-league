using BetFootballLeague.Application;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AuthenticationService _authenticationService;

        public AuthController(JwtSettings jwtSettings, AuthenticationService authenticationService)
        {
            _jwtSettings = jwtSettings;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var token = await _authenticationService.Authenticate(request.Username, request.Password);
                if (token != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Chỉ sử dụng với HTTPS
                        Expires = DateTime.UtcNow.AddHours(1) // Thời gian hết hạn của token
                    };

                    Response.Cookies.Append("jwtToken", token, cookieOptions);

                    return Json(new ResponseModel<string>
                    {
                        Status = ResponseStatusEnum.SUCCEED,
                        Data = token
                    });
                }

                return Json(new ResponseModel<string>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "Login failed"
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<string>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }            
        }
    }
}
