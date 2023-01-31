using Microsoft.AspNetCore.Mvc;
using NewsRSS.API.Data;

namespace NewsRSS_API_test.Controllers
{
    public class UserController : Controller
    {

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
    }
}
