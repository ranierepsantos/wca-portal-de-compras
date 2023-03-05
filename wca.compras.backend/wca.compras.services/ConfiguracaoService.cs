using Microsoft.EntityFrameworkCore;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;

namespace wca.compras.services
{
    public class ConfiguracaoService : IConfiguracaoService
    {
        private readonly IRepositoryManager _rm;

        public ConfiguracaoService(IRepositoryManager rm)
        {
            _rm = rm;
        }

        public async Task<IList<Configuracao>> GetAll()
        {
            try
            {
                IList<Configuracao> configuracoes;
                var query = _rm.ConfiguracaoRepository.SelectAll();

                configuracoes = await query.OrderBy(c => c.TipoCampo).ToListAsync();

                return configuracoes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetAll.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool?> Update(UpdateConfiguracaoDto configuracao)
        {
            try
            {
                var query = _rm.ConfiguracaoRepository.SelectByCondition(c => c.Id == configuracao.Id);

                Configuracao? data = await query.FirstOrDefaultAsync();
                if (data == null) { return null; }

                data.Valor= configuracao.Valor;

                _rm.ConfiguracaoRepository.Update(data);

                await _rm.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
