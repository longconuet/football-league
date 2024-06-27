using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BetFootballLeague.WebUI.Controllers
{
    public class MatchController : Controller
    {
        private readonly MatchService _matchService;
        private readonly RoundService _roundService;
        private readonly TeamService _teamService;

        public MatchController(MatchService matchService, RoundService roundService, TeamService teamService)
        {
            _matchService = matchService;
            _roundService = roundService;
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMatchListAjax()
        {
            try
            {
                List<MatchDto> matchs = await _matchService.GetMatches();

                return Json(new ResponseModel<List<MatchDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = matchs
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<MatchDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatchAjax([FromBody] CreateMatchRequestDto request)
        {
            try
            {
                //var xx = DateTime.ParseExact($"{request.Date} {request.Time}", "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                var round = await _roundService.GetRoundById(request.RoundId);
                if (round == null)
                {
                    return Json(new ResponseModel
                    {
                        Status = ResponseStatusEnum.SUCCEED,
                        Message = "Round not found"
                    });
                }

                if (request.Team1Id != null)
                {
                    var team1 = await _teamService.GetTeamById(request.Team1Id.Value);
                    if (team1 == null)
                    {
                        return Json(new ResponseModel
                        {
                            Status = ResponseStatusEnum.SUCCEED,
                            Message = "Team 1 not found"
                        });
                    }
                }

                if (request.Team2Id != null)
                {
                    var team2 = await _teamService.GetTeamById(request.Team2Id.Value);
                    if (team2 == null)
                    {
                        return Json(new ResponseModel
                        {
                            Status = ResponseStatusEnum.SUCCEED,
                            Message = "Team 2 not found"
                        });
                    }
                }

                await _matchService.AddMatch(request);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new match successfully"
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
        public async Task<IActionResult> GetMatchInfoAjax(Guid id)
        {
            try
            {
                MatchDto? match = await _matchService.GetMatchById(id);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }

                return Json(new ResponseModel<MatchDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = match
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<MatchDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMatchAjax([FromBody] UpdateMatchRequestDto request)
        {
            try
            {
                MatchDto? match = await _matchService.GetMatchById(request.Id);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }

                //match.Name = request.Name;
                await _matchService.UpdateMatch(match);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update match successfully"
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
        public async Task<IActionResult> DeleteMatchAjax(Guid id)
        {
            try
            {
                MatchDto? match = await _matchService.GetMatchById(id);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }

                await _matchService.DeleteMatch(id);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete match successfully"
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
