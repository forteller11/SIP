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
        private static char[] SIP_FOLDER_NAME = "SIP".ToCharArray();
        static void Main(string[] args)
        {
            RunTests();
            Console.WriteLine($"Welcome to SIP\n" +
                              $"Commands:\n" +
                              $"-- Create");

            Input.Input.OnTrackDirectory += Track;
            Input.Input.OnAddFile += AddFile;
            Input.Input.OnRepoInit += InitVCS;
            
            while (true)
            {
                var input = Console.ReadLine();
                Input.Input.Parse(input); 
                Thread.Sleep(1000/60);
            }
        }

        public static void RunTests()
        {
            IO.Path.Test();
        }
        public static bool DirectoryOrFileExist(ReadOnlySpan<char> path)
        {
            return DirectoryOrFileExist(path.ToString());
        }
        public static bool DirectoryOrFileExist(string path)
        {
            return (File.Exists(path) || Directory.Exists(path));
        }

        public static void InitVCS(string path)
        {
            _sipRoot = path.ToCharArray();
            _vcsRoot = IO.Path.Combine(path, SIP_FOLDER_NAME);
            Directory.CreateDirectory(new string(_vcsRoot));

            Track(path);
        }
        
        public static void AddFile(string input)
        {
            var inputCharArray = input.ToCharArray();
            char[] fullFilePath;
            
            fullFilePath = IO.Path.IsWithinPath(_sipRoot, inputCharArray) ? 
                inputCharArray : 
                IO.Path.Combine(_sipRoot, inputCharArray);

            if (!DirectoryOrFileExist(fullFilePath))
            {
                Console.WriteLine($"{fullFilePath} does not exist");
                return;
            }

            var relativePath = IO.Path.GetRelativePath(_sipRoot, fullFilePath);
            var vcsMirrorPath = IO.Path.Combine(_vcsRoot, relativePath);
            if (DirectoryOrFileExist(vcsMirrorPath))
            {
                Console.WriteLine($"{fullFilePath} is already added to SIP");
                return;
            }
            
            var sipFile = SipFile.CreateNew(Guid.NewGuid(), vcsMirrorPath);
            sipFile.Serialize(_vcsRoot);
            Console.WriteLine($"Added {IO.Path.GetNameFromPath(fullFilePath).ToString()} at {new string(fullFilePath)}");
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

            bool isWithinPath = IO.Path.IsWithinPath(_vcsRoot, fullPath);
            if (isWithinPath)
            {
                return;
            }
            
            ReadOnlySpan<char> relativeToSipRoot = IO.Path.GetRelativePath(_sipRoot, fullPath);
            var vcsFullPath = IO.Path.Combine(_vcsRoot,  relativeToSipRoot);
            
            //check if there's a vcs path
            SipFile sipFile;
            if (!File.Exists(new string(vcsFullPath)) && !Directory.Exists(new string(vcsFullPath)))
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
        
        //todo add
        //todo change
        //todo rename
    }
}