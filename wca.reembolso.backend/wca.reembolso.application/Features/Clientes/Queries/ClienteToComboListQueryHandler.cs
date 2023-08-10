﻿using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Common;
using wca.reembolso.application6.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record ClienteToComboListQuerie(int FilialId): IRequest<ErrorOr<IList<ListItem>>>;

    public sealed class ClienteToComboListQueryHandler : IRequestHandler<ClienteToComboListQuerie, ErrorOr<IList<ListItem>>>
    {
        private IClienteRepository _reposistory;
        private IMapper _mapper;
        private ILogger<ClienteToComboListQueryHandler> _logger;

        public ClienteToComboListQueryHandler(IClienteRepository reposistory, IMapper mapper, ILogger<ClienteToComboListQueryHandler> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(ClienteToComboListQuerie request, CancellationToken cancellationToken)
        {
            var query = _reposistory.ToQuery();

            if (request.FilialId > 0)
                query = query.Where(q => q.FilialId.Equals(request.FilialId));

            var items = await query.Where(q =>  q.Ativo).OrderBy(q => q.Nome).ToListAsync();

            return _mapper.Map<List<ListItem>>(items);

        }
    }
}