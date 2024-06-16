using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            List<UserDto> list = await _userService.GetUsers();

            return View(list);
        }
    }
}
