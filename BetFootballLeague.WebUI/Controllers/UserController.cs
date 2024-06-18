using Azure;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            return View();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserListAjax()
        {
            try
            {
                List<UserDto> users = await _userService.GetUsers();

                return Json(new ResponseModel<List<UserDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<UserDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAjax([FromBody] CreateUserRequestDto request)
        {
            try
            {
                var existUserByEmail = await _userService.GetUserByPhoneOrEmail(request.Email);
                if (existUserByEmail != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Email is already exist",
                    });
                }

                var existUserByPhone = await _userService.GetUserByPhoneOrEmail(request.Phone);
                if (existUserByPhone != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Phone is already exist",
                    });
                }

                await _userService.AddUser(request);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new user successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserInfoAjax(Guid id)
        {
            try
            {
                UserDto? user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return Json(new ResponseModel<UserDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found"
                    });
                }

                return Json(new ResponseModel<UserDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<UserDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }
    }
}
