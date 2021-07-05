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
        public double COC_Cl = 10;
        public double COC_Al = 4.2;
        public double Alkalinity_Max = 0.0;

        public double Ca_Alkalinity = 298.0;
        public double Cl_SO4 = 200.0;
        public double SiO2 = 200.0;
        public double Mg_SiO2 = 200.0;

        public const double C_Ca = 2.45;
        public const double C_Mg = 4.11;
        public const double Delta_Coc = 0.01;
        private SettingForm settingForm = null;

        public MainForm()
        {
            InitializeComponent();
            settingForm = new SettingForm(this);
            COC_Cl = Math.Round((Clmax / Cl),2);
            //label_COCmax.Text = "最大浓缩倍数：" + COC_Cl.ToString();
            label4.Text = Ca.ToString();
            label7.Text = Mg.ToString();
            label10.Text = (Ca + Mg).ToString();
            label13.Text = Alkalinity.ToString();
            label16.Text = Cl.ToString();
            label19.Text = Na.ToString();
            label21.Text = COC_Cl.ToString();
            label22.Text = (Cond * COC_Cl).ToString();
            label24.Text = (Alkalinity * COC_Cl).ToString();
            label33.Text = (Ca_Alkalinity * COC_Cl).ToString();
            label31.Text = (Cl_SO4 * COC_Cl).ToString();
            label36.Text = (SiO2 * COC_Cl).ToString();
            label39.Text = (Mg_SiO2 * COC_Cl).ToString();
            //Calculate_COC();
            //chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = settingForm.ShowDialog();
            switch (result)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    //COCmax = Clmax / Cl;
                    //label_COCmax.Text = "最大浓缩倍数：" + COC_Cl.ToString();
                    label4.Text = Ca.ToString();
                    label7.Text = Mg.ToString();
                    label10.Text = (Ca+ Mg).ToString();
                    label13.Text = Alkalinity.ToString();
                    label16.Text = Cl.ToString();
                    label19.Text = Na.ToString();
                    Calculate_COC();
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
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            COC_Cl = Math.Round((Clmax / Cl), 2);
            Alkalinity_Max = Math.Round(Math.Pow(10, ((1/ Math.Log10(COC_Cl)) / (Math.Log10(Ca) + Math.Log10(Mg) - Math.Log10(Cl + Na)) + 1)), 2);
            Alkalinity_Max = Math.Round(Math.Pow(10, ((1 / Math.Log10(COC_Cl)) / (Math.Log10(Ca*Mg/(Cl + Na))) + 1)), 2);
            COC_Al = Math.Pow(10, 1 / (Math.Log10(Ca * Mg / (Cl + Na)) * Math.Log10(Alkalinity / 10)));
            COC_Al = Math.Round(COC_Al,2);
            if (COC_Cl < 2)
                MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
            else
            {
                if (COC_Cl <= COC_Al)
                {
                    label21.Text = COC_Cl.ToString();
                    label22.Text = (Cond * COC_Cl).ToString();
                    label24.Text = (Alkalinity * COC_Cl).ToString();
                    label33.Text = (Ca_Alkalinity * COC_Cl).ToString();
                    label31.Text = (Cl_SO4 * COC_Cl).ToString();
                    label36.Text = (SiO2 * COC_Cl).ToString();
                    label39.Text = (Mg_SiO2 * COC_Cl).ToString();
                    int Chart_N = Convert.ToInt32((COC_Cl - 2) / Delta_Coc);
                    double[] X_Axis = new double[Chart_N];
                    double[] Y_Axis = new double[Chart_N];
                    double[] Xmax_Axis = new double[1];
                    double[] Ymax_Axis = new double[1];
                    for (int i = 0; i < Chart_N; i++)
                    {
                        X_Axis[i] = COC_Cl - (Delta_Coc * i);
                        Y_Axis[i] = Math.Round(Math.Pow(10, ((1 / Math.Log10(X_Axis[i])) / (Math.Log10(Ca * Mg / (Cl + Na))) + 1)), 2);
                    }
                    Xmax_Axis[0] = COC_Cl;
                    Ymax_Axis[0] = Math.Round(Math.Pow(10, ((1 / Math.Log10(Xmax_Axis[0])) / (Math.Log10(Ca * Mg / (Cl + Na))) + 1)), 2);
                    chart1.Series[0].Points.DataBindXY(Y_Axis, X_Axis);
                    chart1.Series[2].Points.DataBindXY(Ymax_Axis, Xmax_Axis);
                }
                else
                {
                    double[] Xmax_Axis = new double[1];
                    double[] Ymax_Axis = new double[1];
                    label21.Text = COC_Al.ToString();
                    label22.Text = (Cond * COC_Al).ToString();
                    label24.Text = (Alkalinity * COC_Al).ToString();
                    label33.Text = (Ca_Alkalinity * COC_Cl).ToString();
                    label31.Text = (Cl_SO4 * COC_Cl).ToString();
                    label36.Text = (SiO2 * COC_Cl).ToString();
                    label39.Text = (Mg_SiO2 * COC_Cl).ToString();
                    if (COC_Al < 2)
                        MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
                    else
                    {
                        int Chart_N1 = Convert.ToInt32((COC_Cl - COC_Al) / Delta_Coc) + 1;
                        int Chart_N2 = Convert.ToInt32((COC_Al - 2) / Delta_Coc);
                        double[] X1_Axis = new double[Chart_N1];
                        double[] Y1_Axis = new double[Chart_N1];
                        double[] X2_Axis = new double[Chart_N2];
                        double[] Y2_Axis = new double[Chart_N2];
                        for (int i = 0; i < Chart_N1; i++)
                        {
                            X1_Axis[i] = COC_Cl - (Delta_Coc * i);
                            Y1_Axis[i] = Math.Round(Math.Pow(10, ((1 / Math.Log10(X1_Axis[i])) / (Math.Log10(Ca * Mg / (Cl + Na))) + 1)), 2);
                        }
                        for (int i = 0; i < Chart_N2; i++)
                        {
                            X2_Axis[i] = COC_Al - (Delta_Coc * i);
                            Y2_Axis[i] = Math.Round(Math.Pow(10, ((1 / Math.Log10(X2_Axis[i])) / (Math.Log10(Ca * Mg / (Cl + Na))) + 1)), 2);
                        }
                        Xmax_Axis[0] = COC_Al;
                        Ymax_Axis[0] = Math.Round(Math.Pow(10, ((1 / Math.Log10(Xmax_Axis[0])) / (Math.Log10(Ca * Mg / (Cl + Na))) + 1)), 2);
                        chart1.Series[0].Points.DataBindXY(Y1_Axis, X1_Axis);
                        //chart1.Series.Add("Series2");
                        chart1.Series[1].Points.DataBindXY(Y2_Axis, X2_Axis);
                        chart1.Series[2].Points.DataBindXY(Ymax_Axis, Xmax_Axis);
                    }
                }
            }
            //chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
        }
    }
}
