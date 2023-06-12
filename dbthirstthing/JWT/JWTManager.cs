using Jose;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbthirstthing.JWT
{
    public class JwtManager
    {
        // You should create your own secret key for HMACSHA256
        private static readonly byte[] secretKey = Convert.FromBase64String("jwtblinclass");

        public static string CreateToken(object payload, TimeSpan expirationTime)
        {
            var payloadJson = JsonConvert.SerializeObject(payload);
            var expirationDate = DateTime.Now.Add(expirationTime);
            var extraHeaders = new Dictionary<string, object>
        {
            { "exp", expirationDate.ToString("O") }
        };

            return Jose.JWT.Encode(payloadJson, secretKey, JwsAlgorithm.HS256, extraHeaders);
        }

        public static T GetPayload<T>(string token)
        {
            var payloadJson = Jose.JWT.Decode(token, secretKey);

            return JsonConvert.DeserializeObject<T>(payloadJson);
        }
    }
}