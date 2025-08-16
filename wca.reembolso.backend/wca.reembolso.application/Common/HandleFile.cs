namespace wca.reembolso.application.Common
{
    public class HandleFile
    {
        private readonly UploadArquivoHandle _uploadService;

        public HandleFile(UploadArquivoHandle uploadService)
        {
            _uploadService = uploadService;
        }

        public bool IsBase64(string dataFile)
        {
            return dataFile.Contains("data:");
        }

        public async Task DeleteFileAsync(string path)
        {
            string nomeArquivo = Path.GetFileName(new Uri(path).AbsolutePath);
            await _uploadService.Excluir(nomeArquivo);
        }

        public async Task<string> SaveFileAsync(string base64String, string nomeArquivo = "")
        {
            string arquivo = base64String.Split(",")[1];
            string extension = base64String.Split(',')[0].Split(';')[0].Split('/')[1];

            if (string.IsNullOrEmpty(nomeArquivo))
                nomeArquivo = Guid.NewGuid().ToString();

            byte[] bytes = Convert.FromBase64String(arquivo);
            using var stream = new MemoryStream(bytes);

            string urlDoArquivo = await _uploadService.Upload(stream, nomeArquivo);
            return urlDoArquivo;
        }
    }
}