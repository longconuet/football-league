namespace BetFootballLeague.Application.DTOs
{
    public class UpdateMatchScoreRequestDto
    {
        public Guid Id { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
    }
}
