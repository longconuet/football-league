using BetFootballLeague.Application.Services;
using Microsoft.AspNetCore.Http;

namespace BetFootballLeague.Application.Middlewares
{
    public class BlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenBlacklistService _tokenBlacklistService;

        public BlacklistMiddleware(RequestDelegate next, TokenBlacklistService tokenBlacklistService)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token) && _tokenBlacklistService.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            await _next(context);
        }
    }

}
