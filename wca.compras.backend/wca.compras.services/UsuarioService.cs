using AutoMapper;
using MongoDB.Driver;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepository<Usuario> _repository;
        private IMapper _mapper;
        
        public UsuarioService(IRepository<Usuario> repository, 
                              IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> Create(CreateUsuarioDto usuario)
        {
            
            try
            {
                // checar se o e-mail ja foi utilizado
                if (await IsEmailExists(usuario.Email))
                {
                    throw new Exception("Email já cadastrado");
                }
                var data = _mapper.Map<Usuario>(usuario);
                data.Ativo = true;

                await _repository.CreateAsync(data);

                return _mapper.Map<UsuarioDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Create.Error" + ex.Message);
                throw new Exception (ex.ToString());
            }            
            
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

        public async Task<bool> Remove(string id)
        {
            try
            {
                var baseData = await _repository.GetAsync(id);

                if (baseData == null)
                {
                    return false;
                }
                await _repository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Remove.Error" + ex.ToString);
                throw new Exception(ex.ToString());
            }
            
        }

        public async Task<UsuarioDto> Update(UpdateUsuarioDto usuario)
        {
            try
            {
                var baseData = await _repository.GetAsync(usuario.Id);

                if (baseData == null)
                {
                    return null;
                }

                if (baseData.Email.ToLower() != usuario.Email.ToLower())
                {
                    // Email alterou, checar se o e-mail ja foi utilizado
                    if (await IsEmailExists(usuario.Email))
                    {
                        throw new Exception("Email já cadastrado");
                    }
                }
                
                var data = _mapper.Map<Usuario>(usuario);
                
                await _repository.UpdateAsync(data);

                return _mapper.Map<UsuarioDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Update.Error" + ex.ToString);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<Pagination<UsuarioDto>> Paginate(int page, int pageSize = 10, string termo = "")
        {
            try
            {
                var builder = Builders<Usuario>.Filter;
                FilterDefinition<Usuario> filter = builder.Empty;

                if (!string.IsNullOrEmpty(termo))
                {
                    filter = builder.Regex("nome", new MongoDB.Bson.BsonRegularExpression(termo, "i"));
                }

                var (totalPages, data) = await _repository.Paginate(page, pageSize, filter, Builders<Usuario>.Sort.Ascending(p => p.Nome));

                var listData = _mapper.Map<List<UsuarioDto>>(data.ToList());

                return new Pagination<UsuarioDto>(listData, page, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Paginate.Error" + ex.ToString);
                throw new Exception(ex.ToString());
            }
            


        }

        private async Task<bool> IsEmailExists(string email)
        {
            if ((await _repository.GetAsync(u => u.Email == email)) != null)
            {
                return true;
            }
            return false;
        }
    }
}
