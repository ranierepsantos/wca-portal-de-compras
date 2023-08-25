using Microsoft.Extensions.Configuration;

namespace wca.reembolso.application.Common
{
    public class HandleFile
    {
        private static IConfiguration _configuration;

        internal static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static bool IsBase64Image(string image)
        {
            return image.Contains("data:image");
        }

        public static void DeleteFile(string path)
        {
            string fileToExclude = path.Replace(MyHttpContext.AppBaseUrl, "wwwroot");
            if (File.Exists(fileToExclude))
            {
                File.Delete(fileToExclude);
            }
        }
        public static string SaveImage(string base64String)
        {

            string image = base64String.Split(",")[1];
            string extension = base64String.Split(',')[0].Split(';')[0].Split('/')[1];

            byte[] bytes = Convert.FromBase64String(image);
            string nomeArquivo = Guid.NewGuid().ToString();
            string dirPath = _configuration.GetSection("MediaDirectory").Value?? "Media";
            
            if (!Directory.Exists($"wwwroot//{dirPath}"))
            {
                Directory.CreateDirectory($"wwwroot//{dirPath}");
            }
            string filePath = $"wwwroot//{dirPath}//{nomeArquivo}.{extension}";
            File.WriteAllBytes(filePath, bytes);
            return  MyHttpContext.AppBaseUrl + $"//{dirPath}//{nomeArquivo}.{extension}";
        }
    }
}
