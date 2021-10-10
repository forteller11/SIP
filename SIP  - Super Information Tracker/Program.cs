using System;
using System.IO;
using System.Threading;
using SIP.DataStructures;

namespace SIP
{
    class Program
    {
        private static char[]? _sipRoot;
        private static char[]? _vcsRoot;
        private static char[] SIP_FOLDER_NAME = ".SIP".ToCharArray();
        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to SIP\n" +
                              $"Commands:\n" +
                              $"-- Create");

            Input.Input.OnTrackDirectory += Track;
            
            while (true)
            {
                var input = Console.ReadLine();
                Input.Input.Parse(input); 
                Thread.Sleep(1000/60);
            }
        }

        public static ReadOnlySpan<char> GetVCSMirrorPath(string fullPath)
        {
            var relativePath = IO.Path.GetRelativePath(_sipRoot, fullPath);
            var mirrorPath = IO.Path.Combine(_vcsRoot, relativePath);
            return mirrorPath;
        }

        public static void Track(string filePath)
        {
            Console.WriteLine($"Begun Tracking File System at {filePath}");

            _sipRoot = filePath.ToCharArray();
            _vcsRoot = IO.Path.Combine(filePath, SIP_FOLDER_NAME);
            
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
            fileWatcher.Changed += FileWatcherOnChanged;
        }
        

        private static void FileWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"{e.FullPath} has changed");
            char[] fullPath = e.FullPath.ToCharArray();

            bool isWithinPath = IO.Path.IsWithinPath(_sipRoot, fullPath);
            if (isWithinPath)
            {
                return;
            }
            
            ReadOnlySpan<char> relativeToSipRoot = IO.Path.GetRelativePath(_sipRoot, fullPath);
            var vcsFullPath = IO.Path.Combine(_sipRoot,  relativeToSipRoot);
            
            //check if there's a vcs path
            SipFile sipFile;
            if (!File.Exists(vcsFullPath.ToString()) && !Directory.Exists(vcsFullPath.ToString()))
            {
                Console.WriteLine("Creating new stuff");
                sipFile = SipFile.CreateNew(Guid.NewGuid(), vcsFullPath);
            }
            else
            {
                throw new NotImplementedException();
                //deserialize,
                //add onto,
                //then researize file
            }
            
            sipFile.Serialize(_vcsRoot);
            
        }
    }
}