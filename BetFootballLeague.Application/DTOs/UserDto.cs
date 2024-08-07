﻿using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.Application.DTOs
{
    public class UserDto : BaseEntityDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
        public UserStatusEnum Status { get; set; }
        public int Point { get; set; }
    }
}
