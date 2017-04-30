using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Linq;
using System.Collections.Generic;

namespace XamarinFormsAzure
{
    public class QueueStorageService
    {
        public async Task<CloudQueue> CreateQueueAsync()
        {
            var queue = GetCloudQueue();
            try
            {
                await queue.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return queue;
        }

        public async Task AddMessageAsync(string message)
        {
            try
            {
                CloudQueue cloudQueue = await CreateQueueAsync();
                await cloudQueue.AddMessageAsync(new CloudQueueMessage(message));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public async Task<IEnumerable<string>> GetMessagesAsync()
        {
            try
            {
                CloudQueue cloudQueue = await CreateQueueAsync();
                var messages = new List<string>();
                bool run = true;
                while (run)
                {
                    var peekedMessage = await cloudQueue.PeekMessageAsync();
                    if (peekedMessage == null)
                    {
                        run = false;
                        continue;
                    }
                    var queue = await cloudQueue.GetMessageAsync();
                    messages.Add(queue.AsString);
                    await cloudQueue.DeleteMessageAsync(queue);
                }
                return messages;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return Enumerable.Empty<string>();
            }
        }

        private CloudQueue GetCloudQueue()
        {
            var account = CloudStorageAccount.Parse(Constants.STORAGE_CONNECTION_STRING);
            var client = account.CreateCloudQueueClient();

            CloudQueue container = client.GetQueueReference(Constants.STORAGE_QUEUE_NAME);

            return container;
        }
    }
}
