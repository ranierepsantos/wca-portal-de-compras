using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Clientes.Common
{
    public class ListItemCliente
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public IList<CentroCusto> CentroCusto { get; set; } = new List<CentroCusto>();
    }
}
