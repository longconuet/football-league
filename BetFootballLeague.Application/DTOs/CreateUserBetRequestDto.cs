namespace BetFootballLeague.Application.DTOs
{
    public class CreateUserBetRequestDto
    {
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public Guid BetTeamId { get; set; }
    }
}
