using System;
using Microsoft.AspNet.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using VitalSigns.API.Security;
using VitalSigns.API.Models;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalSigns.API.Controllers
{

    [Route("[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions tokenOptions;

        public TokenController(TokenAuthOptions tokenOptions)
        {
            this.tokenOptions = tokenOptions;
        }

        [HttpPost]
        public dynamic Post([FromBody]AuthRequest req)
        {
            string login = req.Username;
            string password = req.Password;

            var dataContext = new DataContext();

            // TODO: retrieve tenant ID from configuration
            Profile currentUser = dataContext.Profiles.Find(p => p.TenantId == 5 && p.Email == req.Username).FirstOrDefault();
            
            if (currentUser == default(Profile) || !Startup.VerifyData(
                req.Password,
                currentUser.Hash))
                return new { authenticated = false, error = "Invalid username or password" };

            // TODO: configure token lifetime
            DateTime? expires = DateTime.UtcNow.AddDays(30);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(req.Username), new[]
            {
                new Claim(ClaimTypes.GivenName, currentUser.FullName),
                new Claim(ClaimTypes.Email, currentUser.Email),

                new Claim("created_on", currentUser.Created.ToString("o"), ClaimValueTypes.DateTime),
                new Claim("modified_on", currentUser.Modified.ToString("o"), ClaimValueTypes.DateTime),

                new Claim("tenant_id", currentUser.TenantId.ToString(), ClaimValueTypes.Integer),
            });

            foreach (var role in currentUser.Roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role));

            var securityToken = jwtTokenHandler.CreateToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                signingCredentials: tokenOptions.SigningCredentials,
                subject: identity,
                expires: expires
            );

            return new
            {
                authenticated = true,
                token = jwtTokenHandler.WriteToken(securityToken),
                tokenExpires = expires
            };
        }
    }
}
