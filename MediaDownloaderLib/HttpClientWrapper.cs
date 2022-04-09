using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaDownloaderLib
{
    public interface IHttpClientWrapper
    {
        Task DownloadFileAsync(Uri uri, string outputPath);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task DownloadFileAsync(Uri uri, string outputPath)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentNullException(nameof(outputPath));
        
            var fileBytes = await HttpClient.GetByteArrayAsync(uri);
            await File.WriteAllBytesAsync(outputPath, fileBytes);
        }
    }
}