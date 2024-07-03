namespace BetFootballLeague.Application.DTOs
{
    public class UpdateUserBetRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public Guid BetTeamId { get; set; }
    }
}
