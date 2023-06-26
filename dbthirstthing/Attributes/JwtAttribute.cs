using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace dbthirstthing.Attributes
{
    public class JwtAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies["jwt"];
            if (cookie != null)
            {
                var token = cookie.Value;
                try
                {
                    var secretKey = "MySecretKey";
                    var payload = Jose.JWT.Decode(token, Encoding.UTF8.GetBytes(secretKey));

                    // You can extract the user's ID or any other required information from the token payload and check it against your database or other authentication source here
                    if (payload != null)
                        return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                    // Token verification failed
                }
            }
            // No token provided
            return false;
        }
    }
}