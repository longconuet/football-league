﻿using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Shared.Enums;
using BetFootballLeague.Shared.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenBlacklistService _tokenBlacklistService;

        public AuthService(IUserRepository userRepository, JwtSettings jwtSettings, IHttpContextAccessor httpContextAccessor, TokenBlacklistService tokenBlacklistService)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _httpContextAccessor = httpContextAccessor;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task<string?> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
            {
                // cookie
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleEnum), user.Role)),
                        new Claim("FullName", user.FullName),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await _httpContextAccessor.HttpContext!.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // token
                return GenerateJwtToken(user.Id, username, user.Role);
            }
            return null;
        }

        public async Task SignOut()
        {
            // jwt
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                _tokenBlacklistService.BlacklistToken(token);
            }

            // cookie
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private string GenerateJwtToken(Guid userId, string username, RoleEnum role)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, username),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim("Role", Enum.GetName(typeof(RoleEnum), role) ?? "")  // Thêm claim Role
            //};

            //var token = new JwtSecurityToken(
            //    issuer: jwtSettings.Issuer,
            //    audience: jwtSettings.Audience,
            //    claims: claims,
            //    expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryMinutes),
            //    signingCredentials: credentials
            //);

            //var claims1 = new List<Claim>
            //        {
            //            new Claim(ClaimTypes.Name, user.Username),
            //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //            new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleEnum), user.Role)),
            //            new Claim("FullName", user.FullName),
            //        };

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{

            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, user.UserID),
            //        new Claim(ClaimTypes.Role, user.Role.ToString()),
            //                // Add more claims as needed
            //    }),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //    Issuer = _configuration["Jwt:Issuer"], // Add this line
            //    Audience = _configuration["Jwt:Audience"]
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleEnum), role) ?? ""),
                }),
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}