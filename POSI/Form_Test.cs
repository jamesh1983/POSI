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
    public partial class Form_Test : Form
    {
        public double Ca = 180.0;
        public double Mg = 19.0;
        public double Na = 100.0;
        public double Cl = 100.0;
        public double Clmax = 1000.0;
        public double Cond = 50.0;
        public double Alkalinity_Limit = 680.0;
        public double Alkalinity_Input = 198.0;
        public double COC_Cl = 10;
        public double COC_Al = 0.0;
        public double Alkalinity_Max = 0.0;

        public double Ca_Alkalinity = 0.0;
        public double Cl_SO4 = 0.0;
        public double SiO2 = 0.0;
        public double Mg_SiO2 = 0.0;

        public const double C_Ca = 2.45;
        public const double C_Mg = 4.11;
        public const double Delta_Coc = 0.01;
        public Form_Test()
        {
            InitializeComponent();
            do
            {
                Alkalinity_Max = Math.Round((COC_Cl * Math.Pow(10, (1 / (Math.Log10(COC_Cl) * Math.Log10(Ca * Mg / (Cl + Na))) + 1))), 2);
                COC_Cl = COC_Cl - Delta_Coc;
            } while (Alkalinity_Max < Alkalinity_Limit);
            Label_Result.Text += COC_Cl.ToString();
        }
    }
}
