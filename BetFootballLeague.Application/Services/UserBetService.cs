using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Repositories;

namespace BetFootballLeague.Application.Services
{
    public class UserBetService
    {
        private readonly IUserBetRepository _userBetRepository;
        private readonly IMapper _mapper;

        public UserBetService(IUserBetRepository userBetRepository, IMapper mapper)
        {
            _userBetRepository = userBetRepository;
            _mapper = mapper;
        }

        public async Task<List<UserBetDto>> GetBetsByUser(Guid userId)
        {
            var userBets = await _userBetRepository.GetBetsByUserAsync(userId);
            return _mapper.Map<List<UserBetDto>>(userBets);
        }

        public async Task<List<UserBetDto>> GetBetsByMatch(Guid matchId)
        {
            var userBets = await _userBetRepository.GetBetsByMatchAsync(matchId);
            return _mapper.Map<List<UserBetDto>>(userBets);
        }

        public async Task<UserBetDto?> GetUserBetById(Guid id)
        {
            var userBet = await _userBetRepository.GetUserBetByIdAsync(id);
            return userBet != null ? _mapper.Map<UserBetDto>(userBet) : null;
        }

        public async Task<UserBetDto?> GetUserBetByMatchId(Guid userId, Guid matchId)
        {
            var userBet = await _userBetRepository.GetUserBetByMatchIdAsync(userId, matchId);
            return userBet != null ? _mapper.Map<UserBetDto>(userBet) : null;
        }

        public async Task AddUserBet(CreateUserBetRequestDto userBetDto)
        {
            await _userBetRepository.AddUserBetAsync(_mapper.Map<UserBet>(userBetDto));
        }

        public async Task UpdateUserBet(UserBetDto userBetDto)
        {
            await _userBetRepository.UpdateUserBetAsync(_mapper.Map<UserBet>(userBetDto));
        }

        public async Task UpdateUserBetsResultForEndedMatch(MatchDto matchDto, List<UserBetDto> userBetDtos)
        {
            foreach (var userBetDto in userBetDtos)
            {
                userBetDto.IsWin = matchDto.WinBetTeamId == userBetDto.BetTeamId;
            }

            await _userBetRepository.UpdateUserBetsAsync(_mapper.Map<List<UserBet>>(userBetDtos));
        }

        public async Task DeleteUserBet(Guid id)
        {
            await _userBetRepository.DeleteUserBetAsync(id);
        }
    }
}
