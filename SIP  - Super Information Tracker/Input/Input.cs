using System;

namespace SIP.Input
{
    public static class Input
    {
        public static event CreateNewRepo? OnCreateNewRepo;
        public delegate void CreateNewRepo(string filePath);
        
        
        public static event TrackDirectory? OnTrackDirectory;
        public delegate void TrackDirectory(string filePath);

        public const string KEYWORD = "sip";
        public const string CREATE = "create";
        public const string TRACK = "track";
        public static void Parse(string input)
        {
            var words = input.Split(" ", StringSplitOptions.TrimEntries);
            var keyword = words[0];

            if (!keyword.Equals(KEYWORD, StringComparison.OrdinalIgnoreCase)) 
                return;
            
            string action = words[1];
            string? data = words.Length >= 3
                ? words[2] 
                : null;

            switch (action)
            {
                case CREATE:
                    if (data == null)
                    {
                        Console.WriteLine($"Need third argument path for {nameof(CREATE)}");
                        break;
                    }

                    Console.WriteLine("Creating");
                    OnCreateNewRepo?.Invoke(data);
                    break;
                
                case TRACK:
                    if (data == null)
                    {
                        Console.WriteLine($"Need third argument path for {nameof(TRACK)}");
                        break;
                    }
                    
                    Console.WriteLine("Tracking");
                    OnTrackDirectory?.Invoke(data);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}