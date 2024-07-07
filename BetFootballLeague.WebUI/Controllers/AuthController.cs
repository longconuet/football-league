using BetFootballLeague.Application;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.Shared.Helpers;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AuthService _authenticationService;
        private readonly UserService _userService;

        public AuthController(JwtSettings jwtSettings, AuthService authenticationService, UserService userService)
        {
            _jwtSettings = jwtSettings;
            _authenticationService = authenticationService;
            _userService = userService;
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
                var user = await _userService.GetUserByUsername(request.Username);
                if (user == null)
                {
                    return Json(new ResponseModel<string>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Account does not exist",
                    });
                }

                if (user.Status == UserStatusEnum.Inactive)
                {
                    return Json(new ResponseModel<string>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Account has been locked",
                    });
                }

                if (!await _userService.VerifyPassword(request.Username, request.Password))
                {

                    return Json(new ResponseModel<string>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Incorrect password",
                    });
                }

                await _authenticationService.Authenticate(user);

                return Json(new ResponseModel<string>
                {
                    Status = ResponseStatusEnum.SUCCEED
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

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.SignOut();

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
