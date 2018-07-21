using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neudesic.AzureAssignmentDownload
{
    class Program
    {
        const string storageAccountName = "dhanusha";
        const string storageAccountkey = "6kQpy3YlHakgKSXuClYLR+UQ/cM4vmKM9HnWwJnPmYBFLH8uIZKYufCgu+VMeviHo7s/1Ys6IUiH3+PABlqJDA==";
        static void Main(string[] args)
        {
            var storageAccount = new CloudStorageAccount(new StorageCredentials(storageAccountName, storageAccountkey), true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("azureassignment");
            var blobs = container.ListBlobs();
            DownloadBlobs(blobs);
            Console.WriteLine("completed");
        }
        private static void DownloadBlobs(IEnumerable<IListBlobItem> blobs)
        {
            foreach(var blob in blobs)
            {
                if(blob is CloudBlockBlob blockBob)
                {
                    blockBob.DownloadToFile(Path.Combine(@"C:\Azure",blockBob.Name), FileMode.Create);
                    Console.WriteLine(blockBob.Name);
                }
                else if(blob is CloudBlobDirectory blobDirectory)
                {
                    Directory.CreateDirectory(blobDirectory.Prefix);
                    Console.WriteLine("create Directory" + blobDirectory.Prefix);
                    DownloadBlobs(blobDirectory.ListBlobs());
                }
            }
        }
    }
}
