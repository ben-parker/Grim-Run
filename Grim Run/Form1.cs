using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrimRun
{
    public partial class Form1 : Form
    {
        private const uint WM_COPYDATA = 0x4A;
        private string dllPath = "D:\\Projects\\Grim Run\\x64\\Debug\\grhook.dll";
        private string processName = "Grim Dawn";

        private float totalDamage;
        private bool listen;
        private Thread pipeListener;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Grim Run Test";

            var progress = new Progress<GrimRunMessage>(UpdateDamage);
            pipeListener = new Thread(() => PipeServer(progress));
            pipeListener.Start();
            listen = true;
            //Task.Run(() => PipeServer());
        }

        private void UpdateDamage(GrimRunMessage msg)
        {
            totalDamage += msg.Damage;
            totalDamageDisplay.Text = totalDamage.ToString();
        }
        private void Form1_Shown(Object sender, EventArgs e)
        {
            try
            {
                GrimRunInjector.Inject(dllPath, processName);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.Error.WriteLine($"Unable to find process by name {processName}, exiting.");
                this.Close();
            }
        }

        private void PipeServer(IProgress<GrimRunMessage> progress)
        {
            int bytesRead = 0;
            var pipe = new NamedPipeServerStream("GrimRunPipe", PipeDirection.In);
            pipe.WaitForConnection();

            while (listen)
            {
                var bytes = new byte[Marshal.SizeOf(typeof(GrimRunMessage))];
                bytesRead = pipe.Read(bytes, 0, Marshal.SizeOf(typeof(GrimRunMessage)));

                if (bytesRead > 0)
                {
                    GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    try
                    {
                        var msg = Marshal.PtrToStructure<GrimRunMessage>(handle.AddrOfPinnedObject());
                        Console.WriteLine($"Attacker {msg.AttackerName} Damage {msg.Damage}");
                        progress.Report(msg);
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                else
                {
                    Console.Error.WriteLine("Failed to read from pipe, shutting my ears");
                    listen = false;
                }
                
            }
        }

        //protected override void WndProc(ref Message m)
        //{
        //    // Listen for operating system messages.
        //    if (m.Msg == WM_COPYDATA)
        //    {
        //        COPYDATASTRUCT cds = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
        //        GrimRunMessage msg = Marshal.PtrToStructure<GrimRunMessage>(cds.lpData);

        //        if (msg.AttackerNameLen > 0)
        //        {
        //            this.textBox1.Text = msg.AttackerName.Substring(0, msg.AttackerNameLen);
        //        }
        //        else
        //        {
        //            this.textBox1.Text = msg.AttackerName;
        //        }

        //        if (msg.Damage > 0)
        //        {
        //            totalDamage += msg.Damage;
        //            totalDamageDisplay.Text = totalDamage.ToString();
        //        }
        //    }

        //    base.WndProc(ref m);
        //}

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // GrimRunInjector.Inject(dllPath, processName);
            listen = false;
            pipeListener.Join();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listen = true;
                GrimRunInjector.Inject(dllPath, processName);
            }
            catch (IndexOutOfRangeException ex)
            {
                listen = false;
                Console.Error.WriteLine($"Unable to find process by name {processName}, exiting.");
                this.Close();
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GrimRunMessage
    {
        public float Damage;
        public int AttackerNameLen;
        public int DefenderNameLen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AttackerId;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string AttackerName;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string DefenderId;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string DefenderName;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string CombatType;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string DamageType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
}
