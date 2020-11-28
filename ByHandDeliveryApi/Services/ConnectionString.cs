using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ByHandDeliveryApi.Services
{
    public static class ConnectionString
    {
        static string account = CloudConfigurationManager.GetSetting("StorageAccountName");
        static string key = CloudConfigurationManager.GetSetting("StorageAccountKey");
        public static CloudStorageAccount GetConnectionString()
        {
            //        string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=byhanddeliveryblob;AccountKey=iHTviJYlaEUpl/fVfD5SVIdzLJ/mERwI5c23WMoWZvE5rvZIA1U6Nxg2cKbtCpl+putFzS4HG1pdH+dJ51iB+w==;EndpointSuffix=core.windows.net";
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}
