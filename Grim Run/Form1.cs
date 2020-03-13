using Grim_Run;
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
        private string dllPath = "D:\\Projects\\Grim Run\\x64\\Debug\\grhook.dll";
        private string processName = "Grim Dawn";

        private List<Label> damageValueDisplays;

        private GameEventListener listener;
        private GameEventParser parser;
        private DamageTracker damageTracker;

        public Form1()
        {
            InitializeComponent();

            // event listener class to receive messages
            // message parser class to translate messages into data
            // https://stackoverflow.com/questions/22385529/how-do-i-communicate-with-a-control-of-a-form-from-another-class?noredirect=1&lq=1
            // bind damage displays to a class that is updated by the parser class

            var progress = new Progress<(float, float, DamageType)>(UpdateTotalDamage);
            damageValueDisplays = new List<Label>
            {
                physDmg, piercingDmg, fireDmg, coldDmg, lightningDmg,
                acidDmg, vitalityDmg, aetherDmg, chaosDmg, totalDamageDisplay
            };

            //Task.Run(() => PipeServer());
            damageTracker = new DamageTracker(progress);
            parser = new GameEventParser(damageTracker);
            listener = new GameEventListener(parser);
        }

        private void UpdateTotalDamage((float total, float damage, DamageType type) d)
        {
            var damageTypeValue = d.type switch
            {
                DamageType.Physical => physDmg,
                DamageType.Piercing => piercingDmg,
                DamageType.Fire => fireDmg,
                DamageType.Cold => coldDmg,
                DamageType.Lightning => lightningDmg,
                DamageType.Acid => acidDmg,
                DamageType.Vitality => vitalityDmg,
                DamageType.Aether => aetherDmg,
                DamageType.Chaos => chaosDmg,
            };

            totalDamageDisplay.Text = d.total.ToString("N0");
            damageTypeValue.Text = d.damage.ToString("N0");
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // GrimRunInjector.Uninject(dllPath, processName);
            listener.Stop();
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

        private void resetButton_Click(object sender, EventArgs e)
        {
            damageTracker.Reset();

            foreach (var label in damageValueDisplays)
            {
                label.Text = "0";
            }
        }
    }
}
