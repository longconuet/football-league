namespace BetFootballLeague.Application.DTOs
{
    public class CreateTeamRequestDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid GroupId { get; set; }
    }
}
