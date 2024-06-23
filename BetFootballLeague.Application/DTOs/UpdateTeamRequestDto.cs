namespace BetFootballLeague.Application.DTOs
{
    public class UpdateTeamRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid GroupId { get; set; }
    }
}
