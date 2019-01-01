using ExpenseApi.Data;
using ExpenseApi.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ExpenseApi.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("auth")]
    public class AuthenticationController : ApiController
    {
        [Route("login")]
        public IHttpActionResult Login([FromBody]User user)
        {            
            var result=CreateToken(user);
            return Ok(result);
        }
        [Route("register")]
        public IHttpActionResult Register([FromBody]User user)
        {
            try
            {
                using (var context=new AppDbContext())
                {
                    var exists = context.users.Any(x => x.Username == user.Username);
                    if(exists)
                    {
                        return BadRequest("user already exists");
                    }
                    context.users.Add(user);
                    context.SaveChanges();
                    return Ok(CreateToken(user));
                }
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }
        }
        private JwtPackage CreateToken(User user)
        {
            var tokenHelper = new JwtSecurityTokenHandler();
            var claims = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, user.Username)
            });
            const string secretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var token = (JwtSecurityToken)tokenHelper.CreateJwtSecurityToken(subject: claims, signingCredentials: signinCredentials);
            var tokenString = tokenHelper.WriteToken(token);

            return new JwtPackage()
            {
                Username = user.Username,
                Token = tokenString,
                ExpiryTime = token.ValidTo
            };
        }
    }
}
