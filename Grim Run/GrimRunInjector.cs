﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrimRun
{
    class GrimRunInjector
    {
        private const int PROCESS_CREATE_THREAD = 0x0002;
        private const int PROCESS_QUERY_INFORMATION = 0x0400;
        private const int PROCESS_VM_OPERATION = 0x0008;
        private const int PROCESS_VM_WRITE = 0x0020;
        private const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        private const uint MEM_COMMIT = 0x00001000;
        private const uint MEM_RESERVE = 0x00002000;
        private const uint PAGE_READWRITE = 4;

        public static void Inject(string dllName, string targetProcessName)
        {
                        
            Console.WriteLine("We're running!");

            Process targetProcess = Process.GetProcessesByName(targetProcessName)[0];
            IntPtr procHandle = OpenProcess(processPermissions(), false, targetProcess.Id);
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            var size = (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char)));
            // allocating some memory on the target process - enough to store the name of the dll
            // and storing its address in a pointer
            IntPtr allocMemAddress = VirtualAllocEx(procHandle,
                                                    IntPtr.Zero,
                                                    (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))),
                                                    MEM_COMMIT | MEM_RESERVE,
                                                    PAGE_READWRITE);

            // writing the name of the dll there
            UIntPtr bytesWritten;
            WriteProcessMemory(procHandle,
                                allocMemAddress,
                                Encoding.Default.GetBytes(dllName),
                                (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))),
                                out bytesWritten);

            // creating a thread that will call LoadLibraryA with allocMemAddress as argument
            IntPtr hThread =
                    CreateRemoteThread(procHandle, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);

            if (hThread == IntPtr.Zero)
            {
                System.Console.WriteLine(Marshal.GetLastWin32Error());
            }
            else
            {
                Console.WriteLine($"CreateRemoteThread result is {hThread}");
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
                                                        IntPtr lpThreadAttribute,
                                                        IntPtr dwStackSize,
                                                        IntPtr lpStartAddress,
                                                        IntPtr lpParameter,
                                                        uint dwCreationFlags,
                                                        IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess,
                                            IntPtr lpAddress,
                                            uint dwSize,
                                            uint flAllocationType,
                                            uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess,
                                                IntPtr lpBaseAddress,
                                                byte[] lpBuffer,
                                                uint nSize,
                                                out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        private static int processPermissions()
        {
            return PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION |
                PROCESS_VM_WRITE | PROCESS_VM_READ;
        }
    }
}
