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

                foreach (var item in usuario.TipoFornecimento)
                {
                    var categoria = _mapper.Map<TipoFornecimento>(item);
                    _rm.TipoFornecimentoRepository.Attach(categoria);
                    data.TipoFornecimento.Add(categoria);
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

        public async Task<UsuarioDto> GetById(int filialId, int id, int sistemaId)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == id);

                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }

                query = query.Include(q => q.UsuarioSistemaPerfil.Where(c => c.SistemaId == sistemaId))
                             .Where(c => c.UsuarioSistemaPerfil.Any(q => q.SistemaId == sistemaId));

                Usuario data = new Usuario();

                if (sistemaId == 1) 
                    data = await GetDataToCompras(query);
                else if (sistemaId == 2)
                    data = await GetDataToReembolso(query);

                if (data == null)
                    return null;
                
                return _mapper.Map<UsuarioDto>(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.GetById.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }

        }

        public async Task<bool> Remove(int filialId, int id, int sistemaId = 0)
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

        public async Task<UsuarioDto> Update(int sistemaId, int filialId, UpdateUsuarioDto usuario)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == usuario.Id, true);

                if (filialId > 1)
                {
                    query = query.Where(u => u.FilialId == filialId);
                }
                
                query = query.Include("UsuarioSistemaPerfil");

                //Retorna dados especificos do sistema
                if (sistemaId == 1 ) //compras
                {
                    query = query.Include("Cliente").Include("TipoFornecimento");
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

                if (sistemaId == 1)
                {
                    await UpdateComprasRelacoes(baseData, usuario);
                }
                

                //atualizar/incluir perfil
                var perfilUsuario = baseData.UsuarioSistemaPerfil.FirstOrDefault(q => q.SistemaId == sistemaId);
                if(perfilUsuario == null) {
                    baseData.UsuarioSistemaPerfil.Add(usuario.UsuarioSistemaPerfil[0]);
                }else {
                    int index = baseData.UsuarioSistemaPerfil.IndexOf(perfilUsuario);
                    baseData.UsuarioSistemaPerfil[index] = usuario.UsuarioSistemaPerfil[0];
                }

                baseData.Ativo = usuario.Ativo;
                baseData.Email = usuario.Email;
                baseData.FilialId = usuario.FilialId;
                baseData.Nome = usuario.Nome;
                
                await _rm.SaveAsync();
                
                return _mapper.Map<UsuarioDto>(baseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Update.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public Pagination<UsuarioDto> Paginate(int filialId, int sistemaId, int page, int pageSize = 10, string termo = "")
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
                
                if (filialId == 1)
                {
                    query = query.Include("Filial");
                }
                query = query.Include(q => q.UsuarioSistemaPerfil.Where(c=> c.SistemaId == sistemaId))
                             .Where(c => c.UsuarioSistemaPerfil.Any(q => q.SistemaId == sistemaId));

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

        public async Task<IList<ListItem>> GetToList(int filialId, int sistemaId)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(c => c.Ativo == true);

                if (filialId > 1)
                {
                    query = query.Where(c => c.FilialId == filialId);
                }
                
                query = query.Where(c => c.UsuarioSistemaPerfil.Any(q => q.SistemaId == sistemaId));

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

        private async Task<Usuario> GetDataToCompras(IQueryable<Usuario> query)
        {
            var data = await query.Include("Cliente")
                                  .Include(u => u.TipoFornecimento)
                                  .FirstOrDefaultAsync();
            return data;
        }

        private async Task<Usuario> GetDataToReembolso(IQueryable<Usuario> query)
        {
            var data = await query.FirstOrDefaultAsync();
            return data;
        }

        private async Task<bool> UpdateComprasRelacoes(Usuario usuario ,UpdateUsuarioDto updateUsuario) 
        {
            
            //Remover Cliente que foram retirados do relacionamento
            usuario.Cliente.ToList().ForEach(cli =>
            {

                if (updateUsuario.Cliente.Where(p => p.Value == cli.Id).FirstOrDefault() == null)
                {
                    var cliente = usuario.Cliente.FirstOrDefault(p => p.Id == cli.Id);
                    usuario.Cliente.Remove(cliente);
                }
            });

            //Remover Tipo de Fornecimento que foram retirados do relacionamento
            usuario.TipoFornecimento.ToList().ForEach(tipo =>
            {
                if (updateUsuario.TipoFornecimento.Where(p => p.Value == tipo.Id).FirstOrDefault() == null)
                {
                    var tp = usuario.TipoFornecimento.FirstOrDefault(p => p.Id == tipo.Id);
                    usuario.TipoFornecimento.Remove(tp);
                }
            });

            //Adicionar cliente caso tenha novas
            updateUsuario.Cliente.ToList().ForEach(cli =>
            {
                if (usuario.Cliente.Where(p => p.Id == cli.Value).FirstOrDefault() == null)
                {
                    var cliente = _mapper.Map<Cliente>(cli);
                    _rm.ClienteRepository.Attach(cliente);
                    usuario.Cliente.Add(cliente);
                }
            });

            //Adicionar Tipo Fornecimento caso tenha novas
            updateUsuario.TipoFornecimento.ToList().ForEach(tip =>
            {
                if (usuario.TipoFornecimento.Where(p => p.Id == tip.Value).FirstOrDefault() == null)
                {
                    var tipo = _mapper.Map<TipoFornecimento>(tip);
                    _rm.TipoFornecimentoRepository.Attach(tipo);
                    usuario.TipoFornecimento.Add(tipo);
                }
            });

            return true;
        }

    }
}
