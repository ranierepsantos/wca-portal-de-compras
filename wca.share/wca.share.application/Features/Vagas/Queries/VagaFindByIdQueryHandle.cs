using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Vagas.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Queries
{
    public record VagaFindByIdQuery(int Id): IRequest<ErrorOr<VagaResponse>>;
    internal class VagaFindByIdQueryHandle : IRequestHandler<VagaFindByIdQuery, ErrorOr<VagaResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VagaFindByIdQueryHandle> _logger;

        public VagaFindByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<VagaFindByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<VagaResponse>> Handle(VagaFindByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Buscar pelo id: {request.Id}");

                Vaga? data = await _repository.GetDbSet<Vaga>()
                                .Where(q => q.Id.Equals(request.Id))
                                .AsNoTracking()
                                .Include(q => q.Cliente)
                                .Include(q => q.DocumentoComplementares)
                                .Include(q => q.Escala)
                                .Include(q => q.Escolaridade)
                                .Include(q => q.Funcao)
                                .Include(q => q.Gestor)
                                .Include(q => q.Horario)
                                .Include(q => q.MotivoContratacao)
                                .Include(q => q.Sexo)
                                .Include(q => q.StatusSolicitacao)
                                .Include(q => q.TipoContrato)
                                .Include(q => q.TipoFaturamento)
                                .Include(q => q.Responsavel)
                                .Include(q => q.VagaHistorico.OrderByDescending(f => f.DataHora))
                                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (data == null)
                {
                    _logger.LogWarning($"Vaga não localizada!");
                    return Error.NotFound(description: $"Vaga não localizado!");
                }

                return _mapper.Map<VagaResponse>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error.Message: {ex.Message}");
                _logger.LogError($"Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }
         
        }
    }
}
