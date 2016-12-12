using System;
using Microsoft.AspNet.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using VitalSigns.API.Security;
using VitalSigns.API.Models;
using MongoDB.Driver;
using System.Web.Security;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalSigns.API.Controllers
{

    [Route("[controller]")]
    public class TokenController : BaseController
    {
        private readonly TokenAuthOptions tokenOptions;

        private IRepository<Users> maintainUsersRepository;

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

        [HttpGet("reset_password")]
        public APIResponse ResetPassword(string emailId, string password)
        {
            try
            {
                maintainUsersRepository = new Repository<Users>(ConnectionString);
                var user = maintainUsersRepository.Collection.AsQueryable().FirstOrDefault(x => x.Email == emailId);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(password))
                        password = Membership.GeneratePassword(6, 2);
                    string hashedPassword = Startup.SignData(password);
                    maintainUsersRepository = new Repository<Users>(ConnectionString);
                    FilterDefinition<Users> filterDefination = Builders<Users>.Filter.Where(x => x.Email == emailId);
                    var updatePassword = maintainUsersRepository.Updater.Set(y => y.Hash, hashedPassword)
                                                                        .Set(y => y.IsPasswordResetRequired, true);
                    var result = maintainUsersRepository.Update(filterDefination, updatePassword);

                  (new Common()).SendPasswordEmail(emailId, password);
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Password Reset done successfully and check your email");
                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "User is not available in the database");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error occured value reset the password:" + exception);
            }
            return Response;
        }
    }
}
