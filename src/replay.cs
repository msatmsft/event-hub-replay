using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.WindowsAzure.Storage;
namespace SampleSender
{
    public static class replay
    {
        public static async Task Replay()
        {
            var connectionString = "Your Event Hub Connection String";
            var containerName = "Storage Container Name";
            var blobName = "Blob name ending with .avro";

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);
            blob.StreamMinimumReadSizeInBytes = 10 * 1024 * 1024;

            using (var stream = await blob.OpenReadAsync())                       
            using (var reader = AvroContainer.CreateGenericReader(stream))
            {
                while (reader.MoveNext())
                {
                    foreach (dynamic result in reader.Current.Objects)
                    {
                        var record = new AvroEventData(result);
                        Console.WriteLine(System.Text.Encoding.UTF8.GetString(record.Body));
                    }
                }
            }
            Console.Read();
        }

        public struct AvroEventData
        {
            public AvroEventData(dynamic record)
            {
                SequenceNumber = (long)record.SequenceNumber;
                Offset = (string)record.Offset;
                DateTime.TryParse((string)record.EnqueuedTimeUtc, out var enqueuedTimeUtc);
                EnqueuedTimeUtc = enqueuedTimeUtc;
                SystemProperties = (Dictionary<string, object>)record.SystemProperties;
                Properties = (Dictionary<string, object>)record.Properties;
                Body = (byte[])record.Body;
            }
            public long SequenceNumber { get; set; }
            public string Offset { get; set; }
            public DateTime EnqueuedTimeUtc { get; set; }
            public Dictionary<string, object> SystemProperties { get; set; }
            public Dictionary<string, object> Properties { get; set; }
            public byte[] Body { get; set; }
        }
    }
}

