using System;
using System.Collections.Generic;
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
    }
}
