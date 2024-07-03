using BetFootballLeague.Application;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BetFootballLeague.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace BetFootballLeague.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AuthService _authenticationService;

        public AuthController(JwtSettings jwtSettings, AuthService authenticationService)
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
                await _authenticationService.Authenticate(request.Username, request.Password);

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
