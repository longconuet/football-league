using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "User")]
    public class UserProfileController : Controller
    {
        private readonly UserService _userService;

        public UserProfileController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserInfoAjax()
        {
            try
            {
                Guid currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                UserDto? user = await _userService.GetUserById(currentUserId);
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
        public async Task<IActionResult> UpdateUserAjax([FromBody] UpdateUserProfileRequestDto request)
        {
            try
            {
                Guid currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                UserDto? user = await _userService.GetUserById(currentUserId);
                if (user == null)
                {
                    return Json(new ResponseModel<UserDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found"
                    });
                }

                if (request.Email != user.Email)
                {
                    var existUserByEmail = await _userService.GetUserByPhone(request.Email, currentUserId);
                    if (existUserByEmail != null)
                    {
                        return Json(new ResponseModel
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Email is already exist",
                        });
                    }
                }

                if (request.Phone != user.Phone)
                {
                    var existUserByPhone = await _userService.GetUserByEmail(request.Phone, currentUserId);
                    if (existUserByPhone != null)
                    {
                        return Json(new ResponseModel
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Phone is already exist",
                        });
                    }
                }

                user.FullName = request.FullName;
                user.Phone = request.Phone;
                user.Email = request.Email;
                user.UpdatedBy = currentUserId;
                user.UpdatedAt = DateTime.Now;
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
    }
}
