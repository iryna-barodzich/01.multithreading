using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Threading;

namespace Server
{
    class Program
    {
        static Thread workThread;
        static string ServerQueueName = @".\Private$\server_task3";
        static string ServerDirectoryName = @"D:\Task3_Messaging_Received";
        static ManualResetEvent stopWorkEvent;

        static void Main(string[] args)
        {
            if (!MessageQueue.Exists(ServerQueueName))
            {
                MessageQueue.Create(ServerQueueName);
            }

            stopWorkEvent = new ManualResetEvent(false);

            workThread = new Thread(Server);
            workThread.Start();
        }

        private static void Server(object obj)
        {

            using (var serverQueue = new MessageQueue(ServerQueueName))
            {
                serverQueue.Formatter = new BinaryMessageFormatter();
                serverQueue.MessageReadPropertyFilter.SetAll();
                serverQueue.MessageReadPropertyFilter.IsFirstInTransaction = true;
                serverQueue.MessageReadPropertyFilter.IsLastInTransaction = true;
                List<byte> bytesList = new List<byte>();

                while (true)
                {
                    var asyncReceive = serverQueue.BeginPeek();

                    var res = WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, asyncReceive.AsyncWaitHandle });
                    if (res == 0)
                        break;

                    var message = serverQueue.EndPeek(asyncReceive);
                    serverQueue.ReceiveById(message.Id);
                    var labelParts = message.Label.Split('|');
                    var curentNumber = int.Parse(labelParts[1]);
                    var numberOfParts = int.Parse(labelParts[2]);

                    bytesList.AddRange((byte[])message.Body);
                    if (curentNumber == numberOfParts)
                    {
                        var filename = labelParts[0].Split('\\').Last();
                        File.WriteAllBytes(Path.Combine(ServerDirectoryName, filename), bytesList.ToArray());
                        bytesList = new List<byte>();
                        Console.WriteLine($"Server received file {filename} and save to directory {ServerDirectoryName}");
                    }
                }
            }

        }
    }
}
