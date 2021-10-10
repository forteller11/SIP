using System;
using System.Diagnostics;
using System.IO.Compression;

namespace SIP.IO
{
    /// <summary>
    ///  This only works on windows
    /// </summary>
    public static class Path
    {
        public static readonly char PathSeperator;
        public static readonly char ExtensionSeperator;
        static Path()
        {
            //for windows only
            PathSeperator = '\\';
            ExtensionSeperator = '.';

            Test();
        }
        
        public static char[] Combine(ReadOnlySpan<char> lPath, ReadOnlySpan<char> rPath)
        {
            char[] span = new char[lPath.Length + 1 + rPath.Length];
            
            for (int i = 0; i < lPath.Length; i++)
            {
                span[i] = lPath[i];
            }

            span[lPath.Length] = PathSeperator;

            for (int i = 0; i < rPath.Length; i++)
            {
                int newIndex = lPath.Length + 1 + i;
                span[newIndex] = rPath[i];
            }

            return span;
        }
        
        public static char[] AddExtension(ReadOnlySpan<char> lPath, ReadOnlySpan<char> rPath)
        {
            char[] span = new char[lPath.Length + 1 + rPath.Length];
            
            for (int i = 0; i < lPath.Length; i++)
            {
                span[i] = lPath[i];
            }

            span[lPath.Length] = ExtensionSeperator;

            for (int i = 0; i < rPath.Length; i++)
            {
                int newIndex = lPath.Length + 1 + i;
                span[newIndex] = rPath[i];
            }

            return span;
        }

        //to
        public static bool CharCompare(char l, char r)
        {
            return l == r;
        }

        public static ReadOnlySpan<char> GetNameFromPath(ReadOnlySpan<char> path)
        {
            if (path.Length == 1 && path[0] == PathSeperator)
                return ReadOnlySpan<char>.Empty;

            for (int i = path.Length-1; i >= 0; i--)
            {
                if (path[i] == PathSeperator)
                    return path.Slice(i + 1);
            }
            
            return ReadOnlySpan<char>.Empty;
        }

        public static bool CharArrayCompare(ReadOnlySpan<char> lPath, ReadOnlySpan<char> rPath)
        {
            if (lPath.Length != rPath.Length)
                return false;
            
            for (int i = 0; i < lPath.Length; i++)
            {
                if (!CharCompare(lPath[i], rPath[i]))
                    return false;
            }

            return true;
        }

        // public static char[] Replace(ReadOnlySpan<char> input, char replaceWhat, ReadOnlySpan<char> replaceWith)
        // {
        //     int sizeIncre
        //     for (int i = 0; i < UPPER; i++)
        //     {
        //         
        //     }
        // }
        // public static char EscapePath(ReadOnlySpan<char> input)
        // {
        //     int newSize = input.Length;
        //     for (int i = 0; i < input.Length; i++)
        //     {
        //         if (input[i] == PathSeperator)
        //         {
        //             newSize += 1;
        //         }
        //     }
        //
        //     char[] newPath = new char[newSize];
        //     int inputPathIndex = 0;
        //     int newPathIndex = 0;
        //     while (inputPathIndex < input.Length)
        //     {
        //         newPath[newPathIndex] = input[inputPathIndex];
        //         if (input[inputPathIndex] == PathSeperator)
        //         {
        //             inputPathIndex++;
        //             newPath[newPathIndex] = PathSeperator;
        //         }
        //         inputPathIndex++;
        //         newPathIndex++;
        //     }
        //     for (int i = 0; i < input.Length; i++)
        //     {
        //         
        //     }
        // }

        // public static void EscapePath(Span<char> input)
        // {
        //     int newSize = input.Length;
        //     for (int i = 0; i < input.Length; i++)
        //     {
        //         if (input[i] == '\')
        //         {
        //             newSize += 1;
        //         }
        //     }
        // }

        public static ReadOnlySpan<char> GetRelativePath(ReadOnlySpan<char> parentPath, ReadOnlySpan<char> childPath)
        {
            if (parentPath.Length > childPath.Length)
                return null;

            if (childPath.Length == parentPath.Length)
                return Array.Empty<char>();

            //make sure that child is a superset of parent path
            if (!IsWithinPath(parentPath, childPath))
                return null;

            //todo bug, will this include path seperator at start of relative path? /relativePath/Example.txt, should be length + 1 instead?
            var result =  childPath.Slice(parentPath.Length+1);
            return result;
        }

        public static void Test()
        {
            var path1 = @"D:\MOVE\Homework\Hobby\Custom-Realtime-VCS\TestFolder".ToCharArray();
            var path2 = @"D:\MOVE\Homework\Hobby".ToCharArray();
            var path3 = @"Custom-Realtime-VCS\TestFolder".ToCharArray();
            var path4 = @"TestFolder".ToCharArray();
            
            Debug.Assert(IsWithinPath(path2, path1));

            var test2Data = GetRelativePath(path2, path1);
            bool test2Result = CharArrayCompare(test2Data, path3);
            Debug.Assert(test2Result);

            var test3Data = Combine(path2, path3);
            bool test3Result = CharArrayCompare(test3Data, path1);
            Debug.Assert(test3Result);

            var test4data = GetNameFromPath(path3);
            bool test4Result = CharArrayCompare(path4, test4data);
            Debug.Assert(test4Result);
        }

        public static bool IsWithinPath(ReadOnlySpan<char> parentPath, ReadOnlySpan<char> childPath)
        {
            for (int i = 0; i < parentPath.Length; i++)
            {
                if (!CharCompare(parentPath[i], childPath[i]))
                    return false;
            }

            return true;
        }
    }
}