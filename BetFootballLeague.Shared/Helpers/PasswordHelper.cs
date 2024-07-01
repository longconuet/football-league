namespace BetFootballLeague.Shared.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPasword(string password)
        {
           return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        public static bool VerifyPassword(string checkPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(checkPassword, hashPassword);
        }
    }
}
