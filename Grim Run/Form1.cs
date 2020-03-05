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
        private string dllPath = "D:\\Projects\\Grim Run\\Debug\\grhook.dll";
        private string processName = "Grim Dawn";

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
                DataRec dataRec = Marshal.PtrToStructure<DataRec>(cds.lpData);
                
                this.textBox1.Text = dataRec.Text.Substring(0, cds.cbData);
            }

            base.WndProc(ref m);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            GrimRunInjector.Inject(dllPath, processName);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DataRec
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string Text;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
}
