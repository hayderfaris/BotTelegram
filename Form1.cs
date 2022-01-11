using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindwosBotTimeCurrency
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public bool TurnedOnFlag = false;
        public Bot bot { get; set; }
        public Form1()
        {
            InitializeComponent();
            TurnedOff(false);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TurnedOnFlag) TurnedOff();
            else TurnedOn();
            TurnedOnFlag = !TurnedOnFlag;

        }

        void TurnedOff(bool offIt = true)
        {
            this.button1.Text = "Start Bot";
            this.button1.BackColor = Color.FromArgb(128, 255, 128);
            this.button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 255, 128);
            if(offIt) bot.Stop();

        }

        void TurnedOn()
        {
            this.button1.Text = "Stop Bot";
            this.button1.BackColor = Color.Red;
            this.button1.FlatAppearance.MouseOverBackColor = Color.Red;

            //if (textBox1.Text.Length > 30)
            //    bot = new Bot(textBox1.Text);
            var g = System.Configuration.ConfigurationSettings.AppSettings.Keys[0];
            bot = new Bot(g);

            bot.Start();
        }
    }
}
