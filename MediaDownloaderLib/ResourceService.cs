using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaDownloaderLib
{
    public interface IResourceService
    {
        Task DownloadResourceAsync(Uri? resourceUri, string? destinationPath);

        Task<string> GetResourceStringAsync(Uri? resourceUri);
    }

    public class ResourceService : IResourceService
    {
        public async Task DownloadResourceAsync(Uri? resourceUri, string? destinationPath)
        {
            if (resourceUri == null) 
                throw new ArgumentNullException(nameof(resourceUri));
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentNullException(nameof(destinationPath));
        
            using var httpClient = new HttpClient();
            await using var stream = await httpClient.GetStreamAsync(resourceUri);
            await using var fileStream = new FileStream(destinationPath, FileMode.CreateNew);
            await stream.CopyToAsync(fileStream);
        }
    
        public async Task<string> GetResourceStringAsync(Uri? resourceUri)
        {
            if (resourceUri == null) 
                throw new ArgumentNullException(nameof(resourceUri));

            using var httpClient = new HttpClient();
            return await httpClient
                .GetStringAsync(resourceUri)
                .ConfigureAwait(false);
        }
    }
}