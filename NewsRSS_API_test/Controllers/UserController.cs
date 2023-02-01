using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsRSS.API.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NewsRSS_API_test.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration _config;

        /// <summary>
        /// Constructor for the controller, sets appsettings.json as config
        /// </summary>
        /// <param name="config">Current configuration</param>
        public UserController(IConfiguration config)
        {
            _config = config;
        }
        
        /// <summary>
        /// Adds a user into the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>Bad request if name or password are invalid, OK if otherwise</returns>
        [AllowAnonymous]
        [HttpPost("{name}, {password}")]
        public IActionResult Register(string name, string password)
        {
            if(name.Length == 0 || password.Length < 4)
            {
                return BadRequest();
            }
            using (var context = new RSSFeedDataContext())
            {
                context.Users.Add(new User(name, password));
                context.SaveChanges();
            }
            return StatusCode(200);
        }

        /// <summary>
        /// Cheks if a user is in DB, then returns a JWT token that can be used to authorize
        /// </summary>
        /// <param name="name">Username, case sensitive</param>
        /// <param name="password">User password</param>
        /// <returns>Not found if there's no such user, Ok with the token if otherwise</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(string name, string password) 
        {
            var isUser = IsAuthorizedUser(name, password);

            if (isUser) 
            {
                var token = Generate(name, password);
                return Ok(token);
            }
            return NotFound();
        }

        /// <summary>
        /// Generates a JWT token that is used to authorize into the API. The token is valid for 15 minutes
        /// </summary>
        /// <param name="name">Username, case sensitive</param>
        /// <param name="password">User password</param>
        /// <returns>A JWT token string</returns>
        private string Generate(string name, string password) 
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, name)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
             _config["Jwt:Audience"],
             claims,
             expires: DateTime.Now.AddMinutes(15),
             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Checks if given pair of name and password exist in the DB
        /// </summary>
        /// <param name="username">Username, case sensitive</param>
        /// <param name="password">User password</param>
        /// <returns>True, if such user exists and the password is correct, false if otherwise</returns>
        private bool IsAuthorizedUser(string username, string password)
        {
            using (var context = new RSSFeedDataContext())
            {
                if (context.Users.Where(x => x.Name == username && x.Password == password).Single() != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
