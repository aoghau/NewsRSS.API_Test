using NewsRSS.API.Data;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NewsRSS_API_test
{
    public class BasicAuthAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;
                
                var authTokenDecode = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                var usernameAndPassword = authTokenDecode.Split(':');

                if(IsAuthorizedUser(usernameAndPassword[0], usernameAndPassword[1]))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(usernameAndPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }

        public bool IsAuthorizedUser(string username, string password)
        {
            using (var context = new RSSFeedDataContext())
            {
                if(context.Users.Where(x => x.Name == username && x.Password == password).Single() != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
