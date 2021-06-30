using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSI
{
    public partial class MainForm : Form
    {
        public double Ca = 98.0;
        public double Mg = 82.2;
        public double Na = 100.0;
        public double Cl = 100.0;
        public double Clmax = 1000.0;
        public double Cond = 50.0;
        public double Alkalinity = 200.0;
        public double COCmax = 0.0;
        public const double C_Ca = 2.45;
        public const double C_Mg = 4.11;
        public MainForm()
        {
            InitializeComponent();
            COCmax = Clmax / Cl;
            label_COCmax.Text = "最大浓缩倍数：" + COCmax.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm(this);
            DialogResult result = settingForm.ShowDialog();
            switch (result)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    //COCmax = Clmax / Cl;
                    label_COCmax.Text = "最大浓缩倍数：" + COCmax.ToString();
                    break;
                case DialogResult.Cancel:
                    Show();
                    break;
                case DialogResult.Abort:
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void Calculate_COC()
        {
            COCmax = 0.0;
        }
    }
}
