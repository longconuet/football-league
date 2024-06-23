using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetFootballLeague.WebUI.Controllers
{
    public class RoundController : Controller
    {
        private readonly RoundService _roundService;

        public RoundController(RoundService roundService)
        {
            _roundService = roundService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRoundListAjax()
        {
            try
            {
                List<RoundDto> rounds = await _roundService.GetRounds();

                return Json(new ResponseModel<List<RoundDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = rounds
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<List<RoundDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoundAjax([FromBody] CreateRoundRequestDto request)
        {
            try
            {
                await _roundService.AddRound(request);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new round successfully"
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
        public async Task<IActionResult> GetRoundInfoAjax(Guid id)
        {
            try
            {
                RoundDto? round = await _roundService.GetRoundById(id);
                if (round == null)
                {
                    return Json(new ResponseModel<RoundDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Round not found"
                    });
                }

                return Json(new ResponseModel<RoundDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = round
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel<RoundDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoundAjax([FromBody] UpdateRoundRequestDto request)
        {
            try
            {
                RoundDto? round = await _roundService.GetRoundById(request.Id);
                if (round == null)
                {
                    return Json(new ResponseModel<RoundDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Round not found"
                    });
                }

                round.Name = request.Name;
                round.Index = request.Index;
                await _roundService.UpdateRound(round);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update round successfully"
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
        public async Task<IActionResult> DeleteRoundAjax(Guid id)
        {
            try
            {
                RoundDto? round = await _roundService.GetRoundById(id);
                if (round == null)
                {
                    return Json(new ResponseModel<RoundDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Round not found"
                    });
                }

                await _roundService.DeleteRound(round);

                return Json(new ResponseModel
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete round successfully"
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
