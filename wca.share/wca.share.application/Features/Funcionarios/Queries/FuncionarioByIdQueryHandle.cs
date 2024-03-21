using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcionarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Queries
{
    public record FuncionarioByIdQuery(int Id) : IRequest<ErrorOr<FuncionarioResponse>>;
    internal sealed class FuncionarioByIdQueryHandle : IRequestHandler<FuncionarioByIdQuery, ErrorOr<FuncionarioResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncionarioByIdQueryHandle> _logger;

        public FuncionarioByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncionarioByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FuncionarioResponse>> Handle(FuncionarioByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Funcionario? data = await _repository.GetDbSet<Funcionario>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Funcionário não localizado!");
                return Error.NotFound(description: $"Funcionário não localizado!");
            }

            return _mapper.Map<FuncionarioResponse>(data);
                

        }
    }
}
