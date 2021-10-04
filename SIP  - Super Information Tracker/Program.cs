using System;
using System.IO;
using System.Threading;

namespace SIP____Super_Information_Tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to SIP\n" +
                              $"Commands:\n" +
                              $"-- Create");

            while (true)
            {
                var input = Console.ReadLine();
                Input.Input.OnCreateNewRepo += Create;
                Input.Input.Parse(input); 
            }
            //todo create repo files.....
            //todo begin tracking files within folder....
            //todo everynew finder creates a folder with mirror deltas.....
            //each file has single file tracking ALL of its changes
        }

        public static void Create(string filePath)
        {
            Console.WriteLine($"Create at {filePath}");
            
            Directory.CreateDirectory(filePath);
            FileSystemWatcher fileWatcher = new(filePath)
            {
                NotifyFilter = NotifyFilters.FileName | 
                               NotifyFilters.LastAccess | 
                               NotifyFilters.LastWrite |
                               NotifyFilters.DirectoryName,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            fileWatcher.Created += FileWatcherOnChanged;
            fileWatcher.Changed += FileWatcherOnChanged;
            //todo create repo files.....
            //todo begin tracking files within folder....
            //todo on changes to file.....
            //track deltas....
        }

        private static void FileWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath);
        }
    }
}