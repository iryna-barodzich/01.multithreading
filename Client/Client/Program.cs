using System;
using System.IO;
using System.Messaging;
using System.Threading;

namespace Client
{
    class Program
    {
        static Thread workThread;
        static string ServerQueueName = @".\Private$\server_task3";
        static string WatcherDirectoryName = @"D:\Task3_Messaging";
        static int ChunkSize = 4;
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

                watcher.Created += OnCreated;

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
            using (var serverQueue = new MessageQueue(ServerQueueName, QueueAccessMode.Send))
            {
                serverQueue.MessageReadPropertyFilter.SetAll();
                serverQueue.MessageReadPropertyFilter.IsFirstInTransaction = true;
                serverQueue.MessageReadPropertyFilter.IsLastInTransaction = true;

                var filename = obj.ToString();
                var bytes = FileToByteArray(obj.ToString());
                var size = bytes.Length;

                byte[][] blocks = new byte[(bytes.Length + 4 - 1) / ChunkSize][];

                for (int i = 0, j = 0; i < blocks.Length; i++, j += ChunkSize)
                {
                    blocks[i] = new byte[Math.Min(ChunkSize, bytes.Length - j)];
                    Array.Copy(bytes, j, blocks[i], 0, blocks[i].Length);
                }

                var transaction = new MessageQueueTransaction();
                try
                {
                    transaction.Begin();
                    //foreach(var part in blocks)
                    for(int i = 0; i < blocks.Length; i++)
                    {
                        Message message = new Message(blocks[i], new BinaryMessageFormatter());
                        message.Label = $"{filename}|{i+1}|{blocks.Length}";
                        serverQueue.Send(message, MessageQueueTransactionType.Automatic);
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