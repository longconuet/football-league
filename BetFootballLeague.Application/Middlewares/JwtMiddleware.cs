using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BetFootballLeague.Application.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next, JwtSettings jwtSettings)
        {
            _next = next;
            _jwtSettings = jwtSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            // Logic để bỏ qua xác thực cho một số endpoint cụ thể
            if (context.Request.Path.StartsWithSegments("/auth/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies["jwtToken"];
            if (token == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            AttachUserToContext(context, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "UserId").Value;

                context.Items["User"] = new { Id = userId };
            }
            catch
            {
                
            }
        }
    }

}
