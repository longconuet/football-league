namespace BetFootballLeague.Application.Services
{
    public class TokenBlacklistService
    {
        private readonly List<string> _blacklist = new();

        public void BlacklistToken(string token)
        {
            _blacklist.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklist.Contains(token);
        }
    }
}
