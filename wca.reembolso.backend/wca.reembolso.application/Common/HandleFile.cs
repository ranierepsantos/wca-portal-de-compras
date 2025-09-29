using wca.reembolso.domain.Common.dtos;
using wca.reembolso.domain.Common.Interfaces;

namespace wca.reembolso.application.Common
{
    public class HandleFile
    {
        private readonly IArquivoRepository _arquivoRepository;

        public HandleFile(IArquivoRepository arquivoRepository)
        {
            _arquivoRepository = arquivoRepository;
        }

        public bool IsBase64(string dataFile)
        {
            return dataFile.Contains("data:");
        }

        public async Task DeleteFileAsync(string path)
        {
            if (path.Contains(MyHttpContext.AppBaseUrl))
            {
                string fileToExclude = path.Replace(MyHttpContext.AppBaseUrl, "wwwroot");
                if (File.Exists(fileToExclude))
                {
                    File.Delete(fileToExclude);
                }
            }
            else
            {
                string nomeArquivo = Path.GetFileName(new Uri(path).AbsolutePath);
                await _arquivoRepository.ExcluirArquivoAsync(nomeArquivo);
            }
        }

        public async Task<string> SaveFileAsync(string base64String, string nomeArquivo = "")
        {
            string arquivo = base64String.Split(",")[1];
            string extension = base64String.Split(',')[0].Split(';')[0].Split('/')[1];

            if (string.IsNullOrEmpty(nomeArquivo))
            {
                nomeArquivo = Guid.NewGuid().ToString() + '.' + extension;
            }


            byte[] bytes = Convert.FromBase64String(arquivo);
            using var stream = new MemoryStream(bytes);

            string urlDoArquivo = await _arquivoRepository.SalvarArquivoAsync(stream, nomeArquivo);
            return urlDoArquivo;
        }

        public string GetTemporyLink(string path)
        {
            string nomeArquivo = Path.GetFileName(new Uri(path).AbsolutePath);
            string temporaryLink = _arquivoRepository.GetTemporaryLink(nomeArquivo);
            return temporaryLink;
        }


        public async Task<AzureFile> GetFile(string path)
        {
            if (path.Contains(MyHttpContext.AppBaseUrl))
            {
                string filePath = path.Replace(MyHttpContext.AppBaseUrl, "wwwroot");
                FileStream fileStream = new(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read
                );
                string mimeType = GetMimeType(filePath);
                return new AzureFile()
                {
                    Data = fileStream,
                    Name = Path.GetFileName(new Uri(path).AbsolutePath),
                    MimeType = mimeType
                };
            }
            else
            {
                string nomeArquivo = Path.GetFileName(new Uri(path).AbsolutePath);
                return await _arquivoRepository.GetFileStream(nomeArquivo);
            }

        }
        
        
        private string GetMimeType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            
            // Mapeia as extensões mais comuns para seus respectivos MIME types
            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                default:
                    // Se o tipo for desconhecido, use um MIME type genérico
                    return "application/octet-stream";
            }
        }
    }
}