using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Taskboard_API.CustomModules
{
	public class AuthorizeJwtAttribute : AuthorizeAttribute
	{
        private new List<string> Roles;
        public static string secretKey = "My super secret key used for encoding the token";

        public AuthorizeJwtAttribute(params string[] roles) : base()
        {
            Roles = roles.ToList();
        }
		public override void OnAuthorization(HttpActionContext actionContext)
		{
            string token;
            if (!TryRetrieveToken(actionContext.Request, out token))
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            try
            {
                var now = DateTime.UtcNow;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };
                //extract and assign the user of the jwt
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);
                //check here for user roles
                base.OnAuthorization(actionContext);

            }
            catch (SecurityTokenValidationException e)
            {
                actionContext.Response =
                                     new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                actionContext.Response =
                                      new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }


        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires)
                {
                    return true;
                }
            }
            return false;
        }
    }
}