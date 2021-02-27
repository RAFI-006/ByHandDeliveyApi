using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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





        public async Task<string> FTPUpload(IFormFile data)
        {
            //FTP Server URL.
            string ftp = "ftp://byhanddelivery.com";

            //FTP Folder name. Leave blank if you want to upload to root folder.
            string ftpFolder = "/products/";
            byte[] dataBytes = null;
            byte[] fileBytes = null;

            //Read the FileName and convert it to Byte array.
            string fileName = Path.GetFileName(data.FileName);
            for (int i = 0; i < data.Length; i++)
            {
                using (var ms = new MemoryStream())
                {
                    data.CopyTo(ms);
                    dataBytes = ms.ToArray();
                   

                    // act on the Base64 data
                }
            }
            using (StreamReader fileStream = new StreamReader(new MemoryStream(dataBytes)))
            {
                fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
                fileStream.Close();
            }

            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential("hand", "Delivery1234!@");
                request.ContentLength = fileBytes.Length;
                request.UsePassive = true;
                request.UseBinary = true;
                request.ServicePoint.ConnectionLimit = fileBytes.Length;
                request.EnableSsl = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileBytes, 0, fileBytes.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

               // lblMessage.Text += fileName + " uploaded.<br />";
                response.Close();
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }

            return "https://app.byhanddelivery.com/"+ data.FileName;
        }

      





    }



  
}
