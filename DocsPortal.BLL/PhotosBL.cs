using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DocsPortal.BLL
{
    public class PhotosBL
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "photos";

        public PhotosBL(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            CreateContainerIfNotExistsAsync().GetAwaiter().GetResult();
        }

        private async Task CreateContainerIfNotExistsAsync()
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        }

        public async Task<string> UploadPhotoAsync(Stream content, string fileName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Create a unique blob name
            string blobName = $"{Guid.NewGuid()}-{Path.GetFileName(fileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file
            await blobClient.UploadAsync(content, overwrite: true);

            // Return the blob name and URI
            return new { Name = blobName, Uri = blobClient.Uri.ToString() }.ToString();
        }

        public async Task<(Stream Content, string ContentType)> DownloadPhotoAsync(string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Get properties to determine content type
            BlobProperties properties = await blobClient.GetPropertiesAsync();

            // Download the blob
            var download = await blobClient.DownloadContentAsync();
            return (download.Value.Content.ToStream(), properties.ContentType);
        }

        public async Task DeletePhotoAsync(string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Delete the blob
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> ListPhotosAsync()
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            List<string> blobNames = new();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                blobNames.Add(blobItem.Name);
            }

            return blobNames;
        }
    }
}
