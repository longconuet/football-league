namespace BetFootballLeague.Application.DTOs
{
    public class UpdateMatchBetStatusRequestDto
    {
        public Guid Id { get; set; }
        public int BetStatus { get; set; }
    }
}
