using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;

namespace BetFootballLeague.Application.Services
{
    public class GroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<List<GroupDto>> GetGroups()
        {
            var groups = await _groupRepository.GetLeagueGroupsAsync();
            return _mapper.Map<List<GroupDto>>(groups);
        }

        public async Task<GroupDto?> GetGroupById(Guid id)
        {
            var group = await _groupRepository.GetLeagueGroupByIdAsync(id);
            return group != null ? _mapper.Map<GroupDto>(group) : null;
        }

        public async Task AddGroup(CreateGroupRequestDto groupDto)
        {
            await _groupRepository.AddLeagueGroupAsync(_mapper.Map<LeagueGroup>(groupDto));
        }

        public async Task UpdateGroup(GroupDto groupDto)
        {
            await _groupRepository.UpdateLeagueGroupAsync(_mapper.Map<LeagueGroup>(groupDto));
        }

        public async Task DeleteGroup(GroupDto groupDto)
        {
            await _groupRepository.DeleteLeagueGroupAsync(_mapper.Map<LeagueGroup>(groupDto));
        }
    }
}
