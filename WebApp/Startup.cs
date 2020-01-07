using Microsoft.IdentityModel.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Owin;
using Unity;
using Unity.AspNet.WebApi;
using WebApp.Authorization;

namespace WebApp
{
    public static class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            //configuration.DependencyResolver = new UnityDependencyResolver(_container.Value);
#if DEBUG
            IdentityModelEventSource.ShowPII = true;
#endif   
            //app.UseCors
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost")
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            var audience = "099153c2625149bc8ecb3e85e03f0022";
            var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new Microsoft.Owin.Security.Jwt.SymmetricKeyIssuerSecurityKeyProvider("http://localhost", secret)
                    }
                });


            WebApiConfig.Register(configuration);
            configuration.DependencyResolver = new UnityDependencyResolver(UnityConfig.Container);
            configuration.EnsureInitialized();
            app.UseWebApi(configuration);
            SwaggerConfig.Register(configuration);
        }
    }
}