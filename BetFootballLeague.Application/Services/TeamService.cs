using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;

namespace BetFootballLeague.Application.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public async Task<List<TeamDto>> GetTeams(bool includeGroup = false)
        {
            var teams = await _teamRepository.GetTeamsAsync(includeGroup);
            return _mapper.Map<List<TeamDto>>(teams);
        }

        public async Task<TeamDto?> GetTeamById(Guid id)
        {
            var team = await _teamRepository.GetTeamByIdAsync(id);
            return team != null ? _mapper.Map<TeamDto>(team) : null;
        }

        public async Task AddTeam(CreateTeamRequestDto teamDto)
        {
            await _teamRepository.AddTeamAsync(_mapper.Map<Team>(teamDto));
        }

        public async Task UpdateTeam(TeamDto teamDto)
        {
            await _teamRepository.UpdateTeamAsync(_mapper.Map<Team>(teamDto));
        }

        public async Task DeleteTeam(TeamDto teamDto)
        {
            await _teamRepository.DeleteTeamAsync(_mapper.Map<Team>(teamDto));
        }
    }
}
