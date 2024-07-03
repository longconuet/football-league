using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
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
                var existUserByEmail = await _userService.GetUserByPhone(request.Email);
                if (existUserByEmail != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Email is already exist",
                    });
                }

                var existUserByPhone = await _userService.GetUserByEmail(request.Phone);
                if (existUserByPhone != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Phone is already exist",
                    });
                }

                var existUserByUsername = await _userService.GetUserByUsername(request.Username);
                if (existUserByUsername != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Username is already exist",
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

        [HttpPost]
        public async Task<IActionResult> UpdateUserAjax([FromBody] UpdateUserRequestDto request)
        {
            try
            {
                UserDto? user = await _userService.GetUserById(request.Id);
                if (user == null)
                {
                    return Json(new ResponseModel<UserDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found"
                    });
                }

                var existUserByEmail = await _userService.GetUserByPhone(request.Email, request.Id);
                if (existUserByEmail != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Email is already exist",
                    });
                }

                var existUserByPhone = await _userService.GetUserByEmail(request.Phone, request.Id);
                if (existUserByPhone != null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Phone is already exist",
                    });
                }

                user.FullName = request.FullName;
                user.Phone = request.Phone;
                user.Email = request.Email;
                await _userService.UpdateUser(user);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update user successfully"
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

        [HttpPost]
        public async Task<IActionResult> DeleteUserAjax(Guid id)
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

                await _userService.DeleteUser(user);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete user successfully"
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

        [HttpPost]
        public async Task<IActionResult> UpdateUserStatusAjax([FromBody] UpdateUserStatusRequestDto request)
        {
            try
            {
                UserDto? user = await _userService.GetUserById(request.Id);
                if (user == null)
                {
                    return Json(new ResponseModel<UserDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found"
                    });
                }

                user.Status = (UserStatusEnum)request.Status;
                await _userService.UpdateUser(user);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = user.Status == UserStatusEnum.Active ? "Active user successfully" : "De-active user successfully"
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
    }
}
