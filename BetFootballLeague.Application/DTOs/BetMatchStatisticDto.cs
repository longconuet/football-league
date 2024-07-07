using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.Application.DTOs
{
    public class BetMatchStatisticDto
    {
        public MatchStatisticDto Match { get; set; }
        public TeamStatisticDto Team1 { get; set; }
        public TeamStatisticDto Team2 { get; set; }
        public List<UserStatisticDto> NotBetUsers { get; set; }
        public ChartBetStatisticDto ChartInfo { get; set; }
    }

    public class MatchStatisticDto
    {
        public Guid Id { get; set; }
        public string RoundName { get; set; }
        public string DateTime { get; set; }
        public MatchBetStatusEnum BetStatus { get; set; }
        public double Odds { get; set; }
        public Guid? WinBetTeamId { get; set; }
    }

    public class TeamStatisticDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsUpperDoor { get; set; }
        public int? Score { get; set; }
        public List<UserStatisticDto> BetUsers { get; set; }
    }

    public class UserStatisticDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string BetTime { get; set; }
    }

    public class ChartBetStatisticDto
    {
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int BetTeam1Number { get; set; }
        public int BetTeam2Number { get; set; }
        public int NotBetNumber { get; set; }
    }
}
