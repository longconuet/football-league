namespace BetFootballLeague.Application.DTOs
{
    public class UpdateIsLockedStatusRequestDto
    {
        public Guid MatchId { get; set; }
        public int IsLocked { get; set; }
    }
}
