using System;
using System.Collections.Generic;

namespace SIP____Super_Information_Tracker.DataStructures
{
    public class SipFile
    {
        private List<SipFileSnapshot> _histories;
    }
    public class SipFileSnapshot
    {
        public Guid Guid;
        public long Version;
        public List<SipLine> Lines;
    }

    public class SipLine
    {
        public string Value;
        public int LineNumber;
    }
}