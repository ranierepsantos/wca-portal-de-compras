using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Filiais.Command
{
    public record FilialCreateUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<bool>>;
    internal class FilialCreateUpdateCommandHandle : IRequestHandler<FilialCreateUpdateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialCreateUpdateCommandHandle> _logger;

        public FilialCreateUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FilialCreateUpdateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(FilialCreateUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando criação / atualização de Filial");


            var filial = await _repository.GetDbSet<Filial>()
                           .FirstOrDefaultAsync(q => q.Id.Equals(request.Id), cancellationToken: cancellationToken);

            if (filial == null)
            {
                filial = _mapper.Map<Filial>(request);
                _repository.GetDbSet<Filial>().Add(filial);
                _logger.LogInformation($"Criando usuario {filial.Nome}");
            }
            else
            {
                filial = _mapper.Map<Filial>(request);
                _repository.GetDbSet<Filial>().Entry(filial).State = EntityState.Modified;
                _logger.LogInformation($"Atualizando usuario {filial.Nome}");
            }
            await _repository.SaveAsync();
            return true;
        }
    }
}
