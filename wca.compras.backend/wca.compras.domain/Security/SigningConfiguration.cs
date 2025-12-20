using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;


namespace wca.compras.domain.Security
{
    public class SigningConfiguration
    {
        public SecurityKey Key { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfiguration(string secret)
        {   
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }

    }
}