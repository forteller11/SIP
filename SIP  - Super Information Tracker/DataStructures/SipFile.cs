using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIP.DataStructures
{
    public class SipFile
    {
        public Guid Guid;
        public List<SipFileSnapshot> Snapshots = new List<SipFileSnapshot>();
        
        public static SipFile CreateNew(Guid guid, string vcsMirrorRoot)
        {
            var sipFile = new SipFile();
            sipFile.Snapshots = new List<SipFileSnapshot>();
            sipFile.Guid = guid;

            var firstSnapshot = new SipFileSnapshot(vcsMirrorRoot, 0);
            sipFile.Snapshots.Add(firstSnapshot);
            
            return sipFile;
        }

        public void Serialize(string sipDirPath)
        {
            var stringBuilder = new StringBuilder();
            
            string path = Guid.ToString();
            stringBuilder.Append(path);
            
            foreach (var snapshots in Snapshots)
            {
                stringBuilder.Append(snapshots);
            }

            string fullPath = Path.Combine(sipDirPath, path);
            File.WriteAllText(fullPath, stringBuilder.ToString());
        }

    }
    public class SipFileSnapshot
    {
        private string PathName; //relative to SIP root
        public long Version;
        public List<SipLine>? Lines; //directories dont have lines

        public SipFileSnapshot(string pathName, long version)
        {
            PathName = pathName;
            Version = version;
        }

        public string Serialize()
        {
            var stringBuilder = new StringBuilder();
            string header = 
                $"Version: {Version}\n" +
                $"Path: {PathName}\n";

            stringBuilder.Append(header);

            if (Lines == null)
            {
                return stringBuilder.ToString();
            }
            
            foreach (var line in Lines)
            {
                stringBuilder.Append(line);
            }

            return stringBuilder.ToString();
        }
    }

    public class SipLine
    {
        public string Value;
        public int LineNumber;
    }
}