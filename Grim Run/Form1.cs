using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        public Form1()
        {
            InitializeComponent();
            this.Text = "Grim Run Test";
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


        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            if (m.Msg == WM_COPYDATA)
            {
                COPYDATASTRUCT cds = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                GrimRunMessage msg = Marshal.PtrToStructure<GrimRunMessage>(cds.lpData);

                if (msg.AttackerNameLen > 0)
                {
                    this.textBox1.Text = msg.AttackerName.Substring(0, msg.AttackerNameLen);
                }
                else
                {
                    this.textBox1.Text = msg.AttackerName;
                }

                if (msg.Damage > 0)
                {
                    totalDamage += msg.Damage;
                    totalDamageDisplay.Text = totalDamage.ToString();
                }
            }

            base.WndProc(ref m);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            return;// GrimRunInjector.Inject(dllPath, processName);
        }

        private void button1_Click(object sender, EventArgs e)
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
