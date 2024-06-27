﻿namespace BetFootballLeague.Application.DTOs
{
    public class CreateMatchRequestDto
    {
        public Guid RoundId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int IndexOrder { get; set; }
        public Guid? Team1Id { get; set; }
        public Guid? Team2Id { get; set; }
    }
}
