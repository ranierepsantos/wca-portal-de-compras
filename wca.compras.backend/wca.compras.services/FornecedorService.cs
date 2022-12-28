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
                _rm.FornecedorRepository.Create(data);
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

        public void ImportProdutoFromExcel(int filialId, int fornecedorId, ImportProdutoDto importProdutoDto)
        {
            byte[] data = Convert.FromBase64String(importProdutoDto.Arquivo); ;
            
            // salvar o arquivo
            MemoryStream ms = new MemoryStream(data);

            var sheets = MiniExcel.GetSheetNames(ms);

            for (var index = 0; index < sheets.Count; index++)
            {
                Console.WriteLine("SheetName: " + sheets[index]);
            }

            var rows = MiniExcel.Query(ms, sheetName: sheets[0]).Skip(2).ToList();

            for (var index = 0; index < rows.Count; index++)
            {
                Console.WriteLine($"Row {index}: {rows[index].B}, {rows[index].C}");
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
                var query = _rm.FornecedorRepository.SelectByCondition(p => p.Id == updateFornecedorDto.Id,false);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                var baseData = await query.FirstOrDefaultAsync();
                
                if (baseData == null)
                {
                    return null;
                }
                
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
    }
}
