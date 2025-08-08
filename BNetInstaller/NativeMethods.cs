using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace BNetInstaller;

internal static partial class NativeMethods
{
    private const int MIB_TCPROW2_SIZE = 0x1C;
    private const int MIB_TCP_STATE_LISTEN = 2;

    [LibraryImport("iphlpapi.dll", EntryPoint = "GetTcpTable2")]
    private static partial int GetTcpTable(nint tcpTable, ref int size, [MarshalAs(UnmanagedType.Bool)] bool bOrder);

    public static int GetProcessListeningPort(int pid)
    {
        var size = 0;

        // get MIB_TCPTABLE2 size
        _ = GetTcpTable(0, ref size, false);

        var buffer = Marshal.AllocHGlobal(size);
        var pBuffer = buffer;

        try
        {
            // read MIB_TCPTABLE2
            if (GetTcpTable(pBuffer, ref size, false) != 0)
                return -1;

            var dwNumEntries = Marshal.ReadInt32(pBuffer); // MIB_TCPTABLE2->dwNumEntries
            pBuffer += sizeof(int);

            for (var i = 0; i < dwNumEntries; i++)
            {
                var row = Marshal.PtrToStructure<MIB_TCPROW2>(pBuffer);
                pBuffer += MIB_TCPROW2_SIZE;

                if (row.dwOwningPid == pid && row.dwState == MIB_TCP_STATE_LISTEN)
                {
                    return BinaryPrimitives.ReverseEndianness((ushort)row.dwLocalPort);
                }
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }

        return -1;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MIB_TCPROW2
    {
        public int dwState;
        public int dwLocalAddr;
        public int dwLocalPort;
        public int dwRemoteAddr;
        public int dwRemotePort;
        public int dwOwningPid;
        public int dwOffloadState;
    }
}
