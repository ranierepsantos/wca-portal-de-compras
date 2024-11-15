using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace wca.share.webapi.Utils
{
    public static class JwtUtils
    {
        static readonly string secret = "UPmwa7LHJ7ZXeiBaIO9KcdEkyTy46j9s";

        public static string GenerateJwtToken(int id, string userName)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secret);
            
            List<Claim> claims= new() {
                new("user_id", id.ToString()),
                new("user_name", userName)
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateJwtToken(string jwt, IMediator mediator) 
        {
            JwtSecurityTokenHandler tokenHandler= new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secret);

            TokenValidationParameters validationParameters = new TokenValidationParameters(){
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(jwt,validationParameters, out SecurityToken validatedToken);
            JwtSecurityToken validatedJWT = (JwtSecurityToken) validatedToken;




            return true;
        }
    }
}