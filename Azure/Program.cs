using System;
using System.Configuration;
using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AzureQueues
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Queues...");
            var queue = new QueueConnection();
            //Console.WriteLine("Enter the name of the queue:");
            //var queueName = Console.ReadLine();
            var queueName = "ajv-queue1";
            Console.WriteLine("Enter the number of messages to insert into the queue");
            var total = int.Parse(Console.ReadLine());
            var messages = new List<string>();
            for (var i = 0; i < total; i++)
            {
                Console.Write($"Message[{i + 1}]: ");
                messages.Add(Console.ReadLine());
            }
            foreach (var message in messages)
            {
                queue.InsertMessage(queueName, message);
            }
            var queueLength = queue.GetQueueLength(queueName);
            Console.WriteLine($"QueueLength: {queueLength}");

            queue.PeekMessage(queueName);
            queue.UpdateMessage(queueName);
            queue.DequeueMessage(queueName);

            queueLength = queue.GetQueueLength(queueName);
            Console.WriteLine($"QueueLength: {queueLength}");

            queue.DequeueMessages(queueName, 2);
            queueLength = queue.GetQueueLength(queueName);
            Console.WriteLine($"QueueLength: {queueLength}");

            queue.ClearMessages(queueName);
            queue.DeleteQueue(queueName);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


    }
}
