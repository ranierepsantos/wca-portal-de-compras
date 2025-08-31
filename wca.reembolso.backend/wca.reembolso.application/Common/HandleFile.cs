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
            string nomeArquivo = Path.GetFileName(new Uri(path).AbsolutePath);
            await _arquivoRepository.ExcluirArquivoAsync(nomeArquivo);
        }

        public async Task<string> SaveFileAsync(string base64String, string nomeArquivo = "")
        {
            string arquivo = base64String.Split(",")[1];
            string extension = base64String.Split(',')[0].Split(';')[0].Split('/')[1];

            if (string.IsNullOrEmpty(nomeArquivo))
            {
                nomeArquivo = Guid.NewGuid().ToString() + '.'+ extension;
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
    }
}