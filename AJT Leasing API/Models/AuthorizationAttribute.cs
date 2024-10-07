using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AJT_Leasing_API.Models
{
    public class AuthorizationAttribute : AuthorizationFilterAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers != null && actionContext.Request.Headers.Authorization != null && !string.IsNullOrEmpty(actionContext.Request.Headers.Authorization.Parameter))
                {
                    var authToken = actionContext.Request.Headers
                        .Authorization.Parameter;

                    // decoding authToken we get decode value in 'Username:Password' format
                    var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                        Convert.FromBase64String(authToken));

                    // spliting decodeauthToken using ':' 
                    var arrUserNameandPassword = decodeauthToken.Split(':');

                    // at 0th postion of array we get username and at 1st we get password
                    if (DataAccessLayer.IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                    {
                        // setting current principle
                        Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(arrUserNameandPassword[0]), null);

                    }
                    else
                    {
                        actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);

                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request
                     .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, "SSL Testing", string.Empty, "SSL Testing");
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}