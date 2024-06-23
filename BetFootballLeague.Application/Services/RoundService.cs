using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using System.Linq.Expressions;

namespace BetFootballLeague.Application.Services
{
    public class RoundService
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IMapper _mapper;

        public RoundService(IRoundRepository roundRepository, IMapper mapper)
        {
            _roundRepository = roundRepository;
            _mapper = mapper;
        }

        public async Task<List<RoundDto>> GetRounds(Expression<Func<Round, bool>>? predicate = null)
        {
            var rounds = await _roundRepository.GetRoundsAsync(predicate);
            return _mapper.Map<List<RoundDto>>(rounds);
        }

        public async Task<RoundDto?> GetRoundById(Guid id)
        {
            var round = await _roundRepository.GetRoundByIdAsync(id);
            return round != null ? _mapper.Map<RoundDto>(round) : null;
        }

        public async Task AddRound(CreateRoundRequestDto roundDto)
        {
            await _roundRepository.AddRoundAsync(_mapper.Map<Round>(roundDto));
        }

        public async Task UpdateRound(RoundDto roundDto)
        {
            await _roundRepository.UpdateRoundAsync(_mapper.Map<Round>(roundDto));
        }

        public async Task DeleteRound(RoundDto roundDto)
        {
            await _roundRepository.DeleteRoundAsync(_mapper.Map<Round>(roundDto));
        }
    }
}
