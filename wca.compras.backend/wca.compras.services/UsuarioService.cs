using AutoMapper;
using Microsoft.EntityFrameworkCore;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepositoryManager _rm;
        private IMapper _mapper;
        
        public UsuarioService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
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

                _rm.UsuarioRepository.Create(data); 
                await _rm.SaveAsync();

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
            var list = await _rm.UsuarioRepository.SelectAll().OrderBy(u => u.Nome).ToListAsync();

            return _mapper.Map<IList<UsuarioDto>>(list);
        }

        public async Task<UsuarioDto> GetById(int id)
        {
            try
            {
                var data = await _rm.UsuarioRepository.SelectByCondition(u => u.Id == id)
                                        .FirstOrDefaultAsync();

                if (data == null)
                {
                    return null;
                }

                return _mapper.Map<UsuarioDto>(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.GetById.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }

        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                var baseData = await _rm.UsuarioRepository.SelectByCondition(u => u.Id == id).FirstOrDefaultAsync();

                if (baseData == null)
                {
                    return false;
                }
                _rm.UsuarioRepository.Delete(baseData);
                
                await _rm.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Remove.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }
            
        }

        public async Task<UsuarioDto> Update(UpdateUsuarioDto usuario)
        {
            try
            {
                var baseData = await _rm.UsuarioRepository.SelectByCondition(u => u.Id == usuario.Id)
                                    .FirstOrDefaultAsync();
                
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
                
                _rm.UsuarioRepository.Update(data);

                await _rm.SaveAsync();

                return _mapper.Map<UsuarioDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Update.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<Pagination<UsuarioDto>> Paginate(int page, int pageSize = 10, string termo = "")
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectAll();

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<UsuarioDto>.ToPagedList(query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Paginate.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }
            


        }

        private async Task<bool> IsEmailExists(string email)
        {
            var data = await _rm.UsuarioRepository.SelectByCondition(u => u.Email == email)
                                        .FirstOrDefaultAsync();
            if ( data != null)
            {
                return true;
            }
            return false;
        }
    }
}
