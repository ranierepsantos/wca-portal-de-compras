using AutoMapper;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniExcelLibs;
using System.Linq;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public FornecedorService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        public async Task<FornecedorDto> Create(CreateFornecedorDto createFornecedorDto)
        {
            try
            {
                var data = _mapper.Map<Fornecedor>(createFornecedorDto);
                _rm.FornecedorRepository.Attach(data);

                foreach (var item in createFornecedorDto.FornecedorContatos)
                {
                    var contato = _mapper.Map<FornecedorContato>(item);
                    data.FornecedorContatos.Add(contato);
                }

                await _rm.SaveAsync(); 

                return _mapper.Map<FornecedorDto>(data);
            }   
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ProdutoDto> CreateProduto(CreateProdutoDto createProdutoDto)
        {
            try
            {
                var data = _mapper.Map<Produto>(createProdutoDto);
                _rm.ProdutoRepository.Create(data);
                await _rm.SaveAsync();

                return _mapper.Map<ProdutoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.CreateProduto.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<FornecedorDto> GetById(int filialId, int id)
        {
            try
            {
                var query = _rm.FornecedorRepository.SelectByCondition(p => p.Id == id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include("FornecedorContatos");

                var data = await query.FirstOrDefaultAsync();

                return _mapper.Map<FornecedorDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
            
        }

        public async Task<ProdutoDto> GetProdutoById(int filialId, int fornecedorId, int id)
        {
            try
            {
                var query = _rm.ProdutoRepository
                    .SelectByCondition(p => p.Id == id && p.FornecedorId == fornecedorId)
                    .Include(p => p.ProdutoIcmsEstado)
                    .Include("Fornecedor");

                if (filialId > 1)
                    query = query.Where(c => c.Fornecedor.FilialId == filialId);
                
                var data = await query.FirstOrDefaultAsync();

                return _mapper.Map<ProdutoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.GetProdutoById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<FornecedorListDto>> GetToList(int filialId)
        {
            try
            {
                var query = _rm.FornecedorRepository.SelectByCondition(c => c.Ativo == true);

                if (filialId > 0)
                {
                    query = query.Where(c => c.FilialId == filialId);
                }

                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<FornecedorListDto>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> ImportProdutoFromExcel(int filialId, int fornecedorId, ImportProdutoDto importProdutoDto)
        {
            try
            {
                //checar se o usuario pode alterar dados do fornecedor
                if ((await GetById(filialId, fornecedorId)) == null)
                {
                    return false;
                }

                //EXCLUIR A RELAÇÃO PRODUTO X ICMS X ESTADO dos produto do fornecedor
                string command = "DELETE pi FROM produto_icms_estado pi " 
                               + "INNER JOIN produtos po ON po.id = pi.produto_id "
                               + $"WHERE po.fornecedor_id = {fornecedorId}";

                await _rm.ExecuteCommandAsync(command);

                //EXCLUIR TODOS OS PRODUTOS DO FORNECEDOR
                command = $"DELETE FROM produtos WHERE fornecedor_id = {fornecedorId}";
                
                await _rm.ExecuteCommandAsync(command);


                //TRAZER AS CATEGORIAS
                var categorias = await _rm.TipoFornecimentoRepository.SelectAll().ToListAsync();

                // ler o arquivo
                byte[] arquivo = Convert.FromBase64String(importProdutoDto.Arquivo);
                MemoryStream ms = new MemoryStream(arquivo);
                var sheets = MiniExcel.GetSheetNames(ms);
                string codigoProduto = string.Empty;

                Produto  produto = new Produto();

                for (var idx = 0; idx < sheets.Count; idx++)
                {
                    var rows = MiniExcel.Query(ms, sheetName: sheets[idx]).Skip(1).ToList();
                    
                    for (var index = 0; index < rows.Count; index++)
                    {

                        if (codigoProduto != rows[index].A.ToString())
                        {
                            produto = new Produto
                            {
                                Codigo = rows[index].A.ToString(),
                                FornecedorId = importProdutoDto.FornecedorId,
                                Nome = rows[index].B,
                                TipoFornecimentoId = categorias.FirstOrDefault(c => c.Nome.Contains(rows[index].C)).Id,
                                UnidadeMedida = rows[index].D,
                                Valor = (decimal)rows[index].E,
                                TaxaGestao = (decimal)rows[index].F,
                                PercentualIPI = (decimal)rows[index].G,
                            };
                            codigoProduto = produto.Codigo;
                            _rm.ProdutoRepository.Attach(produto);
                        }

                        produto.ProdutoIcmsEstado.Add(new ProdutoIcmsEstado() {
                            UF = rows[index].H,
                            Icms = (decimal)rows[index].I
                        });
                    }
                    await _rm.SaveAsync();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.ProdutosImportarFromArquivo.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<FornecedorDto> Paginate(int filialId, int page = 1, int pageSize = 10, string termo = "")
        {
            try
            {
                var query = _rm.FornecedorRepository.SelectAll();
                //Matriz (id: 1) retorna todos os dados
                if (filialId > 1)
                {
                    query = query.Where(c => c.FilialId ==filialId);
                }

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                query = query.Include("FornecedorContatos");

                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<FornecedorDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<ProdutoDto> Paginate(int filialId, int fornecedorId, int page = 1, int pageSize = 10, string termo = "")
        {
            try
            {
                var query = _rm.ProdutoRepository.SelectByCondition(c => c.FornecedorId == fornecedorId);
                
                //Matriz (id: 1) retorna todos os dados
                if (filialId > 1)
                {
                    query = query.Include("Fornecedor");
                    query = query.Where(c => c.Fornecedor.FilialId == filialId);
                }

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo) || q.Codigo.Contains(termo));
                }
                
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<ProdutoDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.PaginateProduto.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ProdutoWithIcmsDto>> ListProdutoByFornecedorWithIcms(int filialId, int fornecedorId, string uf, int usuarioId, string? termo = "")
        {
            try
            {
                var query = _rm.ProdutoRepository.SelectByCondition(c => c.FornecedorId == fornecedorId);
                query = query.Include(p => p.ProdutoIcmsEstado.Where(c => c.UF.Equals(uf.ToUpper())));


                //Matriz (id: 1) retorna todos os dados
                if (filialId > 1)
                {
                    query = query.Include("Fornecedor");
                    query = query.Where(c => c.Fornecedor.FilialId == filialId);
                }

                //Filtrar somente produtos das categorias relacionadas ao usuário
                query = query.Include(n => n.TipoFornecimento)
                    .ThenInclude(n => n.Usuario)
                    .Where(c => c.TipoFornecimento.Ativo && c.TipoFornecimento.Usuario.Any(c => c.Id == usuarioId));

                //Filtrar se for informado o nome ou código do produto
                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo) || q.Codigo.Contains(termo));
                }

                query = query.OrderBy(p => p.Nome);

                var queryResult = (await query.ToListAsync()).Select(p => new ProdutoWithIcmsDto(
                        p.Id,
                        p.Codigo,
                        p.Nome,
                        p.UnidadeMedida,
                        p.Valor,
                        p.TaxaGestao,
                        p.PercentualIPI,
                        p.FornecedorId ?? 0,
                        p.TipoFornecimentoId,
                        p.ProdutoIcmsEstado.Count == 0 ? 0 : p.ProdutoIcmsEstado[0].Icms)
                    ).ToList();

                return queryResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.ListProdutoByFornecedorWithIcms.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public Task<bool> Remove(int filialId, int id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> RemoveProduto(int filialId, int fornecedorId, int id)
        {
            try
            {
                var query = _rm.ProdutoRepository
                    .SelectByCondition(c => c.Id == id && c.FornecedorId == fornecedorId)
                    .Include("Fornecedor");

                if (filialId > 1)
                {
                    query = query.Where(c => c.Fornecedor.FilialId == filialId);
                }

                var baseData = await query.FirstOrDefaultAsync();

                if (baseData == null)
                {
                    return false;
                }
                _rm.ProdutoRepository.Delete(baseData);

                await _rm.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FornecedorService.RemoveProduto.Error" + ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<FornecedorDto> Update(int filialId, UpdateFornecedorDto updateFornecedorDto)
        {
            try
            {
                var query = _rm.FornecedorRepository.SelectByCondition(p => p.Id == updateFornecedorDto.Id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                var baseData = await query.FirstOrDefaultAsync();
                
                if (baseData == null)
                {
                    return null;
                }
                //Remover contatos caso tenha alterado
                baseData.FornecedorContatos.ToList().ForEach(contato =>
                {
                    var c = updateFornecedorDto.FornecedorContatos.Where(p => p.Id == contato.Id).FirstOrDefault();
                    if (c == null)
                    {
                        var ct = _rm.FornecedorContatoRepository.SelectByCondition(p => p.Id == contato.Id).FirstOrDefault();
                        if (ct != null) _rm.FornecedorContatoRepository.Delete(ct);
                    }
                });

                _mapper.Map(updateFornecedorDto, baseData);
                _rm.FornecedorRepository.Update(baseData);
                
                await _rm.SaveAsync();

                return _mapper.Map<FornecedorDto>(baseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ProdutoDto> UpdateProduto(int filialId, UpdateProdutoDto updateProdutoDto)
        {
            try
            {
                var query = _rm.ProdutoRepository.SelectByCondition(p => p.Id == updateProdutoDto.Id, false)
                    .Include("Fornecedor");

                if (filialId > 1)
                    query = query.Where(c => c.Fornecedor.FilialId == filialId);

                var baseData = await query.FirstOrDefaultAsync();

                if (baseData == null)
                {
                    return null;
                }

                _mapper.Map(updateProdutoDto, baseData);
                _rm.ProdutoRepository.Update(baseData);

                await _rm.SaveAsync();

                return _mapper.Map<ProdutoDto>(baseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FornecedorService.UpdateProduto.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<Stream?> ProdutosExportToExcel(int fornecedorId)
        {

            try
            {
                List<Produto> produtos = await _rm.ProdutoRepository.SelectByCondition(c => c.FornecedorId == fornecedorId)
                                         .Include(p => p.TipoFornecimento)
                                         .Include(p => p.ProdutoIcmsEstado)
                                         .OrderBy(o =>  o.Nome) 
                                         .ToListAsync();

                if (produtos.Count == 0) return null;


                var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("Produtos");
                ws.Cell("A1").SetValue("CÓDIGO");
                ws.Cell("B1").SetValue("NOME");
                ws.Cell("C1").SetValue("CATEGORIA");
                ws.Cell("D1").SetValue("U.M");
                ws.Cell("E1").SetValue("VALOR");
                ws.Cell("F1").SetValue("TAXA GESTÃO");
                ws.Cell("G1").SetValue("IPI (%)");
                ws.Cell("H1").SetValue("UF");
                ws.Cell("I1").SetValue("ICMS (%)");
                
                var row = 2;

                foreach (var item in produtos)
                {
                    foreach(var icms in item.ProdutoIcmsEstado)
                    {
                        ws.Cell($"A{row}").SetValue(item.Codigo);
                        ws.Cell($"B{row}").SetValue(item.Nome);
                        ws.Cell($"C{row}").SetValue(item.TipoFornecimento.Nome);
                        ws.Cell($"D{row}").SetValue(item.UnidadeMedida);
                        ws.Cell($"E{row}").SetValue(item.Valor);
                        ws.Cell($"F{row}").SetValue(item.TaxaGestao);
                        ws.Cell($"G{row}").SetValue(item.PercentualIPI);
                        ws.Cell($"H{row}").SetValue(icms.UF);
                        ws.Cell($"I{row}").SetValue(icms.Icms);
                        ws.Cell($"J{row}").SetValue("Ok");
                        row++;
                    }
                    
                }
                Stream spreadsheetStream = new MemoryStream();
                workbook.SaveAs(spreadsheetStream);
                spreadsheetStream.Position = 0;

                return spreadsheetStream;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.ProdutosExportToExcel.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }

        }
    }
}
