using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Features.Clientes.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{

    public record ClienteByUserIdQuerie(int UsuarioId): IRequest<ErrorOr<IList<ClienteResponse>>>;
    public class ClienteByUserIdQuerieHandler : IRequestHandler<ClienteByUserIdQuerie, ErrorOr<ClienteResponse>>
    {
        public ClienteByUserIdQuerieHandler()
        {
        }

        public Task<ErrorOr<ClienteResponse>> Handle(ClienteByUserIdQuerie request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
