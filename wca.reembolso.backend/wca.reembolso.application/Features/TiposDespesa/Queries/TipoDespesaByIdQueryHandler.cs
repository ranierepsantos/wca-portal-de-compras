using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Queries
{
    public record TipoDespesaByIdQuerie(int Id) : IRequest<ErrorOr<TipoDespesaResponse>>;
    public class TipoDespesaByIdQueryHandler : IRequestHandler<TipoDespesaByIdQuerie, ErrorOr<TipoDespesaResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaByIdQueryHandler> _logger;

        public TipoDespesaByIdQueryHandler(IRepositoryManager reposistory, IMapper mapper, ILogger<TipoDespesaByIdQueryHandler> logger)
        {
            _repository = reposistory;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<TipoDespesaResponse>> Handle(TipoDespesaByIdQuerie request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando tipo de despesa por id");

            var tipoDespesa = await _repository.TipoDespesaRepository.ToQuery()
                                    .Where(q => q.Id.Equals(request.Id))
                                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (tipoDespesa == null)
            {
                _logger.LogError($"Tipo despesa não localizado!");
                return Error.NotFound(description: $"Tipo despesa não localizado!");
            }

            return _mapper.Map<TipoDespesaResponse>(tipoDespesa);

        }
    }
}
