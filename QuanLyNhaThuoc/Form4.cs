using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace QuanLyNhaThuoc
{
    public partial class Form4 : Form
    {
        private MySqlConnection connection;
        

        public Form4(MySqlConnection dbConnection)
        {
            InitializeComponent();
            connection = dbConnection;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            button3.BackColor = SystemColors.ButtonShadow;
            button2.BackColor = Color.FromArgb(192, 0, 192);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(connection);
            form2.ShowDialog();
            form2.BringToFront();
            button3.BackColor = Color.FromArgb(192, 0, 192);
            button2.BackColor = SystemColors.ButtonShadow;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 form4 = new Form5(connection);
            form4.ShowDialog();
        }
    }
}
