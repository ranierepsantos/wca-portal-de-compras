using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.domain.Common.Interfaces;

namespace wca.reembolso.infrastruture.Integration.Azure
{
    public class AzureBlobStorageRepository : IArquivoRepository
    {
        private readonly string _connectionString;
        private readonly string _container;
        private readonly ILogger<AzureBlobStorageRepository> _logger;
        public AzureBlobStorageRepository(IConfiguration configuration, ILogger<AzureBlobStorageRepository> logger)
        {
            _connectionString = configuration["AzureStorage:connectionString"] ?? "";
            _container = configuration["AzureStorage:container"] ?? "";
            _logger = logger;
        }

        public async Task<string> SalvarArquivoAsync(Stream arquivo, string nomeArquivo)
        {
            // Este é o código de integração com o Azure Blob Storage!
            // Ele vive apenas aqui, na camada de Infraestrutura.

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_container);
            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(nomeArquivo);
            await blobClient.UploadAsync(arquivo, overwrite: true);

            return blobClient.Uri.ToString(); // Retorna a URL do arquivo
        }

        // A nova implementação para exclusão.
        public async Task ExcluirArquivoAsync(string nomeArquivo)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_container);
                var blobClient = containerClient.GetBlobClient(nomeArquivo);

                // Verifica se o blob existe antes de tentar excluir (opcional)
                if (await blobClient.ExistsAsync())
                {
                    await blobClient.DeleteAsync();
                    _logger.LogInformation("Arquivo '{NomeArquivo}' excluído do container '{Container}'.", nomeArquivo, _container);
                }
                else
                {
                    _logger.LogWarning("Arquivo '{NomeArquivo}' não encontrado no container '{Container}'.", nomeArquivo, _container);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir arquivo '{NomeArquivo}' do container '{Container}'.", nomeArquivo, _container);
                throw;
            }
        }

    }
}
