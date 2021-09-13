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
        //public double Ca = 0.0;
        //public double Mg = 0.0;
        //public double Na = 100.0;
        //public double Cl = 100.0;
        //public double Clmax = 1000.0;
        //public double Cond = 50.0;
        //public double Alkalinity = 0.0;
        //public double Alkalinity_Input = 0.0;
        //public double COC_Cl = 10;
        //public double COC_Al = 0.0;
        //public double Alkalinity_Max = 0.0;

        //public double Ca_Alkalinity = 0.0;
        //public double Cl_SO4 = 0.0;
        //public double SiO2 = 0.0;
        //public double Mg_SiO2 = 0.0;

        //public const double C_Ca = 2.45;
        //public const double C_Mg = 4.11;
        //public const double Delta_Coc = 0.1;
        //public const double Coc_Max = 15;
        //public const double Coc_Min = 3;
        //public const int Round_Digital = 1;

        //public bool Alkalinity_Flag = true;
        //public bool Alkalinity_Input_Flag = false;

        public POSI posi = null;

        private SettingForm settingForm = null;
        //DataTable dt = null;

        

        public MainForm()
        {
            InitializeComponent();
            settingForm = new SettingForm(this);
            //COC_Cl = Math.Round(Clmax / Cl, Round_Digital);
            //label4.Text = Ca.ToString();
            //label7.Text = Mg.ToString();
            //label10.Text = (Ca + Mg).ToString();
            //label13.Text = Alkalinity.ToString();
            //label16.Text = Cl.ToString();
            //label19.Text = Na.ToString();
            //label21.Text = COC_Cl.ToString();
            //label22.Text = (Cond * COC_Cl).ToString();
            //label24.Text = (Alkalinity * COC_Cl).ToString();
            //label33.Text = (Ca_Alkalinity * COC_Cl).ToString();
            //label31.Text = (Cl_SO4 * COC_Cl).ToString();
            //label36.Text = (SiO2 * COC_Cl).ToString();
            //label39.Text = (Mg_SiO2 * COC_Cl).ToString();
            posi = new POSI();
            //posi.dt = new DataTable();
            posi.dt.Columns.Add("循环水控制碱度");
            posi.dt.Columns.Add("浓缩倍数");
            
            //Hide();
            Show_settingform();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide(); 
            settingForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (posi.dt.Rows.Count == 0)
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
                    DataRow dr = posi.dt.NewRow();
                    dr[0] = label2.Text;
                    posi.dt.Rows.Add(dr);

                    dr = posi.dt.NewRow();
                    dr[0] = label3.Text;
                    dr[1] = label4.Text;
                    posi.dt.Rows.Add(dr);

                    dr = posi.dt.NewRow();
                    dr[0] = label8.Text;
                    dr[1] = label7.Text;
                    posi.dt.Rows.Add(dr);

                    dr = posi.dt.NewRow();
                    dr[0] = label20.Text;
                    dr[1] = label19.Text;
                    posi.dt.Rows.Add(dr);

                    dr = posi.dt.NewRow();
                    dr[0] = label17.Text;
                    dr[1] = label16.Text;
                    posi.dt.Rows.Add(dr);

                    dr = posi.dt.NewRow();
                    dr[0] = label14.Text;
                    dr[1] = label13.Text;
                    posi.dt.Rows.Add(dr);

                    DataTable excelTable = posi.dt;
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
                    MessageBox.Show("导出Excel完成！", "提示信息");
                }
                else
                    MessageBox.Show("未能导出Excel文件！", "提示信息");
            }
            //Close();
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
            posi.dt.Rows.Clear();
            label4.Text = posi.Ca.ToString();
            label7.Text = posi.Mg.ToString();
            label10.Text = (posi.Ca + posi.Mg).ToString();
            label16.Text = posi.Cl.ToString();
            label19.Text = posi.Na.ToString();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            //COC_Cl = Math.Round(Clmax / Cl, Round_Digital);
            //if (posi.COC_Cl > posi.Coc_Max)
            //    posi.COC_Cl = posi.Coc_Max;

            posi.Data_Initialize();
            if (posi.COC_Al == posi.COC_Cl)
                label_COCmax.Text = "最大浓缩倍数：\n（受氯离子限制）";
            else
                label_COCmax.Text = "最大浓缩倍数：\n（受碱度限制）";
            //if (posi.Alkalinity_Flag && !posi.Alkalinity_Input_Flag)
            //{
            //    double[] Xmax_Axis = new double[1];
            //    double[] Ymax_Axis = new double[1];
            //    Xmax_Axis[0] = posi.COC_Cl;
            //    Ymax_Axis[0] = posi.Alkalinity_Calculate(posi.COC_Cl);
            //Math.Round(posi.COC_Cl * Math.Pow(10, 1 / (Math.Log10(posi.COC_Cl / 1.5) * Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na))) + 1), POSI.Round_Digital);

            //int count = 0;
            //    label_COCmax.Text = "最大浓缩倍数：\n（受氯离子限制）";

            //posi.Alkalinity_Max = Ymax_Axis[0];

            //posi.COC_Al = posi.COC_Recalculate(posi.Alkalinity_Max);
            //Math.Round(posi.COC_Cl * Math.Pow(10, 1 / (Math.Log10(posi.COC_Cl / 1.5) * Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na))) + 1), POSI.Round_Digital);
            //posi.COC_Al = posi.COC_Cl;
            //while (posi.Alkalinity_Max < posi.Alkalinity)
            //{
            //    posi.COC_Al = posi.COC_Al - POSI.Delta_Coc;
            //    posi.Alkalinity_Max = Math.Round(posi.COC_Al * Math.Pow(10, 1 / (Math.Log10(posi.COC_Al / 1.5) * Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na))) + 1), POSI.Round_Digital);
            //    count += 1;
            //    label_COCmax.Text = "最大浓缩倍数：\n（受碱度限制）";
            //} 
            //if (posi.Ca_Alkalinity_Flag)
            //    label33.BackColor = Color.Yellow;
            //else
            //    label33.BackColor = SystemColors.Control;
            //if (posi.Cl_SO4_Flag)
            //    label31.BackColor = Color.Yellow;
            //else
            //    label31.BackColor = SystemColors.Control;
            //if (posi.SiO2_Flag)
            //    label36.BackColor = Color.Yellow;
            //else
            //    label36.BackColor = SystemColors.Control;
            //if (posi.Mg_SiO2_Flag)
            //    label39.BackColor = Color.Yellow;
            //else
            //    label39.BackColor = SystemColors.Control;
            //if (posi.COC_Al < POSI.Coc_Min)
            //    MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
            //else
            //{
            //posi.COC_Al = Math.Round(posi.COC_Al, POSI.Round_Digital);
            //posi.Ca_Alkalinity = Math.Round((posi.Ca + posi.Alkalinity_Input) * posi.COC_Al, POSI.Round_Digital);
            //label13.Text = Math.Round(posi.Alkalinity_Max / posi.COC_Al, POSI.Round_Digital).ToString();
            //label21.Text = posi.COC_Al.ToString();
            //label22.Text = (posi.Cond * posi.COC_Al).ToString();
            //label24.Text = posi.Alkalinity_Max.ToString();
            //label33.Text = posi.Ca_Alkalinity.ToString();
            //label31.Text = (posi.Cl_SO4 * posi.COC_Al).ToString();
            //label36.Text = (posi.SiO2 * posi.COC_Al).ToString();
            //label39.Text = (posi.Mg_SiO2 * posi.COC_Al).ToString();

            //if (posi.Ca_Alkalinity_Flag)
            //    label33.BackColor = Color.Yellow;
            //else
            //    label33.BackColor = SystemColors.Control;
            //if (posi.Cl_SO4_Flag)
            //    label31.BackColor = Color.Yellow;
            //else
            //    label31.BackColor = SystemColors.Control;
            //if (posi.SiO2_Flag)
            //    label36.BackColor = Color.Yellow;
            //else
            //    label36.BackColor = SystemColors.Control;
            //if (posi.Mg_SiO2_Flag)
            //    label39.BackColor = Color.Yellow;
            //else
            //    label39.BackColor = SystemColors.Control;

            //double[] Xmin_Axis = new double[1];
            //double[] Ymin_Axis = new double[1];
            //double[] X_Axis = new double[posi.count];
            //double[] Y_Axis = new double[posi.count];
            //Xmin_Axis[0] = posi.COC_Al;
            //Ymin_Axis[0] = Math.Round(posi.COC_Al * Math.Pow(10, 1 / (Math.Log10(posi.COC_Al / 1.5) * Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na))) + 1), POSI.Round_Digital);

            //for (int i = 0; i < posi.count; i++)
            //{
            //    X_Axis[i] = posi.COC_Al + (POSI.Delta_Coc * i);
            //    Y_Axis[i] = Math.Round(X_Axis[i] * Math.Pow(10, 1 / Math.Log10(X_Axis[i] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
            //    if (i > 0)
            //        if (Y_Axis[i] > Y_Axis[i - 1])
            //            Y_Axis[i] = Y_Axis[i - 1];
            //}

            //int Chart_N = Convert.ToInt32((posi.COC_Al - POSI.Coc_Min) / POSI.Delta_Coc);
            //double[] X2_Axis = new double[Chart_N];
            //double[] Y2_Axis = new double[Chart_N];
            //for (int i = 0; i < Chart_N; i++)
            //{
            //    X2_Axis[i] = POSI.Coc_Min + (POSI.Delta_Coc * i);
            //    Y2_Axis[i] = Math.Round(X2_Axis[i] * Math.Pow(10, 1 / Math.Log10(X2_Axis[i] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
            //    if ((i > 0) && (Chart_N > 0))
            //        if (Y2_Axis[i] > Y2_Axis[i - 1])
            //            Y2_Axis[i] = Y2_Axis[i - 1];
            //}
            label13.Text = Math.Round(posi.Alkalinity_Max / posi.COC_Al, POSI.Round_Digital).ToString();
            label21.Text = posi.COC_Al.ToString();
            label22.Text = Math.Round((posi.Cond * posi.COC_Al), POSI.Round_Digital).ToString();
            label24.Text = posi.Alkalinity_Max.ToString();
            label33.Text = posi.Ca_Alkalinity.ToString();
            label31.Text = Math.Round((posi.Cl_SO4 * posi.COC_Al), POSI.Round_Digital).ToString();
            label36.Text = Math.Round((posi.SiO2 * posi.COC_Al), POSI.Round_Digital).ToString();
            label39.Text = Math.Round((posi.Mg_SiO2 * posi.COC_Al), POSI.Round_Digital).ToString();
            chart1.Series[0].Points.DataBindXY(posi.Y_Axis, posi.X_Axis);
            chart1.Series[1].Points.DataBindXY(posi.Y2_Axis, posi.X2_Axis);
            chart1.Series[2].Points.DataBindXY(posi.Ymin_Axis, posi.Xmin_Axis);
                    
                    if (posi.Chart_N != 0)
                    {
                        for (int row = 0; row < posi.Chart_N; row++) //填充行数据
                        {
                            DataRow dr = posi.dt.NewRow();
                            dr[0] = posi.Y2_Axis[row];
                            dr[1] = posi.X2_Axis[row];
                            posi.dt.Rows.Add(dr);
                        }
                    }
                    if (posi.count != 0)
                    {
                        chart1.Series[3].Points.DataBindXY(posi.Ymax_Axis, posi.Xmax_Axis);
                        for (int row = 0; row < posi.count; row++) //填充行数据
                        {
                            DataRow dr = posi.dt.NewRow();

                            dr[0] = posi.Y_Axis[row];
                            dr[1] = posi.X_Axis[row];

                            posi.dt.Rows.Add(dr);
                        }
                    }
                    dataGridView1.DataSource = posi.dt;
                //}
            //}
            
            if (posi.Alkalinity_Input_Flag && !posi.Alkalinity_Flag)
            {
                posi.COC_Al = Math.Pow(10, 1 / (Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) * Math.Log10(posi.Alkalinity_Input / 10)));
                posi.COC_Al = Math.Round(posi.COC_Al * 1.5, POSI.Round_Digital);
                if (posi.COC_Al > POSI.Coc_Max)
                    posi.COC_Al = POSI.Coc_Max;
                if (posi.COC_Cl < POSI.Coc_Min)
                    MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
                else
                {
                    if (posi.COC_Cl <= posi.COC_Al)
                    {
                        posi.Ca_Alkalinity = Math.Round((posi.Ca + posi.Alkalinity_Input) * posi.COC_Cl, POSI.Round_Digital);
                        label_COCmax.Text = "最大浓缩倍数：\n（受循环水氯离子限制）";
                        
                        label13.Text = Math.Round(posi.Alkalinity_Input, POSI.Round_Digital).ToString();
                        label21.Text = posi.COC_Cl.ToString();
                        label22.Text = (posi.Cond * posi.COC_Cl).ToString();
                        label24.Text = (posi.Alkalinity_Input * posi.COC_Cl).ToString();
                        label33.Text = posi.Ca_Alkalinity.ToString();
                        label31.Text = (posi.Cl_SO4 * posi.COC_Cl).ToString();
                        label36.Text = (posi.SiO2 * posi.COC_Cl).ToString();
                        label39.Text = (posi.Mg_SiO2 * posi.COC_Cl).ToString();
                        if (posi.Ca_Alkalinity > 1100)
                            label33.BackColor = Color.Yellow;
                        if ((posi.Cl_SO4 * posi.COC_Cl) > 2500)
                            label31.BackColor = Color.Yellow;
                        if ((posi.SiO2 * posi.COC_Cl) > 175)
                            label36.BackColor = Color.Yellow;
                        if ((posi.Mg_SiO2 * posi.COC_Cl) > 50000)
                            label39.BackColor = Color.Yellow;
                        int Chart_N = Convert.ToInt32((posi.COC_Cl - POSI.Coc_Min) / POSI.Delta_Coc) + 1;
                        double[] X_Axis = new double[Chart_N];
                        double[] Y_Axis = new double[Chart_N];
                        double[] Xmax_Axis = new double[1];
                        double[] Ymax_Axis = new double[1];
                        int min = 0;
                        //X_Axis[0] = COC_Cl - (Delta_Coc * Chart_N);
                        for (int i = 0; i < Chart_N; i++)
                        {
                            X_Axis[i] = POSI.Coc_Min + (POSI.Delta_Coc * i);
                            Y_Axis[i] = Math.Round(X_Axis[i] * Math.Pow(10, 1 / Math.Log10(X_Axis[i] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
                            if(i > 0)
                            {
                                if (Y_Axis[i] > Y_Axis[i - 1])
                                    Y_Axis[i] = Y_Axis[i - 1];
                                else
                                    min = min + 1;
                            }
                        }
                        Xmax_Axis[0] = posi.COC_Cl;
                        Ymax_Axis[0] = Y_Axis[min];
                        chart1.Series[1].Points.DataBindXY(Y_Axis, X_Axis);
                        chart1.Series[2].Points.DataBindXY(Ymax_Axis, Xmax_Axis);
                        for (int row = 0; row < Chart_N; row++) //填充行数据
                        {
                            DataRow dr = posi.dt.NewRow();

                            dr[0] = Y_Axis[row];
                            dr[1] = X_Axis[row];

                            posi.dt.Rows.Add(dr);
                        }
                        dataGridView1.DataSource = posi.dt;
                    }
                    else
                    {
                        label_COCmax.Text = "最大浓缩倍数：\n（受循环水碱度限制）";
                        double[] Xmax_Axis = new double[1];
                        double[] Ymax_Axis = new double[1];
                        double[] Xmin_Axis = new double[1];
                        double[] Ymin_Axis = new double[1];
                        posi.Ca_Alkalinity = Math.Round((posi.Ca + posi.Alkalinity_Input) * posi.COC_Al, POSI.Round_Digital);
                        label13.Text = Math.Round(posi.Alkalinity_Input, POSI.Round_Digital).ToString();
                        label21.Text = posi.COC_Al.ToString();
                        label22.Text = (posi.Cond * posi.COC_Al).ToString();
                        label24.Text = (posi.Alkalinity_Input * posi.COC_Al).ToString();
                        label33.Text = posi.Ca_Alkalinity.ToString();
                        label31.Text = (posi.Cl_SO4 * posi.COC_Al).ToString();
                        label36.Text = (posi.SiO2 * posi.COC_Al).ToString();
                        label39.Text = (posi.Mg_SiO2 * posi.COC_Al).ToString();
                        if (posi.Ca_Alkalinity > 1100)
                            label33.BackColor = Color.Yellow;
                        else
                            label33.BackColor = SystemColors.Control;
                        if ((posi.Cl_SO4 * posi.COC_Al) > 2500)
                            label31.BackColor = Color.Yellow;
                        else
                            label31.BackColor = SystemColors.Control;
                        if ((posi.SiO2 * posi.COC_Al) > 175)
                            label36.BackColor = Color.Yellow;
                        else
                            label36.BackColor = SystemColors.Control;
                        if ((posi.Mg_SiO2 * posi.COC_Al) > 50000)
                            label39.BackColor = Color.Yellow;
                        else
                            label39.BackColor = SystemColors.Control;
                        if (posi.COC_Al < POSI.Coc_Min)
                            MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
                        else
                        {
                            int Chart_N1 = Convert.ToInt32((posi.COC_Cl - posi.COC_Al) / POSI.Delta_Coc) + 1;
                            int Chart_N2 = Convert.ToInt32((posi.COC_Al - POSI.Coc_Min) / POSI.Delta_Coc) + 1;
                            double[] X1_Axis = new double[Chart_N2];
                            double[] Y1_Axis = new double[Chart_N2];
                            double[] X2_Axis = new double[Chart_N1];
                            double[] Y2_Axis = new double[Chart_N1];
                            int min = 0;
                            for (int i = 0; i < Chart_N2; i++)
                            {
                                X1_Axis[i] = POSI.Coc_Min + (POSI.Delta_Coc * i);
                                Y1_Axis[i] = Math.Round(X1_Axis[i] * Math.Pow(10, 1 / Math.Log10(X1_Axis[i] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
                                if (i > 0)
                                {
                                    if (Y1_Axis[i] > Y1_Axis[i - 1])
                                        Y1_Axis[i] = Y1_Axis[i - 1];
                                    else
                                        min = min + 1;
                                }
                            }
                            min = 0;
                            for (int i = 0; i < Chart_N1; i++)
                            {
                                X2_Axis[i] = posi.COC_Al + (POSI.Delta_Coc * i);
                                Y2_Axis[i] = Math.Round(X2_Axis[i] * Math.Pow(10, 1 / Math.Log10(X2_Axis[i] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
                                if (i > 0)
                                {
                                    if (Y2_Axis[i] > Y2_Axis[i - 1])
                                        Y2_Axis[i] = Y2_Axis[i - 1];
                                    else
                                        min = min + 1;
                                }
                            }
                            Xmax_Axis[0] = posi.COC_Cl;
                            Ymax_Axis[0] = Y2_Axis[min];

                            Xmin_Axis[0] = posi.COC_Al;
                            Ymin_Axis[0] = Math.Round(Xmin_Axis[0] * Math.Pow(10, 1 / Math.Log10(Xmin_Axis[0] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
                            chart1.Series[0].Points.DataBindXY(Y2_Axis, X2_Axis);
                            chart1.Series[1].Points.DataBindXY(Y1_Axis, X1_Axis);
                            if (Chart_N2 != 0)
                                chart1.Series[2].Points.DataBindXY(Ymin_Axis, Xmin_Axis);
                            //chart1.Series[3].Points.DataBindXY(Ymax_Axis, Xmax_Axis);
                            if (Chart_N1 != 0)
                                chart1.Series[3].Points.DataBindXY(Ymax_Axis, Xmax_Axis);
                            for (int row = 0; row < Chart_N2; row++) //填充行数据
                            {
                                DataRow dr = posi.dt.NewRow();

                                dr[0] = Y1_Axis[row];
                                dr[1] = X1_Axis[row];

                                posi.dt.Rows.Add(dr);
                            }
                            for (int row = 1; row < Chart_N1; row++) //填充行数据
                            {
                                DataRow dr = posi.dt.NewRow();
                                dr[0] = Y2_Axis[row];
                                dr[1] = X2_Axis[row];
                                posi.dt.Rows.Add(dr);
                            }
                            dataGridView1.DataSource = posi.dt;
                        }
                    }
                }
            }
        }
    }
}
