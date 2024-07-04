﻿using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    public class UserBetController : Controller
    {
        private readonly UserBetService _userBetService;
        private readonly MatchService _matchService;
        private readonly IMapper _mapper;

        public UserBetController(UserBetService userBetService, MatchService matchService, IMapper mapper)
        {
            _userBetService = userBetService;
            _matchService = matchService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBetListAjax()
        {
            try
            {
                var currentUserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                List<UserBetDto> userBets = await _userBetService.GetBetsByUser(currentUserId);
                List<MatchDto> matches = await _matchService.GetMatches();

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

        [HttpPost]
        public async Task<IActionResult> CreateUserBetAjax([FromBody] CreateUserBetRequestDto request)
        {
            try
            {
                await _userBetService.AddUserBet(request);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new userBet successfully"
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
        public async Task<IActionResult> GetMatchInfoForUserBetAjax(Guid matchId)
        {
            try
            {
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

        [HttpPost]
        public async Task<IActionResult> UpdateUserBetAjax([FromBody] UpdateUserBetRequestDto request)
        {
            try
            {
                UserBetDto? userBet = await _userBetService.GetUserBetById(request.Id);
                if (userBet == null)
                {
                    return Json(new ResponseModel<UserBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "UserBet not found"
                    });
                }

                //userBet.Name = request.Name;
                await _userBetService.UpdateUserBet(userBet);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update userBet successfully"
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
        public async Task<IActionResult> DeleteUserBetAjax(Guid id)
        {
            try
            {
                UserBetDto? userBet = await _userBetService.GetUserBetById(id);
                if (userBet == null)
                {
                    return Json(new ResponseModel<UserBetDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "UserBet not found"
                    });
                }

                //await _userBetService.DeleteUserBet(userBet);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete userBet successfully"
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