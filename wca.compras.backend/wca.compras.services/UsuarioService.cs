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

                _rm.UsuarioRepository.Attach(data);
                
                foreach(var item in usuario.Cliente)
                {
                    var cli = _mapper.Map<Cliente>(item);
                    _rm.ClienteRepository.Attach(cli);
                    data.Cliente.Add(cli);
                }
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
                var data = await query.Include("Cliente").FirstOrDefaultAsync();
                
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
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == usuario.Id,true);

                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }
                query = query.Include("Cliente");

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

                //Remover Cliente que foram retirados do relacionamento
                baseData.Cliente.ToList().ForEach(cli =>
                {
                    
                    if (usuario.Cliente.Where(p => p.Value == cli.Id).FirstOrDefault() == null)
                    {
                        var cliente = baseData.Cliente.FirstOrDefault(p => p.Id == cli.Id);
                        baseData.Cliente.Remove(cliente);
                    }
                });

                //Adicionar permissões caso tenha novas
                usuario.Cliente.ToList().ForEach(cli =>
                {
                    if (baseData.Cliente.Where(p => p.Id == cli.Value).FirstOrDefault() == null)
                    {
                        var cliente = _mapper.Map<Cliente>(cli);
                        _rm.ClienteRepository.Attach(cliente);
                        baseData.Cliente.Add(cliente);
                    }
                });

                baseData.Ativo = usuario.Ativo;
                baseData.Email = usuario.Email;
                baseData.FilialId = usuario.FilialId;
                baseData.Nome = usuario.Nome;
                baseData.PerfilId = usuario.PerfilId;

                await _rm.SaveAsync();

                return _mapper.Map<UsuarioDto>(baseData);
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
                query = query.Include("Cliente");
                if (filialId == 1)
                {
                    query = query.Include("Filial");
                }
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<UsuarioDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Paginate.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<IList<ListItem>> GetToList(int filialId)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(c => c.Ativo == true);

                if (filialId > 1)
                {
                    query = query.Where(c => c.FilialId == filialId);
                }
                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
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
