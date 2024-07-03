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

        public async Task<UserBetDto?> GetUserBetById(Guid id)
        {
            var match = await _userBetRepository.GetUserBetByIdAsync(id);
            return match != null ? _mapper.Map<UserBetDto>(match) : null;
        }

        public async Task AddUserBet(CreateUserBetRequestDto matchDto)
        {
            await _userBetRepository.AddUserBetAsync(_mapper.Map<UserBet>(matchDto));
        }

        public async Task UpdateUserBet(UserBetDto matchDto)
        {
            await _userBetRepository.UpdateUserBetAsync(_mapper.Map<UserBet>(matchDto));
        }

        public async Task DeleteUserBet(Guid id)
        {
            await _userBetRepository.DeleteUserBetAsync(id);
        }
    }
}
