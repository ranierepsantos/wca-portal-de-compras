using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.domain.Common.Interfaces;

namespace wca.reembolso.application.Common
{
    // Em seu projeto de Aplicação (por exemplo, SeuApp.Application)

    public class UploadArquivoHandle
    {
        private readonly IArquivoRepository _arquivoRepository;

        // A dependência é a interface, não a classe concreta do Azure
        public UploadArquivoHandle(IArquivoRepository arquivoRepository)
        {
            _arquivoRepository = arquivoRepository;
        }

        public async Task<string> Upload(Stream arquivo, string nomeArquivo)
        {
            // Aqui, você pode ter alguma regra de negócio, se necessário.
            // Por exemplo, validar o tamanho do arquivo ou o tipo.
            // A lógica de upload em si é delegada.

            string urlDoArquivo = await _arquivoRepository.SalvarArquivoAsync(arquivo, nomeArquivo);

            return urlDoArquivo;
        }

        public async Task Excluir(string url)
        {
            await _arquivoRepository.ExcluirArquivoAsync(url);
        }
    }
}
