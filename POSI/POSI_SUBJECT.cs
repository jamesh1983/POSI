using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSI
{
    public class POSI
    {
        public double 
            Ca,//钙
            Mg,//镁
            Na,//钠
            Cl,//氯
            Clmax,//氯限值
            Cond,//电导率
            Alkalinity,//当前循环水碱度
            Alkalinity_Input,//补充水碱度
            Alkalinity_Max,//循环水碱度限值
            COC_Cl,//按氯计算最大浓缩倍数
            COC_Al,//按碱度计算最大浓缩倍数
            Ca_Alkalinity,//钙硬+全碱
            SO4,//硫酸根
            Cl_SO4,//氯+硫酸根
            SiO2,//硅酸
            Mg_SiO2;//镁+硅酸

        public int count = 0;
        public int Chart_N = 0;

        public const double 
            C_Ca = 2.45,//以碳酸钙计与以钙离子计换算常数
            C_Mg = 4.11,//以碳酸钙计与以镁离子计换算常数
            Delta_Coc = 0.01,//浓缩倍数增减步长
            Coc_Max = 15,//最大浓缩倍数
            Coc_Min = 3,//最小浓缩倍数
            
            Ca_Alkalinity_Limit = 1100,//钙硬+全碱限值
            Cl_SO4_Limit = 2500,//氯+硫酸根限值
            SiO2_Limit = 175,//硅酸限值
            Mg_SiO2_Limit = 50000;//镁+硅酸限值
        public const int
            Round_Digital = 1;//计算后保留小数位数

        public bool 
            Alkalinity_Flag = true,//通过循环水碱度推算浓缩倍数
            Alkalinity_Input_Flag = false,//通过补充水碱度推算浓缩倍数

            Ca_Alkalinity_Flag = false,
            Cl_SO4_Flag = false,
            SiO2_Flag = false,
            Mg_SiO2_Flag = false;

        public DataTable dt = null;

        public double[] Xmax_Axis = new double[1];
        public double[] Ymax_Axis = new double[1];
        public double[] Xmin_Axis = new double[1];
        public double[] Ymin_Axis = new double[1];
        public double[] X_Axis = null;// new double[count];
        public double[] Y_Axis = null;// new double[count];
        public double[] X2_Axis = null;// new double[Chart_N];
        public double[] Y2_Axis = null;// new double[Chart_N];

        public POSI()
        {
            dt = new DataTable();//声明数据表,存放计算后数据
        }

        public bool Check_Ca_Alkalinity_Limit(double Ca_Alkalinity)
        {
            if (Ca_Alkalinity > Ca_Alkalinity_Limit)
                return true;
            else
                return false;
        }

        public bool Check_Cl_SO4_Limit(double Cl_SO4)
        {
            if (Cl_SO4 > Cl_SO4_Limit)
                return true;
            else
                return false;
        }

        public bool Check_SiO2_Limit(double SiO2)
        {
            if (SiO2 > SiO2_Limit)
                return true;
            else
                return false;
        }

        public bool Check_Mg_SiO2_Limit(double Mg_SiO2)
        {
            if (Mg_SiO2 > Mg_SiO2_Limit)
                return true;
            else
                return false;
        }

        public double COC_Calculate(double Alkalinity)
        {
            double result = Math.Pow(10, 1 / (Math.Log10(Ca * Mg / (Cl + Na)) * Math.Log10(Alkalinity / 10)));//按POSI计算浓缩倍数
            result = result * 1.5;//按经验值（1.5）换算浓缩倍数
            result = Math.Round(result, Round_Digital);//保留设定保留小数
            return result;
        }

        public double Alkalinity_Calculate(double COC)
        {
            double result  = Math.Pow(10, 1 / (Math.Log10(COC / 1.5) * Math.Log10(Ca * Mg / (Cl + Na))) + 1);//按POSI计算补充水碱度
            result = COC * result;//折算至循环水碱度
            result = Math.Round(result, Round_Digital);//保留设定保留小数
            return result;
        }

        public void Data_Initialize()//double Na_, double Mg_, double Ca_, double Cl_, double Cl_Max_, double Cond_, double SO4_ = 0, double SiO2_ = 0
        {
            COC_Cl = Math.Round(Clmax / Cl, Round_Digital);
            if (COC_Cl > Coc_Max)
                COC_Cl = Coc_Max;
            if (COC_Cl < Coc_Min)
                System.Windows.Forms.MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
            Alkalinity = Alkalinity_Calculate(COC_Cl);

            if (Alkalinity_Flag && !Alkalinity_Input_Flag)
            {
                Xmax_Axis[0] = COC_Cl;
                Ymax_Axis[0] = Alkalinity;

                COC_Al = COC_Recalculate(Alkalinity_Max);

                Xmin_Axis[0] = COC_Al;
                Alkalinity = Alkalinity_Calculate(COC_Al);
                Ymin_Axis[0] = Alkalinity;

                X_Axis = new double[count];
                Y_Axis = new double[count];

                for (int i = 0; i < count; i++)
                {
                    X_Axis[i] = COC_Al + (Delta_Coc * i);
                    Y_Axis[i] = Alkalinity_Calculate(X_Axis[i]);
                    //Math.Round(X_Axis[i] * Math.Pow(10, 1 / Math.Log10(X_Axis[i] / 1.5) / Math.Log10(posi.Ca * posi.Mg / (posi.Cl + posi.Na)) + 1), POSI.Round_Digital);
                    if (i > 0)
                        if (Y_Axis[i] > Y_Axis[i - 1])
                            Y_Axis[i] = Y_Axis[i - 1];
                }

                Chart_N = Chart_N = Convert.ToInt32((COC_Al - Coc_Min) / Delta_Coc);
                X2_Axis = new double[Chart_N];
                Y2_Axis = new double[Chart_N];

                for (int i = 0; i < Chart_N; i++)
                {
                    X2_Axis[i] = Coc_Min + (Delta_Coc * i);
                    Y2_Axis[i] = Alkalinity_Calculate(X2_Axis[i]);
                    if ((i > 0) && (Chart_N > 0))
                        if (Y2_Axis[i] > Y2_Axis[i - 1])
                            Y2_Axis[i] = Y2_Axis[i - 1];
                }

                Ca_Alkalinity_Flag = false;
                Cl_SO4_Flag = false;
                SiO2_Flag = false;
                Mg_SiO2_Flag = false;
                Ca_Alkalinity = Math.Round((Ca * COC_Cl + Alkalinity), Round_Digital);
                if (Ca_Alkalinity > Ca_Alkalinity_Limit)
                    Ca_Alkalinity_Flag = true;

                Cl_SO4 = Math.Round(((Cl + SO4) * COC_Cl), Round_Digital);
                if (Cl_SO4 > Cl_SO4_Limit)
                    Cl_SO4_Flag = true;

                if ((SiO2 * COC_Cl) > SiO2_Limit)
                    SiO2_Flag = true;

                Mg_SiO2 = Math.Round((Mg * SiO2 * COC_Cl), Round_Digital);
                if (Mg_SiO2 > Mg_SiO2_Limit)
                    Mg_SiO2_Flag = true;
            }
            if (Alkalinity_Input_Flag && !Alkalinity_Flag)
            {

            }
        }

        public double COC_Recalculate(double Alk_Max)
        {
            COC_Al = COC_Cl;
            while (Alk_Max > Alkalinity)
            {
                COC_Al = COC_Al - Delta_Coc;
                Alkalinity = Alkalinity_Calculate(COC_Al);
                count += 1;
            }
            if (COC_Al < Coc_Min)
                System.Windows.Forms.MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
            else
            {
                Ca_Alkalinity_Flag = false;
                Cl_SO4_Flag = false;
                SiO2_Flag = false;
                Mg_SiO2_Flag = false;
                Ca_Alkalinity = Math.Round((Ca * COC_Al + Alkalinity), Round_Digital);
                Cl_SO4 = Math.Round(((Cl + SO4) * COC_Al), Round_Digital);
                Mg_SiO2 = Math.Round((Mg * SiO2 * COC_Al), Round_Digital);
                if (Ca_Alkalinity > Ca_Alkalinity_Limit)
                    Ca_Alkalinity_Flag = true;
                if (Cl_SO4 > Cl_SO4_Limit)
                    Cl_SO4_Flag = true;
                if ((SiO2 * COC_Cl) > SiO2_Limit)
                    SiO2_Flag = true;
                if (Mg_SiO2 > Mg_SiO2_Limit)
                    Mg_SiO2_Flag = true;
            }

            return COC_Al;
        }

        //public DataTable Func_Incert_Table(string[] columns, double[][] data)
        //{
        //    int Cloumn_N = columns.Length;
        //    //int C_N = data[][].Length;
        //    int Row_N = data.Length;
        //    if (C_N <= Cloumn_N)
        //    {
        //        DataTable dt = new DataTable();
        //        for (int i = 0; i < Cloumn_N; i++) //填充行字段名
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[i] = columns[i];
        //            dt.Rows.Add(dr);
        //        }
        //        for (int i = 0; i < C_N; i++) //填充行数据
        //        {
        //            DataRow dr = dt.NewRow();
        //            for(int j = 0; j < Row_N; j++)
        //                dr[j] = data[i][j];
        //            dt.Rows.Add(dr);
        //        }
        //        return dt;
        //    }
        //    return null;
        //}
    }
}
