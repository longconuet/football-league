﻿namespace BetFootballLeague.Application.DTOs
{
    public class UpdateUserRequestDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
