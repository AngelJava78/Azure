using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TopicSender
{
    class Program
    {


        public static async Task Main(string[] args)
        {
            var connectionString = "Endpoint=sb://servicebusajv.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XeKFtxeKTpqLvJDW4T2rYjHnESuYNZL9PRfRMmTa2LU=";
            var topicName = "myservicebustheme";
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(topicName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            var products = new List<Product>
            {
                new Product { Id = 11, Name="TRE", Operation="New"},
                new Product { Id = 12, Name ="TVDE", Operation ="Get"},
                new Product { Id = 14, Name ="Vestimenta", Operation="Update"},
                new Product { Id = 18, Name="Regalo", Operation="Delete"}
            };

            foreach (var product in products)
            {

                var newMessage = JsonSerializer.Serialize(product);
                var message = new ServiceBusMessage(newMessage)
                {
                    ContentType = "application/json",
                };
                Console.WriteLine(newMessage);
                if (!messageBatch.TryAddMessage(message))
                {
                    throw new Exception($"The message {newMessage} is too large to fit in the batch.");
                }
            }

            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {products.Count} messages has been published to the topic.");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
