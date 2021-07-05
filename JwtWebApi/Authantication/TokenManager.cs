using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace JwtWebApi
{
    public class TokenManager
    {
        private static string Secret = "thisismysecretkeywhichisconfidential";
        public static string GenerateToken(string userName)
        {
            Byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[] { new Claim(type: ClaimTypes.Name, value: userName) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(SecurityKey, algorithm: SecurityAlgorithms.HmacSha256Signature),

            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);

        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwttoken = (JwtSecurityToken)handler.ReadJwtToken(token);
                if (jwttoken == null)
                {
                    return null;
                }
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,                    
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out securityToken);

                return principal;


            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static string ValidateToken(string token)
        {

            string userName = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch
            {
                return null;
            }

            Claim userNameClaim = identity.FindFirst(ClaimTypes.Name);
            userName = userNameClaim.Value;
            return userName;

        }
    }
}