using Microsoft.Owin;
using Owin;

using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using dbthirstthing.Models;
using dbthirstthing.DataContext;
using Microsoft.Owin.Security.Google;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Schema;

[assembly: OwinStartup(typeof(AspNetIdentityApp.Startup))]

namespace AspNetIdentityApp
{
    public class Startup
    {
        private const string GoogleClientId = "YOUR_CLIENT_ID_HERE";
        private const string GoogleClientSecret = "YOUR_CLIENT_SECRET_HERE";

        public void ConfigureAuth(IAppBuilder app)
        {
            // Использование cookie для сохранения информации о пользователе, входящего в систему
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Использование Google OAuth для аутентификации
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = GoogleClientId,
                ClientSecret = GoogleClientSecret,
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        // Обработка полученных данных о пользователе из Google
                        context.Identity.AddClaim(new Claim("urn:google:accesstoken", context.AccessToken, XmlSchemaЫекштп, "Google"));
                        context.Identity.AddClaim(new Claim("urn:google:email", context.Email, XmlSchemaString, "Google"));
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}