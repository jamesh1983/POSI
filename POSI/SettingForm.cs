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

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.Cl = Convert.ToDouble(Value_Cl_.Text);
            MainForm.Clmax = Convert.ToDouble(Value_Cl_Limit.Text);
            MainForm.Ca = Convert.ToDouble(Value_Ca_CaCO3.Text);
            MainForm.Mg = Convert.ToDouble(Value_Mg_CaCO3.Text);
            MainForm.Na = Convert.ToDouble(Value_Na_.Text);
            MainForm.Cond = Convert.ToDouble(Value_Cond.Text);
            Close();
            MainForm.Show();
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
            if (Value_Ca_CaCO3.Text == "")
                Value_Ca_CaCO3.Text = "0.0";
            Value_Ca_.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Ca_CaCO3.Text) / Convert.ToDouble(MainForm.C_Ca), 3));
            Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
            Value_Ca_Alkalinity.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Total_Alkalinity.Text));
            MainForm.Ca = Convert.ToDouble(Value_Ca_CaCO3.Text);
        }

        private void Value_Mg_CaCO3_TextChanged(object sender, EventArgs e)
        {
            if (Value_Mg_CaCO3.Text == "")
                Value_Mg_CaCO3.Text = "0.0";
            Value_Mg_.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) / Convert.ToDouble(MainForm.C_Mg), 3));
            Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
            Value_Mg_SiO2.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) * Convert.ToDouble(Value_SiO2.Text), 3));
            MainForm.Mg = Convert.ToDouble(Value_Mg_CaCO3.Text);
        }

        private void Value_Ca__TextChanged(object sender, EventArgs e)
        {
            if (Value_Ca_.Text == "")
                Value_Ca_.Text = "0.0";
            Value_Ca_CaCO3.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Ca_.Text) * Convert.ToDouble(MainForm.C_Ca), 3));
            Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
            Value_Ca_Alkalinity.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Total_Alkalinity.Text));
            MainForm.Ca = Convert.ToDouble(Value_Ca_CaCO3.Text);
        }

        private void Value_Mg__TextChanged(object sender, EventArgs e)
        {
            if (Value_Mg_.Text == "")
                Value_Mg_.Text = "0.0";
            Value_Mg_CaCO3.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_.Text) * Convert.ToDouble(MainForm.C_Mg), 3));
            Value_Total_Hardness.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Mg_CaCO3.Text));
            Value_Mg_SiO2.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) * Convert.ToDouble(Value_SiO2.Text), 3));
            MainForm.Mg = Convert.ToDouble(Value_Mg_CaCO3.Text);
        }

        private void Value_Total_Alkalinity_TextChanged(object sender, EventArgs e)
        {
            if (Value_Total_Alkalinity.Text == "")
                Value_Total_Alkalinity.Text = "0.0";
            Value_Ca_Alkalinity.Text = Convert.ToString(Convert.ToDouble(Value_Ca_CaCO3.Text) + Convert.ToDouble(Value_Total_Alkalinity.Text));
            MainForm.Alkalinity = Convert.ToDouble(Value_Total_Alkalinity.Text);
        }

        private void Value_SO4__TextChanged(object sender, EventArgs e)
        {
            if (Value_SO4_.Text == "")
                Value_SO4_.Text = "0.0";
            Value_Cl_SO4_.Text = Convert.ToString(Convert.ToDouble(Value_Cl_.Text) + Convert.ToDouble(Value_SO4_.Text));
        }

        private void Value_SiO2_TextChanged(object sender, EventArgs e)
        {
            if (Value_SiO2.Text == "")
                Value_SiO2.Text = "0.0";
            Value_Mg_SiO2.Text = Convert.ToString(Math.Round(Convert.ToDouble(Value_Mg_CaCO3.Text) * Convert.ToDouble(Value_SiO2.Text), 3));
        }

        private void Value_Cl__TextChanged(object sender, EventArgs e)
        {
            if (Value_Cl_.Text == "")
                Value_Cl_.Text = "0.0";
            Value_Cl_SO4_.Text = Convert.ToString(Convert.ToDouble(Value_Cl_.Text) + Convert.ToDouble(Value_SO4_.Text));
            MainForm.COCmax = Convert.ToDouble(Value_Cl_Limit.Text) / Convert.ToDouble(Value_Cl_.Text);
        }

        private void Value_Na__TextChanged(object sender, EventArgs e)
        {
            if (Value_Na_.Text == "")
                Value_Na_.Text = "0.0";
            MainForm.Na = Convert.ToDouble(Value_Na_.Text);
        }

        private void Value_Cond_TextChanged(object sender, EventArgs e)
        {
            if (Value_Cond.Text == "")
                Value_Cond.Text = "0.0";
            MainForm.Cond = Convert.ToDouble(Value_Cond.Text);
        }

        private void Value_Cl_Limit_TextChanged(object sender, EventArgs e)
        {
            if (Value_Cl_Limit.Text == "")
                Value_Cl_Limit.Text = "0.0";
            MainForm.COCmax = Convert.ToDouble(Value_Cl_Limit.Text) / Convert.ToDouble(Value_Cl_.Text);
        }
    }
}
