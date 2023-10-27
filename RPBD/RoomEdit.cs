using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RPBD
{
    public partial class RoomEdit : Form
    {
        public RoomEdit()
        {
            InitializeComponent();
        }

        public void SetFields(string s1, string s2, DataTable d1, string s3, string s4)
        {
            textBox1.Text = s1;
            textBox2.Text = s2;
            comboBox2.DataSource = d1;
            comboBox2.DisplayMember = "Адрес";
            comboBox2.Text = s4;
            comboBox1.DataSource = d1;
            comboBox1.DisplayMember = "Имя";
            comboBox1.Text = s3;
        }

        public void SetFocus(string column)
        {
            switch (column)
            {
                case "Название здания":
                    {
                        comboBox1.Select();
                    }
                    break;
                case "Площадь помещения":
                    {
                        textBox2.Select();
                    }
                    break;
                case "Имя помещения":
                    {
                        textBox1.Select();
                    }
                    break;
                case "Адрес здания":
                    {
                        comboBox2.Select();
                    }
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RoomEdit_Load(object sender, EventArgs e)
        {
            
        }
    }
}
