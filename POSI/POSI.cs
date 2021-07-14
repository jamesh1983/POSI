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
            Alkalinity,//碱度
            Alkalinity_Input,//补充水碱度
            Alkalinity_Max,//循环水碱度限值
            COC_Cl,//按氯计算最大浓缩倍数
            COC_Al,//按碱度计算最大浓缩倍数
            Ca_Alkalinity,//钙硬+全碱
            Cl_SO4,//氯+硫酸根
            SiO2,//硅酸
            Mg_SiO2;//镁+硅酸

        const double 
            C_Ca = 2.45,//以碳酸钙计与以钙离子计换算常数
            C_Mg = 4.11,//以碳酸钙计与以镁离子计换算常数
            Delta_Coc = 0.01;//浓缩倍数增减步长

        public bool 
            Alkalinity_Flag = true,//通过循环水碱度推算浓缩倍数
            Alkalinity_Input_Flag = false;//通过补充水碱度推算浓缩倍数

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
