namespace BetFootballLeague.Application.DTOs
{
    public class UserBetDto : BaseEntityDto
    {
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public Guid BetTeamId { get; set; }
        public bool IsWin { get; set; }

        public UserDto User { get; set; }
        public MatchDto Match { get; set; }
        public TeamDto BetTeam { get; set; }
    }
}
