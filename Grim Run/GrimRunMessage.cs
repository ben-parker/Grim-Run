using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grim_Run
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GrimRunMessage
    {
        [MarshalAs(UnmanagedType.I4)]
        public MessageType MessageType;
        public float Damage;
        public int DataLen;
        public int Data2Len;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] bytes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Data;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Data2;
    }
}
