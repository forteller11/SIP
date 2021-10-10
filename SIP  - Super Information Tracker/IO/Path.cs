using System;

namespace SIP.IO
{
    /// <summary>
    ///  This only works on windows
    /// </summary>
    public struct Path
    {
        public static readonly char PathSeperator;
        static Path()
        {
            //for windows only
            PathSeperator = '/';
        }

        public static char[] Combine(ReadOnlySpan<char> lPath, ReadOnlySpan<char> rPath)
        {
            char[] span = new char[lPath.Length + 1 + rPath.Length];
            
            for (int i = 0; i < lPath.Length; i++)
            {
                span[i] = lPath[i];
            }

            span[lPath.Length + 1] = PathSeperator;

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

        public static ReadOnlySpan<char> GetRelativePath(ReadOnlySpan<char> parentPath, ReadOnlySpan<char> childPath)
        {
            if (childPath.Length > parentPath.Length)
                return null;

            if (childPath.Length == parentPath.Length)
                return Array.Empty<char>();

            //make sure that child is a superset of parent path
            if (IsWithinPath(parentPath, childPath))
                return null;

            //todo bug, will this include path seperator at start of relative path? /relativePath/Example.txt, should be length + 1 instead?
            return childPath.Slice(parentPath.Length);

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