using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;

namespace BetFootballLeague.Application.Services
{
    public class MatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;

        public MatchService(IMatchRepository matchRepository, IMapper mapper)
        {
            _matchRepository = matchRepository;
            _mapper = mapper;
        }

        public async Task<List<MatchDto>> GetMatches()
        {
            var matches = await _matchRepository.GetMatchesAsync();
            return _mapper.Map<List<MatchDto>>(matches);
        }

        public async Task<MatchDto?> GetMatchById(Guid id)
        {
            var match = await _matchRepository.GetMatchByIdAsync(id);
            return match != null ? _mapper.Map<MatchDto>(match) : null;
        }

        public async Task AddMatch(CreateMatchRequestDto matchDto)
        {
            await _matchRepository.AddMatchAsync(_mapper.Map<LeagueMatch>(matchDto));
        }

        public async Task UpdateMatch(MatchDto matchDto)
        {
            await _matchRepository.UpdateMatchAsync(_mapper.Map<LeagueMatch>(matchDto));
        }

        public async Task DeleteMatch(MatchDto matchDto)
        {
            await _matchRepository.DeleteMatchAsync(_mapper.Map<LeagueMatch>(matchDto));
        }
    }
}
