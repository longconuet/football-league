using BetFootballLeague.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetFootballLeague.Application.Middlewares
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public UserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity!.IsAuthenticated)
            {
                string userName = context.User.Identity.Name;

                var claims = context.User.Claims;
                var role = claims.First(c => c.Type == ClaimTypes.Role).Value;
                //var userId = claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                //context.Items["UserId"] = userId;
                context.Items["Username"] = userName;
                context.Items["Role"] = role;
            }

            await _next(context);
        }
    }
}
