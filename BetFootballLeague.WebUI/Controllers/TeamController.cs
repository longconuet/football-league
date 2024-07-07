using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using BetFootballLeague.WebUI.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace BetFootballLeague.WebUI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
    public class TeamController : Controller
    {
        private readonly TeamService _teamService;
        private readonly GroupService _groupService;

        public TeamController(TeamService teamService, GroupService groupService)
        {
            _teamService = teamService;
            _groupService = groupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamListAjax()
        {
            try
            {
                List<TeamDto> teams = await _teamService.GetTeams(true);

                return Json(new ResponseModel<List<TeamDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = teams
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<TeamDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSimpleTeamListAjax()
        {
            try
            {
                List<TeamDto> teams = await _teamService.GetTeams();

                return Json(new ResponseModel<List<TeamDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = teams
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<TeamDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
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
        public async Task<IActionResult> CreateTeamAjax([FromForm] CreateTeamRequestDto request, IFormFile image)
        {
            try
            {
                if (image == null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Please choose the image",
                    });
                }

                var imagePath = await Common.UploadFile(image, "teams");
                if (imagePath == null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Upload image failed",
                    });
                }
                request.Image = imagePath;

                var group = await _groupService.GetGroupById(request.GroupId);
                if (group == null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Group not found",
                    });
                }

                await _teamService.AddTeam(request);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new team successfully"
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
        public async Task<IActionResult> GetTeamInfoAjax(Guid id)
        {
            try
            {
                TeamDto? team = await _teamService.GetTeamById(id);
                if (team == null)
                {
                    return Json(new ResponseModel<TeamDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Team not found"
                    });
                }

                return Json(new ResponseModel<TeamDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = team
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<TeamDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeamAjax([FromForm] UpdateTeamRequestDto request, IFormFile image)
        {
            try
            {
                TeamDto? team = await _teamService.GetTeamById(request.Id);
                if (team == null)
                {
                    return Json(new ResponseModel<TeamDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Team not found"
                    });
                }

                if (image != null)
                {
                    var imagePath = await Common.UploadFile(image, "teams");
                    if (imagePath == null)
                    {
                        return Json(new ResponseModel
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Upload image failed",
                        });
                    }
                    team.Image = imagePath;
                }

                if (request.GroupId != team.GroupId)
                {
                    var group = await _groupService.GetGroupById(request.GroupId);
                    if (group == null)
                    {
                        return Json(new ResponseModel
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Group not found",
                        });
                    }
                }

                team.Name = request.Name;
                team.GroupId = request.GroupId;
                await _teamService.UpdateTeam(team);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update team successfully"
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
        public async Task<IActionResult> DeleteTeamAjax(Guid id)
        {
            try
            {
                TeamDto? team = await _teamService.GetTeamById(id);
                if (team == null)
                {
                    return Json(new ResponseModel<TeamDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Team not found"
                    });
                }

                await _teamService.DeleteTeam(team);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete team successfully"
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
