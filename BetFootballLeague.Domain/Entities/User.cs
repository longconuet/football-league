using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; } = RoleEnum.NORMAL_USER;
        public UserStatusEnum Status { get; set; } = UserStatusEnum.Active;
        public int Point { get; set; } = 0;

        public ICollection<UserBet> UserBets { get; set; }
    }
}
