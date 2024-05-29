using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public ClienteService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        public async Task<ClienteDto> Create(CreateClienteDto cliente)
        {
            try
            {
                var data = _mapper.Map<Cliente>(cliente);
                _rm.ClienteRepository.Attach(data);

                foreach(var item in cliente.ClienteContatos)
                {
                    var contato = _mapper.Map<ClienteContato>(item);
                    data.ClienteContatos.Add(contato);
                }

                foreach (var item in cliente.ClienteOrcamentoConfiguracao)
                {
                    var orcamentoConfiguracao = _mapper.Map<ClienteOrcamentoConfiguracao>(item);
                    data.ClienteOrcamentoConfiguracao.Add(orcamentoConfiguracao);
                }
                
                await _rm.SaveAsync(); 

                return _mapper.Map<ClienteDto>(data);
            }   
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ClienteDto>> GetAll()
        {
            try
            {
                IList<Cliente> clientes;
                var query = _rm.ClienteRepository.SelectAll();

                clientes = await query.OrderBy(c => c.Nome).ToArrayAsync();

                return _mapper.Map<IList<ClienteDto>>(clientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.GetAll.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ClienteDto> GetById(int id)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(p => p.Id == id);

                query = query.Include(cc => cc.ClienteContatos)
                             .Include(co => co.ClienteOrcamentoConfiguracao);

                var data = await query.FirstOrDefaultAsync();

                return _mapper.Map<ClienteDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
            
        }

        public async Task<IList<ListItem>> GetToList(int[] filiais)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(c => c.Ativo == true);

                if (filiais.Length > 0)
                    query = query.Where(q => filiais.Contains(q.FilialId));

                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<ClienteDto> Paginate(int[] filialId, int page = 1, int pageSize = 10, string termo = "")
        {
            try
            {
                var query = _rm.ClienteRepository.SelectAll();
                //Matriz (id: 1) retorna todos os dados
                if (filialId.Length> 0)
                {
                    query = query.Where(c => filialId.Contains(c.FilialId));
                }

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<ClienteDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Task<bool> Remove( int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClienteDto> Update(UpdateClienteDto cliente)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(p => p.Id == cliente.Id,false);

                query = query.Include(cc => cc.ClienteContatos)
                             .Include(co => co.ClienteOrcamentoConfiguracao);

                var baseData = await query.FirstOrDefaultAsync();
                
                if (baseData == null)
                {
                    return null;
                }

                //Remover contatos caso tenha alterado
                baseData.ClienteContatos.ToList().ForEach(contato =>
                {
                    var c = cliente.ClienteContatos.Where(p => p.Id == contato.Id).FirstOrDefault();
                    if (c == null)
                    {
                        var ct = _rm.ClienteContatoRepository.SelectByCondition(p => p.Id == contato.Id).FirstOrDefault();
                        if (ct != null) _rm.ClienteContatoRepository.Delete(ct);
                    }
                });

                //Remover configuração de orçamento caso tenha alterado
                baseData.ClienteOrcamentoConfiguracao.ToList().ForEach(config =>
                {
                    var c = cliente.ClienteOrcamentoConfiguracao.Where(p => p.Id == config.Id).FirstOrDefault();
                    if (c == null)
                    {
                        baseData.ClienteOrcamentoConfiguracao.Remove(config);
                    }
                });

                _mapper.Map(cliente, baseData);
                _rm.ClienteRepository.Update(baseData);
                
                await _rm.SaveAsync();

                return _mapper.Map<ClienteDto>(baseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ClienteDto>> GetByUser(int usuarioId, int[]? filial = null)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(c => c.Ativo)
                    .Include(c => c.ClienteOrcamentoConfiguracao)
                    .Where(c => c.Usuario.Any(c => c.Id == usuarioId));

                if (filial?.Length > 0)
                    query = query.Where(c => filial.Contains(c.FilialId));

                IList<Cliente> clientes = await query.ToListAsync();

                return _mapper.Map<IList<ClienteDto>>(clientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetByUser.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> ImportarDadosClientes(ClienteImportacaoDto clienteImportacaoDto)
        {
            try
            {
                // ler o arquivo
                byte[] arquivo = Convert.FromBase64String(clienteImportacaoDto.Arquivo);
                MemoryStream ms = new MemoryStream(arquivo);
                var sheets = MiniExcel.GetSheetNames(ms);
                string sheetName = "NORGE LABS";
                var rows = MiniExcel.Query(ms, sheetName: sheetName).Skip(3).ToList();

                IDictionary<string, object> categoryRow = rows[0];
                IDictionary<string, object> categorias = new Dictionary<string, object>();

                foreach (var key in categoryRow.Keys)
                {
                    if ((string.Compare(key, "J") > 0 || key.Length > 1) && categoryRow[key] != null)
                    {
                        categorias.Add(key, categoryRow[key]);
                        Console.WriteLine($"{key} : {categoryRow[key]?.ToString()}");

                        var tipo = await _rm.TipoFornecimentoRepository.SelectByCondition(q => 
                                    q.Nome.ToLower().Equals(categoryRow[key].ToString().ToLower())).FirstOrDefaultAsync();
                        if (tipo is null)
                        {
                            _rm.TipoFornecimentoRepository.Create(new TipoFornecimento() { Nome = categoryRow[key].ToString() });
                            await _rm.SaveAsync();
                        }
                            
                    }
                }

                var filiais = _rm.FilialRepository.SelectByCondition(q =>  q.SistemaId == 1).ToList();
                var tipos = _rm.TipoFornecimentoRepository.SelectAll().ToList();


                string filialNome = "";
                for (int idx = 2; idx < rows.Count; idx++)
                {
                    IDictionary<string, object> row = rows[idx];
                    filialNome = row["E"]?.ToString() ?? "";

                    string cep = row["H"]?.ToString();
                    cep = cep?.Replace("s/n", "");
                    if (cep?.Length > 9) cep = cep.Substring(0, 9);

                    var clienteFilial = filiais.Where(q => q.Nome.ToLower().Equals(filialNome.ToLower())).FirstOrDefault();
                    if (clienteFilial ==null)
                    {
                        throw new Exception($"Filial {filialNome}, não cadastrada");
                    }
                    CreateClienteDto createCliente = new CreateClienteDto(
                        Nome: row["B"]?.ToString(),
                        CNPJ: row["C"]?.ToString(),
                        InscricaoEstadual: row["D"]?.ToString(),
                        Endereco: row["F"]?.ToString(),
                        Numero: row["G"]?.ToString(),
                        CEP: cep,
                        Cidade: row["I"]?.ToString(),
                        UF: row["J"]?.ToString(),
                        Ativo: true,
                        PeriodoEntrega: "",
                        NaoUltrapassarLimitePorRequisicao: false,
                        ValorLimiteRequisicao: 0,
                        FilialId: clienteFilial.Id,
                        ClienteContatos: new List<ClienteContatoDto>(),
                        ClienteOrcamentoConfiguracao: new List<ClienteOrcamentoConfiguracaoDto>()
                    );

                    var teste = await _rm.ClienteRepository.SelectByCondition(q => q.Nome.ToLower().Equals(createCliente.Nome) && q.CNPJ.Equals(createCliente.CNPJ)).FirstOrDefaultAsync();
                    if (teste is null) { 
                        // adicionar as configurações de orçamento
                        foreach (string key in categorias.Keys)
                        {
                            (int codigo, int codigo1) = (0, 0);

                            if (key.Length > 1)
                            {
                                codigo = (int)char.Parse(key.Substring(0, 1));
                            }
                            codigo1 = (int)(key.Length == 1 ? char.Parse(key.Substring(0, 1)) : char.Parse(key.Substring(1, 1)));

                            string chave = key;
                            
                            ClienteOrcamentoConfiguracao configuracao = new ClienteOrcamentoConfiguracao();

                            configuracao.TipoFornecimentoId = tipos.Where(q => q.Nome.ToUpper() == categorias[chave].ToString().ToUpper()).First().Id;

                            (codigo, codigo1, chave) = retornaChave(codigo, codigo1, true);
                            configuracao.ValorPedido = decimal.Parse(row[chave]?.ToString());

                            (codigo, codigo1, chave) = retornaChave(codigo, codigo1);
                            configuracao.QuantidadeMes = int.Parse(row[chave]?.ToString());

                            (codigo, codigo1, chave) = retornaChave(codigo, codigo1);
                            configuracao.Tolerancia = decimal.Parse(row[chave]?.ToString());

                            (codigo, codigo1, chave) = retornaChave(codigo, codigo1);
                            configuracao.AprovadoPor = EnumAprovadoPor.WCA;
                            if (row[chave].ToString().Equals("CLIENTE"))
                            {
                                configuracao.AprovadoPor = EnumAprovadoPor.Cliente;
                                (codigo, codigo1, chave) = retornaChave(codigo, codigo1);
                                var email = row[chave].ToString();
                                if (createCliente.ClienteContatos.Where(q => q.Email.Equals(email)).FirstOrDefault() is null)
                                {
                                    var contato = new ClienteContato()
                                    {
                                        Nome = email.Split("@")[0],
                                        Email = email,
                                        AprovaPedido = true,
                                    };
                                    createCliente.ClienteContatos.Add(_mapper.Map<ClienteContatoDto>(contato));
                                }
                            }
                            configuracao.Ativo = true;
                            createCliente.ClienteOrcamentoConfiguracao.Add(_mapper.Map<ClienteOrcamentoConfiguracaoDto>(configuracao));
                        }
                        await Create(createCliente);
                    }
                }
                ms.Close();
                ms.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.ImportarDadosCliente.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private (int, int, string)  retornaChave(int c1, int c2, bool firstTime = false)
        {
            if (!firstTime)
            {
                c2++;
                if (c2 > 90)
                {
                    c2 = 65;
                    if (c1 > 0) c1++;
                    else c1 = 65;
                }
            }
            
            string chave = (c1 > 0 ? ((char)c1).ToString() : "") + ((char)c2).ToString();
            return (c1, c2, chave);
        }
    }
}
