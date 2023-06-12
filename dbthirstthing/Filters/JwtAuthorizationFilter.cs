using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jose;

namespace dbthirstthing.Filters
{
    public class JwtAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = GetTokenFromHeader(context);

            if (!VerifyToken(token))
            {
                context.Result = new HttpNotFoundResult(); //random ex
            }

            base.OnActionExecuting(context);
        }


        private string GetTokenFromHeader(ActionExecutingContext context)
        {
            try
            {
                var header = context.HttpContext.Request.Cookies["token"];

                string headerString = header.ToString();
                if (headerString != null && headerString.StartsWith("Bearer "))
                {
                    return headerString.Substring(7);
                }

                return null;
            }
            catch(Exception ex)
            {
                context.Result = new HttpNotFoundResult();

                return ex.Message;
            }

            


        }

        private bool VerifyToken(string token)
        {
            try
            {
                var payloadJson = Jose.JWT.Decode(token, secretKey);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private const string secretKey = "yourSecretKey";
    }
}