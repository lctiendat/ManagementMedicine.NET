using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLyNhaThuoc
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            ConnectToDatabase();

        }

        private void ConnectToDatabase()
        {
            string connectionString = "Server=localhost;Database=drugstore;User ID=root;Password=;SslMode=none;";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                //MessageBox.Show("Kết nối cơ sở dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                Application.Exit();

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (Login(username, password))
            {
                MessageBox.Show("Đăng nhập thành công!");
                // Chuyển đến form hoặc chức năng khác sau khi đăng nhập thành công
                //DataTable dataTableCategory = LoadDataCategory();

                // Mở Form2 và truyền dữ liệu
                //Form2 form2 = new Form2(connection);
                //form2.SetData(dataTableCategory);
                Form4 form4 = new Form4(connection);
                this.Hide();
                form4.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

        }

        private bool Login(string username, string password)
        {
            // Câu truy vấn SQL để kiểm tra thông tin đăng nhập
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";

            // Sử dụng MySqlCommand để thực hiện truy vấn
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0; // Trả về true nếu có người dùng tồn tại
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message);
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
