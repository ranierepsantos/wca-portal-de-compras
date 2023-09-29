using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record ClienteByUserIdQuerie(int UsuarioId) : IRequest<ErrorOr<IList<ClienteResponseWithValorUsado>>>;
    public class ClienteByUserIdQueryHandler : IRequestHandler<ClienteByUserIdQuerie, ErrorOr<IList<ClienteResponseWithValorUsado>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<ClienteByUserIdQueryHandler> _logger;

        public ClienteByUserIdQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<ClienteByUserIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<ClienteResponseWithValorUsado>>> Handle(ClienteByUserIdQuerie request, CancellationToken cancellationToken)
        {

            var query = _repository.ClienteRepository.ToQuery()
                .Include("Solicitacoes")
                .Where(q => q.UsuarioClientes.Any(sq => sq.UsuarioId == request.UsuarioId))
                .Select(f => new ClienteResponseWithValorUsado(
                    f.Id,
                    f.FilialId,
                    f.Nome,
                    f.CNPJ,
                    f.InscricaoEstadual,
                    f.Endereco,
                    f.Numero,
                    f.Cep,
                    f.Cidade,
                    f.UF,
                    f.Ativo,
                    f.ValorLimite,
                    f.Solicitacoes.Where(q=> q.Status != 8 && q.DataSolicitacao.Month.Equals(DateTime.Now.Month)).Sum(q => q.ValorDespesa)
                ));

            List<ClienteResponseWithValorUsado> list = await query.ToListAsync<ClienteResponseWithValorUsado>(cancellationToken: cancellationToken);

            return list;

        }
    }
}
