using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupListAjax()
        {
            try
            {
                List<GroupDto> groups = await _groupService.GetGroups();

                return Json(new ResponseModel<List<GroupDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = groups
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<GroupDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupAjax([FromBody] CreateGroupRequestDto request)
        {
            try
            {
                await _groupService.AddGroup(request);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new group successfully"
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
        public async Task<IActionResult> GetGroupInfoAjax(Guid id)
        {
            try
            {
                GroupDto? group = await _groupService.GetGroupById(id);
                if (group == null)
                {
                    return Json(new ResponseModel<GroupDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Group not found"
                    });
                }

                return Json(new ResponseModel<GroupDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = group
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<GroupDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGroupAjax([FromBody] UpdateGroupRequestDto request)
        {
            try
            {
                GroupDto? group = await _groupService.GetGroupById(request.Id);
                if (group == null)
                {
                    return Json(new ResponseModel<GroupDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Group not found"
                    });
                }

                group.Name = request.Name;
                await _groupService.UpdateGroup(group);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update group successfully"
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
        public async Task<IActionResult> DeleteGroupAjax(Guid id)
        {
            try
            {
                GroupDto? group = await _groupService.GetGroupById(id);
                if (group == null)
                {
                    return Json(new ResponseModel<GroupDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Group not found"
                    });
                }

                await _groupService.DeleteGroup(group);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete group successfully"
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
