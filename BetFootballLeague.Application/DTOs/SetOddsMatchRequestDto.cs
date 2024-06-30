namespace BetFootballLeague.Application.DTOs
{
    public class SetOddsMatchRequestDto
    {
        public Guid Id { get; set; }
        public double? Odds { get; set; }
        public Guid? UpperDoorTeamId { get; set; }
    }
}
