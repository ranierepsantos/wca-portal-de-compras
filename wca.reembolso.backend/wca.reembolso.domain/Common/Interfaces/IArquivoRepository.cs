using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Common.Interfaces
{
    public interface IArquivoRepository
    {
        // O domínio precisa apenas da capacidade de salvar e retornar a URL do arquivo.
        // O 'stream' é um tipo genérico que representa o arquivo.
        Task<string> SalvarArquivoAsync(Stream arquivo, string nomeArquivo);

        // Adicione o método de exclusão. Ele precisa saber o nome do arquivo e o container.
        Task ExcluirArquivoAsync(string nomeArquivo);

        string GetTemporaryLink(string nomeArquivo);

        Task<Stream> GetFileStream(string nomeArquivo);

    }
}
