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
    public partial class SettingForm : Form
    {
        private MainForm MainForm;
        public SettingForm(MainForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
            mainForm.Hide();
        }

        private bool Check_All_Data()
        {
            if ((Value_Ca_CaCO3.Text == "") || (Value_Ca_CaCO3.Text == "0.0"))
            {
                MessageBox.Show("缺少补水Ca离子参数");
                Value_Ca_CaCO3.Focus();
                return false;
            }
            else
            {
                if ((Value_Mg_CaCO3.Text == "") || (Value_Mg_CaCO3.Text == "0.0"))
                {
                    MessageBox.Show("缺少补水Mg离子参数");
                    Value_Mg_CaCO3.Focus();
                    return false;
                }
                else
                {
                    if ((Value_Cl_Limit.Text == "") || (Value_Cl_Limit.Text == "0.0"))
                    {
                        MessageBox.Show("缺少补水Cl离子参数");
                        radioButton4.Checked = true;
                        Value_Cl_Limit.Focus();
                        return false;
                    }
                    else
                    {
                        if ((Value_Cl_.Text == "") || (Value_Cl_.Text == "0.0"))
                        {
                            MessageBox.Show("缺少循环水Cl限值参数");
                            Value_Cl_.Focus();
                            return false;
                        }
                        else
                        {
                            if ((Value_Na_.Text == "") || (Value_Na_.Text == "0.0"))
                            {
                                MessageBox.Show("缺少补水Na离子参数");
                                Value_Na_.Focus();
                                return false;
                            }
                            else
                            {
                                if ((Value_Cond.Text == "") || (Value_Cond.Text == "0.0"))
                                {
                                    MessageBox.Show("缺少补水电导率参数");
                                    Value_Cond.Focus();
                                    return false;
                                }
                                else
                                {
                                    if (checkBox3.Checked)
                                    {
                                        if ((Value_Total_Alkalinity.Text == "") || (Value_Total_Alkalinity.Text == "0.0"))
                                        {
                                            MessageBox.Show("缺少循环水碱度限值参数");
                                            Value_Total_Alkalinity.Focus();
                                            return false;
                                        }
                                        else if (checkBox4.Checked)
                                        {
                                            if ((Value_Alkalinity_Input.Text == "") || (Value_Alkalinity_Input.Text == "0.0"))
                                            {
                                                MessageBox.Show("缺少补水碱度参数");
                                                Value_Alkalinity_Input.Focus();
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (checkBox4.Checked)
                                        {
                                            if ((Value_Alkalinity_Input.Text == "") || (Value_Alkalinity_Input.Text == "0.0"))
                                            {
                                                MessageBox.Show("缺少补水碱度参数");
                                                Value_Alkalinity_Input.Focus();
                                                return false;
                                            }
                                        }
                                        MessageBox.Show("缺少循环水碱度限值或补水碱度参数");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox1.CheckState)
            {
                case CheckState.Unchecked:
                    checkBox2.Checked = true;
                    Value_Ca_.Enabled = true;
                    Value_Mg_.Enabled = true;
                    Value_Ca_CaCO3.Enabled = false;
                    Value_Mg_CaCO3.Enabled = false;
                    break;
                case CheckState.Checked:
                    checkBox2.Checked = false;
                    Value_Ca_.Enabled = false;
                    Value_Mg_.Enabled = false;
                    Value_Ca_CaCO3.Enabled = true;
                    Value_Mg_CaCO3.Enabled = true;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox2.CheckState)
            {
                case CheckState.Unchecked:
                    checkBox1.Checked = true;
                    Value_Ca_.Enabled = false;
                    Value_Mg_.Enabled = false;
                    Value_Ca_CaCO3.Enabled = true;
                    Value_Mg_CaCO3.Enabled = true;
                    break;
                case CheckState.Checked:
                    checkBox1.Checked = false;
                    Value_Ca_.Enabled = true;
                    Value_Mg_.Enabled = true;
                    Value_Ca_CaCO3.Enabled = false;
                    Value_Mg_CaCO3.Enabled = false;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox3.CheckState)
            {
                case CheckState.Unchecked:
                    //checkBox4.Checked = true;
                    //Value_Total_Alkalinity.Text = "";
                    Value_Total_Alkalinity.Enabled = false;
                    //Value_Alkalinity_Input.Enabled = true;
                    //Value_Alkalinity_Input.Text = "";
                    MainForm.Alkalinity_Flag = false;
                    break;
                case CheckState.Checked:
                    //checkBox4.Checked = false;
                    //Value_Total_Alkalinity.Text = "";
                    Value_Total_Alkalinity.Enabled = true;
                    //Value_Alkalinity_Input.Enabled = false;
                    //Value_Alkalinity_Input.Text = "";
                    MainForm.Alkalinity_Flag = true;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox4.CheckState)
            {
                case CheckState.Unchecked:
                    //checkBox3.Checked = true;
                    //Value_Total_Alkalinity.Text = "";
                    //Value_Total_Alkalinity.Enabled = true;
                    Value_Alkalinity_Input.Enabled = false;
                    Value_Alkalinity_Input.Text = "";
                    MainForm.Alkalinity_Input_Flag = false;
                    break;
                case CheckState.Checked:
                    //checkBox3.Checked = false;
                    //Value_Total_Alkalinity.Text = "";
                    //Value_Total_Alkalinity.Enabled = false;
                    Value_Alkalinity_Input.Enabled = true;
                    Value_Alkalinity_Input.Text = "";
                    MainForm.Alkalinity_Input_Flag = true;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }
        }

        private bool Check_Num(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
            {
                e.Handled = true;
                return false;
            }
            Type senderType = sender.GetType();
            if (senderType.Name == "TextBox")
            {
                TextBox textbox = sender as TextBox;
                if (textbox.Text.Contains("."))
                    if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8)
                    {
                        e.Handled = true;
                        return false;
                    }
            }
            return true;
        }

        private void Value_Ca_CaCO3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Mg_CaCO3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Ca__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Mg__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Total_Alkalinity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Alkalinity_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_SO4__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_SiO2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Cl__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Na__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Cond_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Cl_Limit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Check_Num(sender, e))
                e.Handled = true;
        }

        private void Value_Ca_CaCO3_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (Value_Ca_CaCO3.Text != "")
                    Value_Ca_.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Ca_CaCO3.Text) / Convert.ToDouble(MainForm.C_Ca), 2));
                if ((Value_Mg_CaCO3.Text != "") && (Value_Ca_CaCO3.Text != ""))
                    Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
                if ((Value_Total_Alkalinity.Text != "") && (Value_Ca_CaCO3.Text != ""))
                    Value_Ca_Alkalinity.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Total_Alkalinity.Text));
                //MainForm.Ca = Convert.ToDouble(Value_Ca_CaCO3.Text);
            }
        }

        private void Value_Mg_CaCO3_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (Value_Mg_CaCO3.Text != "")
                    Value_Mg_.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) / Convert.ToDouble(MainForm.C_Mg), 2));
                if ((Value_Ca_CaCO3.Text != "") && (Value_Mg_CaCO3.Text != ""))
                    Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
                if ((Value_SiO2.Text != "") && (Value_Mg_CaCO3.Text != ""))
                    Value_Mg_SiO2.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) * Convert.ToDouble(Value_SiO2.Text), 2));
                //MainForm.Mg = Convert.ToDouble(Value_Mg_CaCO3.Text);
            }
        }

        private void Value_Ca__TextChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (Value_Ca_.Text != "")
                    Value_Ca_CaCO3.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Ca_.Text) * Convert.ToDouble(MainForm.C_Ca), 3));
                if ((Value_Mg_CaCO3.Text != "") && (Value_Mg_CaCO3.Text != ""))
                    Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
                if ((Value_Total_Alkalinity.Text != "") && (Value_Ca_.Text != ""))
                    Value_Ca_Alkalinity.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Total_Alkalinity.Text));
                //MainForm.Ca = Convert.ToDouble(Value_Ca_CaCO3.Text);
            }
        }

        private void Value_Mg__TextChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (Value_Mg_.Text != "")
                    Value_Mg_CaCO3.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_.Text) * Convert.ToDouble(MainForm.C_Mg), 3));
                if ((Value_Ca_CaCO3.Text != "") && (Value_Mg_CaCO3.Text != ""))
                    Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
                if (Value_SiO2.Text != "")
                    Value_Mg_SiO2.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) * Convert.ToDouble(Value_SiO2.Text), 3));
                //MainForm.Mg = Convert.ToDouble(Value_Mg_CaCO3.Text);
            }
        }

        private void Value_Total_Alkalinity_TextChanged(object sender, EventArgs e)
        {
            if (Value_Total_Alkalinity.Text != "")
                MainForm.Alkalinity = Convert.ToDouble(Value_Total_Alkalinity.Text);
        }

        private void Value_Alkalinity_Input_TextChanged(object sender, EventArgs e)
        {
            if ((Value_Alkalinity_Input.Text != "") && (Value_Ca_CaCO3.Text != ""))
                Value_Ca_Alkalinity.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Alkalinity_Input.Text));
            MainForm.Alkalinity_Input = Convert.ToDouble(Value_Alkalinity_Input.Text);
        }

        private void Value_SO4__TextChanged(object sender, EventArgs e)
        {
            if ((Value_SO4_.Text != "") && (Value_Cl_.Text != ""))
                Value_Cl_SO4_.Text = Convert.ToString(Convert.ToDouble(Value_Cl_.Text) + Convert.ToDouble(Value_SO4_.Text));
        }

        private void Value_SiO2_TextChanged(object sender, EventArgs e)
        {
            if ((Value_SiO2.Text != "") && (Value_Mg_CaCO3.Text != ""))
                Value_Mg_SiO2.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) * Convert.ToDouble(Value_SiO2.Text), 3));
        }

        private void Value_Cl__TextChanged(object sender, EventArgs e)
        {
            if ((Value_Cl_.Text != "") && (Value_SO4_.Text != ""))
                Value_Cl_SO4_.Text = Convert.ToString(Convert.ToDouble(Value_Cl_.Text) + Convert.ToDouble(Value_SO4_.Text));
            //MainForm.Cl = Convert.ToDouble(Value_Cl_.Text);
            //MainForm.COC_Cl = Convert.ToDouble(Value_Cl_Limit.Text) / Convert.ToDouble(Value_Cl_.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Value_Cl_Limit.Text = "2000";
            Value_Cl_Limit.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Value_Cl_Limit.Text = "1000";
            Value_Cl_Limit.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Value_Cl_Limit.Text = "700";
            Value_Cl_Limit.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Value_Cl_Limit.Enabled = true;
            Value_Cl_Limit.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Check_All_Data())
            {
                MainForm.Cl = Convert.ToDouble(Value_Cl_.Text);
                MainForm.Clmax = Convert.ToDouble(Value_Cl_Limit.Text);
                MainForm.Ca = Convert.ToDouble(Value_Ca_CaCO3.Text);
                MainForm.Mg = Convert.ToDouble(Value_Mg_CaCO3.Text);
                MainForm.Na = Convert.ToDouble(Value_Na_.Text);
                MainForm.Cond = Convert.ToDouble(Value_Cond.Text);
                //MainForm.Alkalinity = Convert.ToDouble(Value_Total_Alkalinity.Text);

                //MainForm.Ca_Alkalinity = Convert.ToDouble(Value_Ca_Alkalinity.Text);
                MainForm.Cl_SO4 = Convert.ToDouble(Value_Cl_SO4_.Text);
                MainForm.SiO2 = Convert.ToDouble(Value_SiO2.Text);
                MainForm.Mg_SiO2 = Convert.ToDouble(Value_Mg_SiO2.Text);
                //MainForm.Alkalinity_Input = Convert.ToDouble(Value_Alkalinity_Input.Text);
                Hide();
                MainForm.Show();
                DialogResult = DialogResult.OK;
                MainForm.Calculate_COC();
            }
            //DialogResult = DialogResult.Cancel;
        }

    }
}
