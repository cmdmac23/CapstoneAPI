using CapstoneAPI_new.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneAPI_new.Controllers
{

    [ApiController]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Route("api/authenticate")]
    public class JWTAuthenticateController : Controller
    {
        private readonly IConfiguration _config;
        private readonly Dictionary<string, string> validUsers = new();

        public JWTAuthenticateController(IConfiguration config)
        {
            _config = config;

            validUsers.Add("inFocus", "g=Q4c$A!*-Emf7rC");
        }

        [HttpGet]
        public object Authenticate([FromHeader] AuthorizationHeader login)
        {
            object response;

            var user = AuthenticateLogin(login);

            if (user != null)
            {
                var tokenString = GenerateJWT(user);
                response = new JsonResult(new { token = tokenString }) { StatusCode = 200 };
            }
            else
            {
                response = new ContentResult() { StatusCode = 401 };
            }
            return response;
        }

        private string GenerateJWT(AuthorizationJWT user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddHours(3), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private AuthorizationJWT AuthenticateLogin (AuthorizationHeader login)
        {
            AuthorizationJWT user = null;

            if (validUsers.Any(x => x.Key == login.username && x.Value == login.password))
            {
                user = new AuthorizationJWT { user = login.username };
            }

            return user;
        }
    }
}
