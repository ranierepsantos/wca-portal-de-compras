using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{
    public record AssuntoListQuery () : IRequest<ErrorOr<IList<ListItem>>>;

    internal class AssuntoListQueryHandle : IRequestHandler<AssuntoListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<AssuntoListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public AssuntoListQueryHandle(IRepositoryManager repository, ILogger<AssuntoListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(AssuntoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Assunto>? items = await _repository.GetDbSet<Assunto>()
                                            .AsNoTracking()
                                            .OrderBy(x =>  x.Nome)
                                            .ToListAsync(cancellationToken: cancellationToken);

                return _mapper.Map<List<ListItem>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Error.Failure("", ex.Message);
            }
        }
    }
}
