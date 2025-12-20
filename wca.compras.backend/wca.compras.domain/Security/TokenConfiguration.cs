using System.Security.Cryptography.X509Certificates;

namespace wca.compras.domain.Security
{
    public class TokenConfiguration
    {
        public List<string> Audience { get; set; } = new List<string>();
        public string Issuer { get; set; }
        public int Seconds { get; set; }

        public string Secret { get; set; }
    }
}