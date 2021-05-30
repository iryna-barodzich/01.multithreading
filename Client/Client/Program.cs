using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static Thread workThread;
        static string ServerQueueName = @".\Private$\server_task3";
        static string WatcherDirectoryName = @"D:\Task3_Messaging";
        static void Main()
        {
            using (var watcher = new FileSystemWatcher(WatcherDirectoryName))
            {
                watcher.NotifyFilter = NotifyFilters.Attributes
                     | NotifyFilters.CreationTime
                     | NotifyFilters.DirectoryName
                     | NotifyFilters.FileName
                     | NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.Security
                     | NotifyFilters.Size;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Renamed += OnRenamed;

                watcher.Filter = "*.txt";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static void NotifyServer(string filepath)
        {
            workThread = new Thread(Client);
            workThread.Start(filepath);
        }

        public static byte[] FileToByteArray(string fileName)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            return bytes;
        }

        private static void Client(object obj)
        {
            using (var clientQueue = new MessageQueue(ServerQueueName, QueueAccessMode.Send))
            {
                var filename = obj.ToString();
                var bytes = FileToByteArray(obj.ToString());
                var chunkCount = 3;
                var size = bytes.Length;

                var bufferArray = new byte[chunkCount][];

                for (var i = 0; i < chunkCount; i++)
                {
                    bufferArray[i] = new byte[1000];
                    for (var j = 0; j < 1000 && i * chunkCount + j < size; j++)
                    {
                        bufferArray[i][j] = bytes[i * chunkCount + j];
                    }
                }

                var transaction = new MessageQueueTransaction();

                try
                {
                    transaction.Begin();
                    foreach(var part in bufferArray)
                    {
                        Message message = new Message(part, new BinaryMessageFormatter());
                        message.Label = filename;
                        clientQueue.Send(message);
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Abort();
                }


            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed file: {e.FullPath}");
            NotifyServer(e.FullPath);
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created file: {e.FullPath}";
            Console.WriteLine(value);
            NotifyServer(e.FullPath);
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            NotifyServer(e.FullPath);
        }

    }
}