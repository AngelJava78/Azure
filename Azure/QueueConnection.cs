using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Configuration;

namespace AzureQueues
{
    public class QueueConnection
    {
        QueueClient queueClient;
        private bool CreateQueue(string queueName)
        {
            var result = false;
            try
            {
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                queueClient = new QueueClient(connectionString, queueName);
                queueClient.CreateIfNotExists();
                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: {queueClient.Name}");
                    result = true;
                }
                else
                {
                    Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                result = false;
            }
            return result;
        }
        public void InsertMessage(string queueName, string message)
        {
            if (CreateQueue(queueName))
            {
                queueClient.SendMessage(message);
                Console.WriteLine($"Message inserted: {message}");
            }
        }
        public void PeekMessage(string queueName)
        {
            Console.WriteLine("Peek Message");
            if (CreateQueue(queueName))
            {
                PeekedMessage[] messages = queueClient.PeekMessages();
                for (var i = 0; i < messages.Length; i++)
                {
                    Console.WriteLine($"Message[{i}]:{messages[i].Body}");
                }
            }
        }
        public void UpdateMessage(string queueName)
        {
            Console.WriteLine("Update Message");
            if (CreateQueue(queueName))
            {
                QueueMessage[] messages = queueClient.ReceiveMessages();
                for (var i = 0; i < messages.Length; i++)
                {
                    var newBody = messages[i].Body + " updated on " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    queueClient.UpdateMessage(messages[i].MessageId, messages[i].PopReceipt, newBody, TimeSpan.FromSeconds(5));
                    Console.WriteLine($"Message updated. Id: {messages[i].MessageId}. Body: {messages[i].Body}. ");
                }
            }
        }

        public void DequeueMessage(string queueName)
        {
            Console.WriteLine("Dequeue Message");
            if (CreateQueue(queueName))
            {
                QueueMessage[] messages = queueClient.ReceiveMessages();
                for (var i = 0; i < messages.Length; i++)
                {
                    queueClient.DeleteMessage(messages[i].MessageId, messages[i].PopReceipt);
                    Console.WriteLine($"Dequeue message. Id: {messages[i].MessageId}. Body: {messages[i].Body}.");
                }
            }
        }

        public void DequeueMessages(string queueName, int total)
        {
            Console.WriteLine("Dequeue Messages");
            if (CreateQueue(queueName))
            {
                QueueMessage[] messages = queueClient.ReceiveMessages(total, TimeSpan.FromSeconds(10));
                for (var i = 0; i < messages.Length; i++)
                {
                    Console.WriteLine($"Dequeue message. Id: {messages[i].MessageId}. Body: {messages[i].Body}.");
                    queueClient.DeleteMessage(messages[i].MessageId, messages[i].PopReceipt);
                }
            }

        }
        public int GetQueueLength(string queueName)
        {
            var result = 0;
            Console.WriteLine("GetQueueLength");
            if (CreateQueue(queueName))
            {
                QueueProperties properties = queueClient.GetProperties();
                result = properties.ApproximateMessagesCount;
            }
            return result;
        }

        public void ClearMessages(string queueName)
        {
            Console.WriteLine("Clear messages");
            if (CreateQueue(queueName))
            {
                queueClient.ClearMessages();

            }
        }
        public void DeleteQueue(string queueName)
        {
            Console.WriteLine("Delete Queue");
            if (CreateQueue(queueName))
            {
                queueClient.Delete();

            }
        }
    }

}
