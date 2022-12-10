using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;
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

        public async Task<IList<UsuarioDto>> GetAll(int filialId)
        {
            List<Usuario> usuarios;
            if (filialId == 1)
                usuarios = await _rm.UsuarioRepository.SelectAll().OrderBy(u => u.Nome).ToListAsync();
            else 
               usuarios =  await _rm.UsuarioRepository.SelectByCondition(u => u.FilialId == filialId).OrderBy(u => u.Nome).ToListAsync();

            return _mapper.Map<IList<UsuarioDto>>(usuarios);
        }

        public async Task<UsuarioDto> GetById(int filialId, int id)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == id);

                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }
                var data = await query.FirstOrDefaultAsync();
                
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

        public async Task<bool> Remove(int filialId, int id)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == id);

                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }
                var baseData = await query.FirstOrDefaultAsync();

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

        public async Task<UsuarioDto> Update(int filialId, UpdateUsuarioDto usuario)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == usuario.Id);

                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }
                var baseData = await query.FirstOrDefaultAsync();
                
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
                data.Password = baseData.Password;
                
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

        public Pagination<UsuarioDto> Paginate(int filialId, int page, int pageSize = 10, string termo = "")
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectAll();
                
                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }

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
