using AutoMapper;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
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
                var query = _rm.ProdutoRepository.SelectByCondition(p => p.Id == id && p.FornecedorId == fornecedorId)
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

        public async Task<IList<ListItem>> GetToList(int filialId)
        {
            try
            {
                var query = _rm.FornecedorRepository.SelectByCondition(c => c.Ativo == true);

                if (filialId > 0)
                {
                    query = query.Where(c => c.FilialId == filialId);
                }

                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
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

                var produtos = await _rm.ProdutoRepository
                            .SelectByCondition(c => c.FornecedorId.Equals(importProdutoDto.FornecedorId))
                            .ToListAsync();

                foreach (var produto in produtos)
                {
                    _rm.ProdutoRepository.Delete(produto);
                }

                var categorias = await _rm.TipoFornecimentoRepository.SelectAll().ToListAsync();

                // ler o arquivo
                byte[] arquivo = Convert.FromBase64String(importProdutoDto.Arquivo);
                MemoryStream ms = new MemoryStream(arquivo);
                var sheets = MiniExcel.GetSheetNames(ms);
                for (var idx = 0; idx < sheets.Count; idx++)
                {
                    var rows = MiniExcel.Query(ms, sheetName: sheets[idx]).Skip(1).ToList();
                    for (var index = 0; index < rows.Count; index++)
                    {
                        var produto = new Produto
                        {
                            Codigo = rows[index].A.ToString(),
                            FornecedorId = importProdutoDto.FornecedorId,
                            Nome = rows[index].B,
                            TipoFornecimentoId = categorias.FirstOrDefault(c => c.Nome.Contains(rows[index].C)).Id,
                            UnidadeMedida = rows[index].D,
                            Valor = (decimal)rows[index].E,
                            TaxaGestao = (decimal)rows[index].F,
                            PercentualIPI = (decimal)rows[index].G
                        };

                        _rm.ProdutoRepository.Create(produto);
                    }
                }
                await _rm.SaveAsync();
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
                var query = _rm.ProdutoRepository.SelectByCondition(c => c.FornecedorId == fornecedorId)
                    .Include("Fornecedor");
                //Matriz (id: 1) retorna todos os dados
                if (filialId > 1)
                {
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

                var row = 2;

                foreach (var item in produtos)
                {
                    ws.Cell($"A{row}").SetValue(item.Codigo);
                    ws.Cell($"B{row}").SetValue(item.Nome);
                    ws.Cell($"C{row}").SetValue(item.TipoFornecimento.Nome);
                    ws.Cell($"D{row}").SetValue(item.UnidadeMedida);
                    ws.Cell($"E{row}").SetValue(item.Valor);
                    ws.Cell($"F{row}").SetValue(item.TaxaGestao);
                    ws.Cell($"G{row}").SetValue(item.PercentualIPI);
                    row++;
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
