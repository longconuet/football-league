using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public RoleEnum Role { get; set; }
        public UserStatusEnum Status { get; set; }
        public int Point { get; set; }
    }
}
