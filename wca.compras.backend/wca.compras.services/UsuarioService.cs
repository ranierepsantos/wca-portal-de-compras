using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Immutable;
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

        public async Task<UsuarioDto> Create(int sistemaId, CreateUsuarioDto usuario)
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

                if (sistemaId == 1) //1 - COMPRAS
                    AddComprasRelacoes(data, usuario);
                
                await _rm.SaveAsync();

                return _mapper.Map<UsuarioDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.Create.Error" + ex.Message);
                throw new Exception (ex.ToString());
            }            
            
        }

        public async Task<UsuarioDto> GetById(int id, int sistemaId)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == id)
                               .Include(q => q.Filial.Where(c =>  c.SistemaId == sistemaId))
                               .Include(q => q.UsuarioSistemaPerfil.Where(c => c.SistemaId == sistemaId));


                //query = query.Where(c => c.UsuarioSistemaPerfil.Any(q => q.SistemaId == sistemaId));

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

        public async Task<UsuarioDto> GetByEmail(string email)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Email.ToLower().Equals(email.ToLower())); ;
                query = query.Include(q => q.UsuarioSistemaPerfil);
                
                var data = await query.FirstOrDefaultAsync();

                if (data == null)
                    return null;

                return _mapper.Map<UsuarioDto>(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Usuario.GetByEmail.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }

        }


        public async Task<bool> Remove(int id, int sistemaId = 0)
        {
            throw new NotImplementedException();
            //try
            //{
                

            //    var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == id);

            //    var baseData = await query.FirstOrDefaultAsync();

            //    if (baseData == null)
            //    {
            //        return false;
            //    }
            //    _rm.UsuarioRepository.Delete(baseData);
                
            //    await _rm.SaveAsync();

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Usuario.Remove.Error" + ex.Message);
            //    throw new Exception(ex.ToString());
            //}
            
        }

        public async Task<UsuarioDto> Update(int sistemaId, UpdateUsuarioDto usuario)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(u => u.Id == usuario.Id, true);

                query = query.Include("UsuarioSistemaPerfil")
                             .Include(c =>  c.Filial.Where(q =>  q.SistemaId.Equals(sistemaId)))
                             .Include("UsuarioReembolsoComplemento");

                //Retorna dados especificos do sistema
                if (sistemaId == 1 ) //compras
                {
                    query = query.Include("Cliente")
                                 .Include("TipoFornecimento");
                                 
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

                //Remover Filiais que foram retiradas do relacionamento
                if (usuario.Filial != null)
                {
                    List<Filial> filiais = baseData.Filial
                    .Where(x => !usuario.Filial.Where(q => q.Value == x.Id).Any())
                    .Where(x => x.Id != 0)
                    .ToList();

                    foreach (Filial item in filiais)
                    {
                        baseData.Filial.Remove(item);
                    }
                }

                //Adicionar Filiais caso tenha novas
                usuario.Filial?.ToList().ForEach(item =>
                {
                    if (baseData.Filial.Where(p => p.Id == item.Value).FirstOrDefault() == null)
                    {
                        var filial = _mapper.Map<Filial>(item);
                        _rm.FilialRepository.Attach(filial);
                        baseData.Filial.Add(filial);
                    }
                });

                if (sistemaId == 1)
                {
                    await UpdateComprasRelacoes(baseData, usuario);
                }else if (sistemaId ==2 && usuario.UsuarioReembolsoComplemento != null)
                {
                        baseData.UsuarioReembolsoComplemento = usuario.UsuarioReembolsoComplemento;
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

        public Pagination<UsuarioDto> Paginate(int sistemaId, int page, int pageSize = 10, string termo = "", int[]? filial = null)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectAll();
                
                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                if (filial?.Length > 0)
                {
                    query = query.Where(q => q.Filial.Any(c => filial.Contains(c.Id)));
                }

                query = query.Include(q => q.UsuarioSistemaPerfil.Where(c=> c.SistemaId == sistemaId))
                             .Where(c => c.UsuarioSistemaPerfil.Any(q => q.SistemaId == sistemaId))
                             .Include(c => c.Filial.Where(x => x.SistemaId == sistemaId));

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

        public async Task<IList<ListItem>> GetToList(int sistemaId, int[]? filial)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(c => c.Ativo == true);

                query = query.Where(c => c.UsuarioSistemaPerfil.Any(q => q.SistemaId == sistemaId));

                if (filial?.Length> 0)
                {
                    query = query.Where(q => q.Filial.Any(c => filial.Contains(c.Id)));
                }

                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); 

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ListItem>> GetToListByPerfil(int perfilId)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectByCondition(c => c.Ativo == true);
                query = query.Where(c => c.UsuarioSistemaPerfil.Where(q => q.PerfilId.Equals(perfilId)).Count() > 0);
                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetToListByClientePerfil.Error: {ex.Message}");
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
            query = query.Include(ic => ic.UsuarioReembolsoComplemento);

            var data = await query.FirstOrDefaultAsync();
            
            return data;
        }

        private async Task<bool> UpdateComprasRelacoes(Usuario usuario ,UpdateUsuarioDto updateUsuario) 
        {

            
            //Remover Cliente que foram retirados do relacionamento
            if (updateUsuario.Cliente != null)
            {
                List<Cliente> clientes = usuario.Cliente
                .Where(x => !updateUsuario.Cliente.Where(q => q.Value == x.Id).Any())
                .Where(x => x.Id != 0)
                .ToList();

                foreach (Cliente item in clientes)
                {
                    usuario.Cliente.Remove(item);
                }
            }

            //Remover Tipo de Fornecimento que foram retirados do relacionamento

            if (updateUsuario.TipoFornecimento != null)
            {
                List<TipoFornecimento> list = usuario.TipoFornecimento
                .Where(x => !updateUsuario.TipoFornecimento.Where(q => q.Value == x.Id).Any())
                .Where(x => x.Id != 0)
                .ToList();

                foreach (var item in list)
                {
                    usuario.TipoFornecimento.Remove(item);
                }
            }

            //Adicionar cliente caso tenha novas
            updateUsuario.Cliente?.ToList().ForEach(cli =>
            {
                if (usuario.Cliente.Where(p => p.Id == cli.Value).FirstOrDefault() == null)
                {
                    var cliente = _mapper.Map<Cliente>(cli);
                    _rm.ClienteRepository.Attach(cliente);
                    usuario.Cliente.Add(cliente);
                }
            });

            //Adicionar Tipo Fornecimento caso tenha novas
            updateUsuario.TipoFornecimento?.ToList().ForEach(tip =>
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


        private void AddComprasRelacoes(Usuario usuario, CreateUsuarioDto createUsuario)
        {

            if (createUsuario.Filial != null)
            {
                foreach (var item in createUsuario.Filial)
                {
                    var filial = _mapper.Map<Filial>(item);
                    _rm.FilialRepository.Attach(filial);
                    usuario.Filial.Add(filial);
                }
            }

            if (createUsuario.Cliente != null)
            {
                foreach (var item in createUsuario.Cliente)
                {
                    var cli = _mapper.Map<Cliente>(item);
                    _rm.ClienteRepository.Attach(cli);
                    usuario.Cliente.Add(cli);
                }
            }
            
            if (createUsuario.TipoFornecimento != null)
            {
                foreach (var item in createUsuario.TipoFornecimento)
                {
                    var categoria = _mapper.Map<TipoFornecimento>(item);
                    _rm.TipoFornecimentoRepository.Attach(categoria);
                    usuario.TipoFornecimento.Add(categoria);
                }
            }
        }
    }
}
