namespace BetFootballLeague.Application.DTOs
{
    public class TeamDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid GroupId { get; set; }
        public int IsEliminated { get; set; } = 0;
        public GroupDto Group { get; set; }
    }
}
