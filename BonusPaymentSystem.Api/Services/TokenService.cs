using BonusPaymentSystem.Api.Models;
using BonusPaymentSystem.Api.Models.Security;
using BonusPaymentSystem.Core.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Services
{
    public static class TokenService
    {
        public static Token CreateToken(UserModel user, string secret, int totalSeconds = 3000)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(totalSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);


            return new Token { AccessToken = tokenHandler.WriteToken(token), ExpireIn = totalSeconds };
        }


        public static string CreateTokenV2(User user, string secret, string subject = "JWTServiceAccessToken",
                                          string issuer = "JWTAuthenticationServer",
                                          string audience = "JWTServicePostmanClient",
                                          int totalSeconds = 3000)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
                        new Claim("DisplayName", user.FirstName + " " +  user.LastName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                                    issuer: issuer,
                                    audience: audience,
                                    claims: claims,
                                    expires: DateTime.UtcNow.AddSeconds(totalSeconds),
                                    signingCredentials: signIn);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
