using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using JWT.Builder;

namespace dbthirstthing.JWT
{
    public class JWTManager
    {
        public string CreateToken(X509Certificate2 certificate, string claim1, string claim1_value, string claim2, string claim2_value)
        {
            return JwtBuilder.Create()
                                  .WithAlgorithm(new RS256Algorithm(certificate))
                                  .AddClaim(claim1, claim1_value)
                                  .AddClaim(claim2, claim2_value)
                                  .Encode();
 

        }

        public string VerifyToken(X509Certificate2 certificate,string token)
        {
            return JwtBuilder.Create()
                     .WithAlgorithm(new RS256Algorithm(certificate))
                     .MustVerifySignature()
                     .Decode(token);
        }
    }
}