using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace XamarinFormsAzure
{
    public class BlobStorageService
    {
        public async Task<string> UploadImageAsync(Stream image)
        {
            var container = GetContainer();

            await container.CreateIfNotExistsAsync();

            var name = $"{Guid.NewGuid().ToString("D")}.jpg";

            var imageBlob = container.GetBlockBlobReference(name);
            await imageBlob.UploadFromStreamAsync(image);

            return name;
        }

        public async Task<byte[]> GetImageAsync(string name)
        {
            var container = GetContainer();

            var blob = container.GetBlobReference(name);

            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();

                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);

                return blobBytes;
            }

            return null;
        }

        private static CloudBlobContainer GetContainer()
        {
            var account = CloudStorageAccount.Parse(Constants.STORAGE_CONNECTION_STRING);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(Constants.STORAGE_IMAGE_BLOB);

            return container;
        }
    }
}
