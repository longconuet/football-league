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

                match.IndexOrder = request.IndexOrder;
                match.RoundId = request.RoundId;
                match.Date = request.Date;
                match.Time = request.Time;
                match.Team1Id = request.Team1Id;
                match.Team2Id = request.Team2Id;
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

        [HttpPost]
        public async Task<IActionResult> SetOddsMatchAjax([FromBody] SetOddsMatchRequestDto request)
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

                if (match.BetStatus == MatchBetStatusEnum.OPENING || match.BetStatus == MatchBetStatusEnum.IS_CLOSED)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Can not set odds"
                    });
                }

                if (match.Team1Id == null || match.Team2Id == null)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Team match is not set"
                    });
                }

                if (request.UpperDoorTeamId != match.Team1Id && request.UpperDoorTeamId != match.Team2Id)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Invalid team"
                    });
                }

                match.Odds = request.Odds;
                match.UpperDoorTeamId = request.Odds != null ? request.UpperDoorTeamId : null;
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
        public async Task<IActionResult> UpdateMatchStatusAjax([FromBody] UpdateMatchBetStatusRequestDto request)
        {
            try
            {
                if (!Enum.IsDefined(typeof(MatchBetStatusEnum), request.BetStatus))
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Invalid status"
                    });
                }
                var newStatus = (MatchBetStatusEnum)request.BetStatus;

                MatchDto? match = await _matchService.GetMatchById(request.Id);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }
                var oldStatus = match.BetStatus;

                bool invalidCondition1 = (newStatus == MatchBetStatusEnum.OPENING || newStatus == MatchBetStatusEnum.IS_CLOSED)
                    && (match.Team1Id == null || match.Team2Id == null || match.Odds == null);
                bool invalidCondition2 = newStatus == MatchBetStatusEnum.NOT_ALLOWED && (oldStatus == MatchBetStatusEnum.OPENING || oldStatus == MatchBetStatusEnum.IS_CLOSED);
                if (invalidCondition1 || invalidCondition2)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Can not change status"
                    });
                }

                if (newStatus == MatchBetStatusEnum.IS_CLOSED && (match.Team1Score == null || match.Team2Score == null))
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "The score has not been updated yet"
                    });
                }

                if (newStatus == MatchBetStatusEnum.IS_CLOSED)
                {

                }

                match.BetStatus = newStatus;
                await _matchService.UpdateMatch(match);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update status successfully"
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
        public async Task<IActionResult> UpdateMatchScoreAjax([FromBody] UpdateMatchScoreRequestDto request)
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

                if (match.BetStatus != MatchBetStatusEnum.OPENING)
                {
                    return Json(new ResponseModel<MatchDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Can not update score"
                    });
                }

                match.Team1Score = request.Team1Score;
                match.Team2Score = request.Team2Score;
                await _matchService.UpdateMatch(match);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update score successfully"
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
