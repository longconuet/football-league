using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Shared.Enums;
using System.Linq.Expressions;

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

        public async Task<List<MatchDto>> GetMatches(MatchBetStatusEnum? status = null)
        {
            Expression<Func<LeagueMatch, bool>>? filter = null;
            if (status != null)
            {
                filter = x => x.BetStatus == status.Value;
            }

            var matches = await _matchRepository.GetMatchesAsync(filter);
            return _mapper.Map<List<MatchDto>>(matches);
        }

        public async Task<List<MatchDto>> GetMatchesForScheduleJob()
        {
            Expression<Func<LeagueMatch, bool>>? filter = x => x.BetStatus == MatchBetStatusEnum.NOT_ALLOWED
                || (x.BetStatus == MatchBetStatusEnum.OPENING && x.IsLockedBet == false);

            var matches = await _matchRepository.GetMatchesAsync(filter, false, false);
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

        public async Task UpdateMatchList(List<MatchDto> matchDtos)
        {
            await _matchRepository.UpdateMatchListAsync(_mapper.Map<List<LeagueMatch>>(matchDtos));
        }

        public async Task DeleteMatch(Guid id)
        {
            await _matchRepository.DeleteMatchAsync(id);
        }
    }
}
