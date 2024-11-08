using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class Form2 : Form
    {
        private MySqlConnection connection;

        public Form2(MySqlConnection dbConnection)
        {
            InitializeComponent();
            connection = dbConnection;
            LoadCategories();
            txtCategoryId.Visible = false;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "Edit";
            editButtonColumn.HeaderText = "Chỉnh sửa";
            editButtonColumn.Text = "Chỉnh sửa";
            editButtonColumn.UseColumnTextForButtonValue = true;
            dataGridViewCategories.Columns.Add(editButtonColumn);

            // Thêm cột nút "Xóa"
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.Name = "Delete";
            deleteButtonColumn.HeaderText = "Xóa";
            deleteButtonColumn.Text = "Xóa";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            dataGridViewCategories.Columns.Add(deleteButtonColumn);

            dataGridViewCategories.CellClick += dataGridViewCategories_CellClick;

        }
        private void LoadCategories()
        {
            string query = "SELECT * FROM categories";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
                dataGridViewCategories.DataSource = dataTable; // Assumes you have a DataGridView named dataGridViewCategories
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        // Add a new category
        private void AddCategory(string name)
        {
            string query = "INSERT INTO categories (name, created_at) VALUES (@name, NOW())";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm danh mục thành công!");
                LoadCategories(); // Refresh data
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm danh mục: " + ex.Message);
            }
        }

        // Update existing category
        private void UpdateCategory(int id, string name)
        {
            string query = "UPDATE categories SET name = @name WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật danh mục thành công!");
                LoadCategories(); // Refresh data
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật danh mục: " + ex.Message);
            }
        }

        // Delete category
        private void DeleteCategory(int id)
        {
            string query = "DELETE FROM categories WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa danh mục thành công!");
                LoadCategories(); // Refresh data
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa danh mục: " + ex.Message);
            }
        }

        // Button click event handlers
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            if (!string.IsNullOrWhiteSpace(name))
            {
                AddCategory(name);
                txtCategoryName.Clear();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.");
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCategoryId.Text, out int id) && !string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                string name = txtCategoryName.Text;
                UpdateCategory(id, name); // Gọi hàm cập nhật
                txtCategoryId.Clear();
                txtCategoryName.Clear();

                // Đổi lại trạng thái của các nút sau khi cập nhật xong
                titleCategory.Text = "Add Category";
                button5.Text = "Add";
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên danh mục và chọn một danh mục hợp lệ để cập nhật.");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCategoryId.Text, out int id))
            {
                DeleteCategory(id);
                txtCategoryId.Clear();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập ID hợp lệ.");
            }
        }

        private void dataGridViewCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng nhấn vào cột "Chỉnh sửa"
            if (e.ColumnIndex == dataGridViewCategories.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                titleCategory.Text = "Update Category";
                


                int id = Convert.ToInt32(dataGridViewCategories.Rows[e.RowIndex].Cells["id"].Value);
                string name = dataGridViewCategories.Rows[e.RowIndex].Cells["name"].Value.ToString();

                // Đặt giá trị vào TextBox để người dùng chỉnh sửa
                txtCategoryId.Text = id.ToString();
                txtCategoryName.Text = name;
                button5.Text = "Update";
                button5.Click -= btnAddCategory_Click;
                button5.Click += btnUpdateCategory_Click;
            }

            // Kiểm tra nếu người dùng nhấn vào cột "Xóa"
            if (e.ColumnIndex == dataGridViewCategories.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridViewCategories.Rows[e.RowIndex].Cells["id"].Value);

                // Xác nhận xóa
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?", "Xác nhận Xóa", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteCategory(id);
                    LoadCategories(); // Tải lại dữ liệu sau khi xóa
                }
            }
        }

        private void txtCategoryId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
