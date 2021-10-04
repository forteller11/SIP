using System;

namespace SIP____Super_Information_Tracker.Input
{
    public static class Input
    {
        public static event CreateNewRepo? OnCreateNewRepo;
        public delegate void CreateNewRepo(string filePath);

        public const string KEYWORD = "sip";
        public const string CREATE = "create";
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
                        Console.WriteLine($"Need third argument path for create");
                        break;
                    }

                    Console.WriteLine("Creating");
                    OnCreateNewRepo?.Invoke(data);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}