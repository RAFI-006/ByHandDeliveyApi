using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Services
{
    public class AzureBlobService
    {

          public async Task<string> UploadImageAsync(IFormFile imageToUpload)
            {
                string imageFullPath = null;
                if (imageToUpload == null || imageToUpload.Length == 0)
                {
                    return null;
                }
                try
                {
                    CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("byhanddeliveyblobcontainer");

                    if (await cloudBlobContainer.CreateIfNotExistsAsync())
                    {
                        await cloudBlobContainer.SetPermissionsAsync(
                            new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            }
                            );
                    }
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageToUpload.FileName);

                imageFullPath = imageName;

                  CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                    cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;

                  for (int i=0;i<imageToUpload.Length;i++)
                {
                    using (var ms = new MemoryStream())
                    {
                        imageToUpload.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes,i,Convert.ToInt32(imageToUpload.Length));

                        // act on the Base64 data
                    }
                }

             
                    imageFullPath = cloudBlockBlob.Uri.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            return "https://byhanddeliveryblob.blob.core.windows.net/byhanddeliveyblobcontainer/" + imageFullPath;
            }
        }
   
}
