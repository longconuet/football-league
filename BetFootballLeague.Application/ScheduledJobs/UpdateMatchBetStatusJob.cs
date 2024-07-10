using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Application.Services;
using System.Globalization;

namespace BetFootballLeague.Application.ScheduledJobs
{
    public class UpdateMatchBetStatusJob
    {
        private readonly MatchService _matchService;

        public UpdateMatchBetStatusJob(MatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task Execute()
        {
            Console.WriteLine("UpdateMatchBetStatusJob is executing...");

            var timeNow = DateTime.Now;
            var matches = await _matchService.GetMatchesForScheduleJob();
            List<MatchDto> updateMatches = new();

            // Open match
            var notAllowedMatches = matches.Where(x => x.BetStatus == Shared.Enums.MatchBetStatusEnum.NOT_ALLOWED).ToList();
            if (notAllowedMatches.Any())
            {
                int count = 0;
                foreach (var match in notAllowedMatches)
                {
                    var matchTime = DateTime.ParseExact(match.DateTime, "HH:mm dd/MM/yyyy", CultureInfo.CurrentCulture);
                    if (matchTime.Subtract(timeNow).TotalDays <= 1 
                        && match.Team1Id != null 
                        && match.Team2Id != null 
                        && match.Odds != null)
                    {
                        match.BetStatus = Shared.Enums.MatchBetStatusEnum.OPENING;
                        match.IsLockedBet = false;
                        updateMatches.Add(match);
                        count++;
                    }
                }
                Console.WriteLine($"> Opening {count} matches.");
            }

            // Lock bet
            var openingMatches = matches.Where(x => x.BetStatus == Shared.Enums.MatchBetStatusEnum.OPENING).ToList();
            if (openingMatches.Any())
            {
                int count = 0;
                foreach (var match in openingMatches)
                {
                    var matchTime = DateTime.ParseExact(match.DateTime, "HH:mm dd/MM/yyyy", CultureInfo.CurrentCulture);
                    if (matchTime.Subtract(timeNow).TotalMinutes <= 30)
                    {
                        match.IsLockedBet = true;
                        updateMatches.Add(match);
                    }
                }
                Console.WriteLine($"> Locking {count} matches.");
            }

            if (updateMatches.Any())
            {
                await _matchService.UpdateMatchList(updateMatches);
            }

            Console.WriteLine($"> {updateMatches.Count} matches updated.");
            Console.WriteLine(">>> UpdateMatchBetStatusJob is executed.\n");
        }
    }
}
