using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;

namespace wca.compras.services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepository<Usuario> _repository;
        private IMapper _mapper;

        public UsuarioService(IRepository<Usuario> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> Create(CreateUsuarioDto usuario)
        {
            // checar se o e-mail ja foi utilizado
            if ((await _repository.GetAllAsync(u => u.Email == usuario.Email)) != null)
            {
                throw new Exception("Email já cadastrado");
            }
            
            var data = _mapper.Map<Usuario>(usuario);
            
            await _repository.CreateAsync(data);

            return _mapper.Map<UsuarioDto>(data);
        }

        public async Task<IList<UsuarioDto>> GetAll()
        {
            var list = await _repository.GetAllAsync();

            return _mapper.Map<IList<UsuarioDto>>(list.OrderBy(u => u.Nome));
        }

        public async Task<UsuarioDto> GetById(string id)
        {
            var data = await _repository.GetAsync(id);

            return _mapper.Map<UsuarioDto>(data);
        }

        public Task<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioDto> Update(UpdateUsuarioDto usuario)
        {
            throw new NotImplementedException();
        }
    }
}
