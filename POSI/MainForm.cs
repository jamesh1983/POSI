using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using POSI;
using DataTable = System.Data.DataTable;

namespace POSI
{
    public partial class MainForm : Form
    {
        public double Ca = 0.0;
        public double Mg = 0.0;
        public double Na = 100.0;
        public double Cl = 100.0;
        public double Clmax = 1000.0;
        public double Cond = 50.0;
        public double Alkalinity = 0.0;
        public double Alkalinity_Input = 0.0;
        public double COC_Cl = 10;
        public double COC_Al = 0.0;
        public double Alkalinity_Max = 0.0;

        public double Ca_Alkalinity = 0.0;
        public double Cl_SO4 = 0.0;
        public double SiO2 = 0.0;
        public double Mg_SiO2 = 0.0;

        public const double C_Ca = 2.45;
        public const double C_Mg = 4.11;
        public const double Delta_Coc = 0.1;
        public const double Coc_Max = 15;
        public const double Coc_Min = 3;
        public const int Round_Digital = 1;

        public bool Alkalinity_Flag = true;
        public bool Alkalinity_Input_Flag = false;

        private SettingForm settingForm = null;

        DataTable dt = new DataTable();

        public MainForm()
        {
            InitializeComponent();
            settingForm = new SettingForm(this);
            COC_Cl = Math.Round(Clmax / Cl, Round_Digital);
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
            dt.Columns.Add("浓缩倍数");
            dt.Columns.Add("循环水控制碱度");
            Hide();
            Show_settingform();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide(); 
            settingForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dt.Rows.Count == 0)
                MessageBox.Show("没有任何数据可以导入到Excel文件！");
            else
            {
                SaveFileDialog SaveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = Environment.SpecialFolder.DesktopDirectory.ToString(),
                    Filter = "Excel Files (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|All Files (*.*)|*.*",
                    RestoreDirectory = true
                };
                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //此处做你想做的事 ...=SaveFileDialog.FileName;
                    DataTable excelTable = dt;
                    Microsoft.Office.Interop.Excel.Application ExcelFile = new Microsoft.Office.Interop.Excel.Application();
                    try
                    {
                        ExcelFile.Visible = false;
                        Workbook WorkBook = ExcelFile.Workbooks.Add(true);
                        Worksheet WorkSheet = WorkBook.Worksheets[1] as Worksheet;
                        Range range;
                        ExcelFile.ScreenUpdating = false;
                        int colCount = excelTable.Columns.Count;
                        int rowCount = excelTable.Rows.Count;
                        object[,] objData = new object[rowCount + 1, colCount];
                        int size = excelTable.Columns.Count;
                        for (int i = 0; i < size; i++)
                            WorkSheet.Cells[1, 1 + i] = excelTable.Columns[i].ToString();
                        range = (Range)WorkSheet.get_Range((object)ExcelFile.Cells[1, 1], (object)ExcelFile.Cells[1, colCount]);
                        range.Interior.ColorIndex = 15;
                        range.Font.Bold = true;
                        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                            for (int colIndex = 0; colIndex < colCount; colIndex++)
                            {
                                objData[rowIndex, colIndex] = excelTable.Rows[rowIndex][colIndex].ToString();
                            }
                        range = (Range)WorkSheet.get_Range((object)ExcelFile.Cells[2, 1], (object)ExcelFile.Cells[rowCount + 1, colCount]);
                        range.Value2 = objData;
                        ExcelFile.DisplayAlerts = false;
                        ExcelFile.AlertBeforeOverwriting = false;
                        WorkBook.Saved = true;
                        WorkBook.SaveCopyAs(SaveFileDialog.FileName);
                        ExcelFile.Quit();
                        //app = null;
                        GC.Collect();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息");
                    }
                }
            }
            //Close();
            MessageBox.Show("导出Excel完成！", "提示信息");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Show_settingform()
        {
            DialogResult result = settingForm.ShowDialog();
            switch (result)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    //COCmax = Clmax / Cl;
                    //label_COCmax.Text = "最大浓缩倍数：" + COC_Cl.ToString();
                    
                    //Calculate_COC();
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

        public void Calculate_COC()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataMember = null;
            dt.Rows.Clear();
            label4.Text = Ca.ToString();
            label7.Text = Mg.ToString();
            label10.Text = (Ca + Mg).ToString();
            label16.Text = Cl.ToString();
            label19.Text = Na.ToString();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            COC_Cl = Math.Round(Clmax / Cl, Round_Digital);
            if (COC_Cl > Coc_Max)
                COC_Cl = Coc_Max;

            if (Alkalinity_Flag && !Alkalinity_Input_Flag)
            {
                double[] Xmax_Axis = new double[1];
                double[] Ymax_Axis = new double[1];
                Xmax_Axis[0] = COC_Cl;
                Ymax_Axis[0] = Math.Round(COC_Cl * Math.Pow(10, 1 / (Math.Log10(Xmax_Axis[0] / 1.5) * Math.Log10(Ca * Mg / (Cl + Na))) + 1), Round_Digital);
                
                int count = 0;
                label_COCmax.Text = "最大浓缩倍数：\n（受氯离子限制）";

                Alkalinity_Max = Math.Round(COC_Cl * Math.Pow(10, 1 / (Math.Log10(COC_Cl / 1.5) * Math.Log10(Ca * Mg / (Cl + Na))) + 1), Round_Digital);
                while (Alkalinity_Max < Alkalinity)
                {
                    COC_Cl = COC_Cl - Delta_Coc;
                    Alkalinity_Max = Math.Round(COC_Cl * Math.Pow(10, 1 / (Math.Log10(COC_Cl / 1.5) * Math.Log10(Ca * Mg / (Cl + Na))) + 1), Round_Digital);
                    count += 1;
                    label_COCmax.Text = "最大浓缩倍数：\n（受碱度限制）";
                } 

                if (COC_Cl < Coc_Min)
                    MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
                else
                {
                    COC_Cl = Math.Round(COC_Cl, Round_Digital);
                    Ca_Alkalinity = Math.Round(Ca * COC_Cl + Alkalinity_Max, Round_Digital);
                    label13.Text = Math.Round(Alkalinity_Max / COC_Cl, Round_Digital).ToString();
                    label21.Text = COC_Cl.ToString();
                    label22.Text = (Cond * COC_Cl).ToString();
                    label24.Text = Alkalinity_Max.ToString();
                    label33.Text = Ca_Alkalinity.ToString();
                    label31.Text = (Cl_SO4 * COC_Cl).ToString();
                    label36.Text = (SiO2 * COC_Cl).ToString();
                    label39.Text = (Mg_SiO2 * COC_Cl).ToString();

                    if (Ca_Alkalinity > 1100)
                        label33.BackColor = Color.Yellow;
                    if ((Cl_SO4 * COC_Cl) > 2500)
                        label31.BackColor = Color.Yellow;
                    if ((SiO2 * COC_Cl) > 175)
                        label36.BackColor = Color.Yellow;
                    if ((Mg_SiO2 * COC_Cl) > 50000)
                        label39.BackColor = Color.Yellow;

                    double[] Xmin_Axis = new double[1];
                    double[] Ymin_Axis = new double[1];
                    double[] X_Axis = new double[count];
                    double[] Y_Axis = new double[count];

                    Xmin_Axis[0] = COC_Cl;
                    Ymin_Axis[0] = Alkalinity_Max;
                    for (int i = 0; i < count; i++)
                    {
                        X_Axis[i] = COC_Cl + (Delta_Coc * i);
                        Y_Axis[i] = Math.Round(X_Axis[i] * Math.Pow(10, 1 / Math.Log10(X_Axis[i] / 1.5) / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                    }
                    int Chart_N = Convert.ToInt32((COC_Cl - Coc_Min) / Delta_Coc);
                    double[] X2_Axis = new double[Chart_N];
                    double[] Y2_Axis = new double[Chart_N];
                    for (int i = 0; i < Chart_N; i++)
                    {
                        X2_Axis[i] = COC_Cl - (Delta_Coc * i);
                        Y2_Axis[i] = Math.Round(X2_Axis[i] * Math.Pow(10, 1 / Math.Log10(X2_Axis[i] / 1.5) / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                    }

                    chart1.Series[0].Points.DataBindXY(Y_Axis, X_Axis);
                    chart1.Series[1].Points.DataBindXY(Y2_Axis, X2_Axis);
                    chart1.Series[2].Points.DataBindXY(Ymin_Axis, Xmin_Axis);
                    chart1.Series[3].Points.DataBindXY(Ymax_Axis, Xmax_Axis);
                    
                    for (int row = 0; row < count; row++) //填充行数据
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = X_Axis[row];
                        dr[1] = Y_Axis[row];
                        dt.Rows.Add(dr);
                    }
                    for (int row = 0; row < Chart_N; row++) //填充行数据
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = X2_Axis[row];
                        dr[1] = Y2_Axis[row];
                        dt.Rows.Add(dr);
                    }
                    dataGridView1.DataSource = dt;
                }
            }
            
            if (Alkalinity_Input_Flag && !Alkalinity_Flag)
            {
                COC_Al = Math.Pow(10, 1 / (Math.Log10(Ca * Mg / (Cl + Na)) * Math.Log10(Alkalinity_Input / 10)));
                COC_Al = Math.Round(COC_Al * 1.5, Round_Digital);
                if (COC_Al > Coc_Max)
                    COC_Al = Coc_Max;
                if (COC_Cl < Coc_Min)
                    MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
                else
                {
                    if (COC_Cl <= COC_Al)
                    {
                        Ca_Alkalinity = Ca + Alkalinity_Input;
                        label_COCmax.Text = "最大浓缩倍数：\n（受循环水氯离子限制）";
                        
                        label13.Text = Math.Round(COC_Cl * Alkalinity_Input, Round_Digital).ToString();
                        label21.Text = COC_Cl.ToString();
                        label22.Text = (Cond * COC_Cl).ToString();
                        label24.Text = (Alkalinity_Input * COC_Cl).ToString();
                        label33.Text = (Ca_Alkalinity * COC_Cl).ToString();
                        label31.Text = (Cl_SO4 * COC_Cl).ToString();
                        label36.Text = (SiO2 * COC_Cl).ToString();
                        label39.Text = (Mg_SiO2 * COC_Cl).ToString();
                        if (Ca_Alkalinity > 1100)
                            label33.BackColor = Color.Yellow;
                        if ((Cl_SO4 * COC_Cl) > 2500)
                            label31.BackColor = Color.Yellow;
                        if ((SiO2 * COC_Cl) > 175)
                            label36.BackColor = Color.Yellow;
                        if ((Mg_SiO2 * COC_Cl) > 50000)
                            label39.BackColor = Color.Yellow;
                        int Chart_N = Convert.ToInt32((COC_Cl - Coc_Min) / Delta_Coc);
                        double[] X_Axis = new double[Chart_N];
                        double[] Y_Axis = new double[Chart_N];
                        double[] Xmax_Axis = new double[1];
                        double[] Ymax_Axis = new double[1];
                        for (int i = 0; i < Chart_N; i++)
                        {
                            X_Axis[i] = COC_Cl - (Delta_Coc * i);
                            Y_Axis[i] = Math.Round(Math.Pow(10, 1 / Math.Log10(X_Axis[i] / 1.5) / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                        }
                        Xmax_Axis[0] = COC_Cl;
                        Ymax_Axis[0] = Math.Round(Math.Pow(10, 1 / Math.Log10(Xmax_Axis[0]) / 1.5 / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                        chart1.Series[0].Points.DataBindXY(Y_Axis, X_Axis);
                        chart1.Series[2].Points.DataBindXY(Ymax_Axis, Xmax_Axis);

                        for (int row = 0; row < Chart_N; row++) //填充行数据
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = X_Axis[row];
                            dr[1] = Y_Axis[row];
                            dt.Rows.Add(dr);
                        }
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        label_COCmax.Text = "最大浓缩倍数：\n（受循环水碱度限制）";
                        double[] Xmax_Axis = new double[1];
                        double[] Ymax_Axis = new double[1];
                        Ca_Alkalinity = Ca + Alkalinity_Input;

                        label21.Text = COC_Al.ToString();
                        label22.Text = (Cond * COC_Al).ToString();
                        label24.Text = (Alkalinity_Input * COC_Al).ToString();
                        label33.Text = (Ca_Alkalinity * COC_Al).ToString();
                        label31.Text = (Cl_SO4 * COC_Al).ToString();
                        label36.Text = (SiO2 * COC_Al).ToString();
                        label39.Text = (Mg_SiO2 * COC_Al).ToString();
                        if (Ca_Alkalinity > 1100)
                            label33.BackColor = Color.Yellow;
                        if ((Cl_SO4 * COC_Cl) > 2500)
                            label31.BackColor = Color.Yellow;
                        if ((SiO2 * COC_Cl) > 175)
                            label36.BackColor = Color.Yellow;
                        if ((Mg_SiO2 * COC_Cl) > 50000)
                            label39.BackColor = Color.Yellow;
                        if (COC_Al < Coc_Min)
                            MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
                        else
                        {
                            int Chart_N1 = Convert.ToInt32((COC_Cl - COC_Al) / Delta_Coc) + 1;
                            int Chart_N2 = Convert.ToInt32((COC_Al - Coc_Min) / Delta_Coc);
                            double[] X1_Axis = new double[Chart_N1];
                            double[] Y1_Axis = new double[Chart_N1];
                            double[] X2_Axis = new double[Chart_N2];
                            double[] Y2_Axis = new double[Chart_N2];
                            for (int i = 0; i < Chart_N1; i++)
                            {
                                X1_Axis[i] = COC_Cl - (Delta_Coc * i);
                                Y1_Axis[i] = Math.Round(Math.Pow(10, 1 / Math.Log10(X1_Axis[i] / 1.5) / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                            }
                            for (int i = 0; i < Chart_N2; i++)
                            {
                                X2_Axis[i] = COC_Al - (Delta_Coc * i);
                                Y2_Axis[i] = Math.Round(Math.Pow(10, 1 / Math.Log10(X2_Axis[i] / 1.5) / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                            }
                            Xmax_Axis[0] = COC_Al;
                            Ymax_Axis[0] = Math.Round(Math.Pow(10, 1 / Math.Log10(Xmax_Axis[0] / 1.5) / Math.Log10(Ca * Mg / (Cl + Na)) + 1), Round_Digital);
                            chart1.Series[0].Points.DataBindXY(Y1_Axis, X1_Axis);
                            //chart1.Series.Add("Series2");
                            chart1.Series[1].Points.DataBindXY(Y2_Axis, X2_Axis);
                            chart1.Series[2].Points.DataBindXY(Ymax_Axis, Xmax_Axis);

                            for (int row = 0; row < Chart_N1; row++) //填充行数据
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = X1_Axis[row];
                                dr[1] = Y1_Axis[row];
                                dt.Rows.Add(dr);
                            }
                            for (int row = 0; row < Chart_N2; row++) //填充行数据
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = X2_Axis[row];
                                dr[1] = Y2_Axis[row];
                                dt.Rows.Add(dr);
                            }
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
        }
    }
}
