using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EBook.Data.Entities;
using EBook.Services.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EBook.Services.Jwt
{
    public class JwtTokenFactory
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public JwtTokenFactory(IOptions<TokenConfiguration> tokenOptions)
        {
            _tokenConfiguration = tokenOptions.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var issued = DateTime.UtcNow;
            var expires = DateTime.Now.AddDays(_tokenConfiguration.AccessExpiration);
            var jwtHeader = new JwtHeader(credentials);
            var jwtPlayload = new JwtPayload(_tokenConfiguration.Issuer, _tokenConfiguration.Audience, claims, issued, expires, issued);
            var jwtToken = new JwtSecurityToken(jwtHeader, jwtPlayload);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
