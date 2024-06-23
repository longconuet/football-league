using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Application.DTOs
{
    public class GroupDto : BaseEntityDto
    {
        public string Name { get; set; }
        public List<TeamDto> Teams { get; set; }
    }
}
