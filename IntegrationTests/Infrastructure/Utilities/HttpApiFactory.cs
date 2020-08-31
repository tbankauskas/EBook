using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using EBook.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationTests.Infrastructure.Utilities
{
    public class HttpApiFactory : WebApplicationFactory<Startup>
    {
        private const string Secret = "wGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwGwG75fzeg==";
        private const string Issuer = "issuer";
        private const string Audience = "audience";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.UseSetting("TokenConfiguration:Secret", Secret);
            builder.UseSetting("TokenConfiguration:Issuer", Issuer);
            builder.UseSetting("TokenConfiguration:Audience", Audience);
            builder.UseSetting("ConnectionStrings:EBookDb", DatabaseUtility.ConnectionString);

        }

        public HttpClient AddAuthorization(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            base.ConfigureClient(client);
            return client;
        }

        private string GetToken()
        {

            var bytes = Encoding.ASCII.GetBytes(Secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(bytes), SecurityAlgorithms.HmacSha256Signature);

            var jwtHeader = new JwtHeader(credentials);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TestUser")
            };

            var jwtPayload = new JwtPayload(
                Issuer,
                Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow
            );

            var token = new JwtSecurityToken(
                jwtHeader,
                jwtPayload
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
