using AutoMapper;
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
    public class UserBetController : Controller
    {
        private readonly UserBetService _userBetService;
        private readonly MatchService _matchService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserBetController(UserBetService userBetService, MatchService matchService, UserService userService, IMapper mapper)
        {
            _userBetService = userBetService;
            _matchService = matchService;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBetListAjax([FromQuery] BetListByUserFilterRequestDto request)
        {
            try
            {
                var currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                List<UserBetDto> userBets = await _userBetService.GetBetsByUser(currentUserId, request);
                List<MatchDto> matches = await _matchService.GetMatches(request.Status != null ? (MatchBetStatusEnum)request.Status : null);

                List<MatchBetDto> data = _mapper.Map<List<MatchBetDto>>(matches);
                foreach (var matchBet in data)
                {
                    var userBet = userBets.FirstOrDefault(x => x.MatchId == matchBet.Id);
                    matchBet.UserBet = userBet;
                }

                return Json(new ResponseModel<List<MatchBetDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<MatchBetDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMatchInfoForUserBetAjax(string id)
        {
            try
            {
                var matchId = Guid.Parse(id);
                var match = await _matchService.GetMatchById(matchId);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }

                if (match.BetStatus != MatchBetStatusEnum.OPENING)
                {
                    return Json(new ResponseModel<MatchBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Can not bet this match"
                    });
                }

                var currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var userBet = await _userBetService.GetUserBetByMatchId(currentUserId, matchId);

                MatchBetDto data = _mapper.Map<MatchBetDto>(match);
                data.UserBet = userBet;

                return Json(new ResponseModel<MatchBetDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<MatchBetDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BetMatchAjax([FromBody] BetMatchRequestDto request)
        {
            try
            {
                Guid currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());

                var match = await _matchService.GetMatchById(request.MatchId);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }

                if (match.BetStatus != MatchBetStatusEnum.OPENING || match.IsLockedBet)
                {
                    return Json(new ResponseModel<MatchBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Can not bet this match"
                    });
                }

                if (request.BetTeamId != match.Team1Id && request.BetTeamId != match.Team2Id)
                {
                    return Json(new ResponseModel<MatchBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Invalid team"
                    });
                }

                var userBetDto = await _userBetService.GetUserBetByMatchId(currentUserId, match.Id);
                if (userBetDto == null)
                {
                    // create
                    var createUserBetRequestDto = _mapper.Map<CreateUserBetRequestDto>(request);
                    createUserBetRequestDto.UserId = currentUserId;
                    await _userBetService.AddUserBet(createUserBetRequestDto);
                }
                else
                {
                    // update
                    userBetDto.BetTeamId = request.BetTeamId;
                    await _userBetService.UpdateUserBet(userBetDto);
                }                

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Bet successfully"
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
        public async Task<IActionResult> GetUserBetInfoAjax(Guid id)
        {
            try
            {
                UserBetDto? userBet = await _userBetService.GetUserBetById(id);
                if (userBet == null)
                {
                    return Json(new ResponseModel<UserBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Bet not found"
                    });
                }

                Guid currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                if (userBet.UserId != currentUserId)
                {
                    return Json(new ResponseModel<UserBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Invalid bet"
                    });
                }

                var match = await _matchService.GetMatchById(userBet.MatchId);
                if (match == null)
                {
                    return Json(new ResponseModel<MatchBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }
                userBet.Match = match;

                return Json(new ResponseModel<UserBetDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = userBet
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<UserBetDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBetStatisticAjax(string id)
        {
            try
            {
                var matchId = Guid.Parse(id);
                var match = await _matchService.GetMatchById(matchId);
                if (match == null)
                {
                    return Json(new ResponseModel<BetMatchStatisticDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match not found"
                    });
                }

                if (match.BetStatus == MatchBetStatusEnum.NOT_ALLOWED)
                {
                    return Json(new ResponseModel<BetMatchStatisticDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Match is not allowed to bet"
                    });
                }

                var userBets = await _userBetService.GetBetsByMatch(matchId);
                var users = await _userService.GetAllNormalUsers();

                BetMatchStatisticDto data = new()
                {
                    Match = new MatchStatisticDto
                    {
                        Id = matchId,
                        RoundName = match.Round.Name,
                        DateTime = match.DateTime,
                        BetStatus = match.BetStatus,
                        Odds = match.Odds!.Value,
                        WinBetTeamId = match.WinBetTeamId
                    }
                };

                var usersBetTeam1 = userBets.Where(x => x.BetTeamId == match.Team1Id).ToList();
                var usersBetTeam2 = userBets.Where(x => x.BetTeamId == match.Team2Id).ToList();
                Guid currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());

                data.Team1 = new TeamStatisticDto
                {
                    Id = match.Team1.Id,
                    Name = match.Team1.Name,
                    Image = match.Team1.Image,
                    IsUpperDoor = match.UpperDoorTeamId == match.Team1Id ? true : false,
                    Score = match.Team1Score,
                    BetUsers = usersBetTeam1.Select(x => new UserStatisticDto
                    {
                        Id = x.UserId,
                        FullName = x.User.FullName + (currentUserId == x.UserId ? " (You)" : ""),
                        BetTime = x.CreatedAtStr
                    }).ToList()
                };

                data.Team2 = new TeamStatisticDto
                {
                    Id = match.Team2.Id,
                    Name = match.Team2.Name,
                    Image = match.Team2.Image,
                    IsUpperDoor = match.UpperDoorTeamId == match.Team2Id ? true : false,
                    Score = match.Team2Score,
                    BetUsers = usersBetTeam2.Select(x => new UserStatisticDto
                    {
                        Id = x.UserId,
                        FullName = x.User.FullName + (currentUserId == x.UserId ? " (You)" : ""),
                        BetTime = x.CreatedAtStr
                    }).ToList()
                };

                var betUserIds = userBets.Select(x => x.UserId).ToList();
                var notBetUsers = users.Where(x => !betUserIds.Contains(x.Id))
                    .Select(x => new UserStatisticDto
                    {
                        Id = x.Id,
                        FullName = x.FullName + (currentUserId == x.Id ? " (You)" : "")
                    })
                    .ToList();
                data.NotBetUsers = notBetUsers;

                data.ChartInfo = new ChartBetStatisticDto
                {
                    Team1Name = match.Team1.Name,
                    Team2Name = match.Team2.Name,
                    BetTeam1Number = data.Team1.BetUsers.Count(),
                    BetTeam2Number = data.Team2.BetUsers.Count(),
                    NotBetNumber = notBetUsers.Count()
                };

                return Json(new ResponseModel<BetMatchStatisticDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<BetMatchStatisticDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }
    }
}
