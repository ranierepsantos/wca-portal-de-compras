using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
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
            try
            {
                //var credential = new DefaultAzureCredential();
                // Crie um credential que pula o Azure CLI 
                //var credential = new ChainedTokenCredential(
                //    new EnvironmentCredential(),
                //    new ManagedIdentityCredential(),
                //    new VisualStudioCredential(),
                //    new VisualStudioCodeCredential()
                //);
                //var blobServiceClient = new BlobServiceClient(new Uri(_connectionString), credential);
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_container);
                await containerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = containerClient.GetBlobClient(nomeArquivo);
                await blobClient.UploadAsync(arquivo, overwrite: true);

                return blobClient.Uri.ToString(); // Retorna a URL do arquivo
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar arquivo '{NomeArquivo}' do container '{Container}'.", nomeArquivo, _container);
                throw;
            }
            
        }

        // A nova implementação para exclusão.
        public async Task ExcluirArquivoAsync(string nomeArquivo)
        {
            try
            {
                //var credential = new ChainedTokenCredential(
                //    new EnvironmentCredential(),
                //    new ManagedIdentityCredential(),
                //    new VisualStudioCredential(),
                //    new VisualStudioCodeCredential()
                //);
                //var blobServiceClient = new BlobServiceClient(new Uri(_connectionString), credential);
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

        public string GetTemporaryLink(string nomeArquivo)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_container);
                BlobClient blobClient = containerClient.GetBlobClient(nomeArquivo);

                if (!blobClient.Exists())
                {
                    _logger.LogWarning("Arquivo '{NomeArquivo}' não encontrado no container '{Container}'.", nomeArquivo, _container);
                    throw new Exception($"File not found in storage {nomeArquivo}, container {_container}.");
                }

                // Cria uma SAS que expira em 5 minutos e permite apenas leitura.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.BlobContainerName,
                    BlobName = blobClient.Name,
                    Resource = "b", // 'b' para blob
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

                return sasUri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar link '{NomeArquivo}' do container '{Container}'.", nomeArquivo, _container);
                throw;
            }
        }

        public async Task<Stream> GetFileStream(string nomeArquivo)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_container);
                BlobClient blobClient = containerClient.GetBlobClient(nomeArquivo);

                if (!blobClient.Exists())
                {
                    _logger.LogWarning("Arquivo '{NomeArquivo}' não encontrado no container '{Container}'.", nomeArquivo, _container);
                    throw new Exception($"File not found in storage {nomeArquivo}, container {_container}.");
                }

                var stream = await blobClient.OpenReadAsync();

                // Retorna o arquivo com o MIME type correto e dinâmico
                return stream;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao retornar '{NomeArquivo}' do container '{Container}'.", nomeArquivo, _container);
                throw;
            }
        }


    }
}
