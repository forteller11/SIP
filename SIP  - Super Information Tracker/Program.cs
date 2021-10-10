using System;
using System.IO;
using System.Threading;
using SIP.DataStructures;

namespace SIP
{
    class Program
    {
        private static string? _rootPath;
        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to SIP\n" +
                              $"Commands:\n" +
                              $"-- Create");

            while (true)
            {
                var input = Console.ReadLine();
                Input.Input.OnTrackDirectory += Track;
                Input.Input.Parse(input); 
            }
            //todo create repo files.....
            //todo begin tracking files within folder....
            //todo everynew finder creates a folder with mirror deltas.....
            //each file has single file tracking ALL of its changes
        }

        public static string GetVCSRoot()
        {
            return Path.Combine(_rootPath, ".SIP");
        }

        public static string GetVCSMirrorPath(string fullPath)
        {
            string relativePath = Path.GetRelativePath(_rootPath, fullPath);
            string vcsRoot = GetVCSRoot();
            string mirrorPath = Path.Combine(vcsRoot, relativePath);
            return mirrorPath;
        }

        public static bool IsInVCSFolder(string fullpath)
        {
        }
        public static string GetFileRelativePath(string fullPath)
        {
            return  Path.GetRelativePath(_rootPath, fullPath);
        }
        
        
        public static void Track(string filePath)
        {
            Console.WriteLine($"Track at {filePath}");

            _rootPath = filePath;
            Directory.CreateDirectory(filePath);
            Directory.GetParent(filePath);
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
        
        public bool IsInRoot()

        private static void FileWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            //todo this doesn't get the full path rn, it gets relative
            string vcsRoot = GetVCSRoot();
            string fileChangedFullPath = e.FullPath;

            string relativeToSipRoot = Path.GetRelativePath(GetVCSRoot(), fileChangedFullPath);

            if (relativeToSipRoot != null)
            {
                return;
            }
            
            //tood 
            string vcsFullPath = GetVCSMirrorPath(e.FullPath);
            
            string relativePath = Path.GetRelativePath(_rootPath, e.FullPath);
            Console.WriteLine($"{relativePath} has changed");
            
            //check if there's a vcs path
            SipFile sipFile;
            if (!File.Exists(vcsFullPath) && !Directory.Exists(vcsFullPath))
            {
                Console.WriteLine("Creating new stuff");
                DirectoryInfo info = new DirectoryInfo(e.FullPath);
                
                sipFile = SipFile.CreateNew(Guid.NewGuid(), GetVCSRoot());
            }
            else
            {
                throw new NotImplementedException();
                //deserialize,
                //add onto,
                //then researize file
            }
            
            sipFile.Serialize(relativePath);
            
        }
    }
}