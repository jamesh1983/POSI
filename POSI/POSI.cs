using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSI
{
    class POSI
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

        const double 
            C_Ca = 2.45,//以碳酸钙计与以钙离子计换算常数
            C_Mg = 4.11,//以碳酸钙计与以镁离子计换算常数
            Delta_Coc = 0.01,//浓缩倍数增减步长
            Coc_Max = 15,//最大浓缩倍数
            Coc_Min = 3,//最小浓缩倍数
            
            Ca_Alkalinity_Limit = 1100,//钙硬+全碱限值
            Cl_SO4_Limit = 2500,//氯+硫酸根限值
            SiO2_Limit = 175,//硅酸限值
            Mg_SiO2_Limit = 50000;//镁+硅酸限值
        const int
            Round_Digital = 1;//计算后保留小数位数

        public bool 
            Alkalinity_Flag = true,//通过循环水碱度推算浓缩倍数
            Alkalinity_Input_Flag = false;//通过补充水碱度推算浓缩倍数

        public DataTable dt = new DataTable();//声明数据表,存放计算后数据

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

        public POSI(double Na_, double Mg_, double Ca_, double Cl_, double Cl_Max_, double Cond_, double SO4_ = 0, double SiO2_ = 0)
        {
            Na = Na_;
            Mg = Mg_;
            Ca = Ca_;
            Cl = Cl_;
            Clmax = Cl_Max_;
            Cond = Cond_;
            COC_Cl = Math.Round(Clmax / Cl, Round_Digital);
            if (COC_Cl > Coc_Max)
                COC_Cl = Coc_Max;
            if (COC_Cl < Coc_Min)
                System.Windows.Forms.MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
            Alkalinity = Alkalinity_Calculate(COC_Cl);
            Ca_Alkalinity = Ca * COC_Cl + Alkalinity;
            SO4 = SO4_;
            SiO2 = SiO2_;
            Cl_SO4 = (Cl + SO4) * COC_Cl;
            Mg_SiO2 = Mg * SiO2 * COC_Cl;
        }

        public double COC_Recalculate(double Alk_Max)
        {
            COC_Al = COC_Cl;
            while (Alk_Max > Alkalinity)
            {
                COC_Al = COC_Al - Delta_Coc;
                Alkalinity = Alkalinity_Calculate(COC_Al);
            }
            if (COC_Al < Coc_Min)
                System.Windows.Forms.MessageBox.Show("最大浓缩倍数过低，无法计算，请重新输入~");
            Ca_Alkalinity = Ca * COC_Al + Alkalinity;
            Cl_SO4 = (Cl + SO4) * COC_Al;
            Mg_SiO2 = Mg * SiO2 * COC_Al;
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
