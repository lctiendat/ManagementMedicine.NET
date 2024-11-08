using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace QuanLyNhaThuoc
{
    public partial class Form5 : Form
    {
        private MySqlConnection connection;

        public Form5(MySqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            LoadCategories();
            LoadProducts();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "Edit";
            editButtonColumn.HeaderText = "Chỉnh sửa";
            editButtonColumn.Text = "Chỉnh sửa";
            editButtonColumn.UseColumnTextForButtonValue = true;
            dataGridViewProducts.Columns.Add(editButtonColumn);

            // Thêm cột nút "Xóa"
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.Name = "Delete";
            deleteButtonColumn.HeaderText = "Xóa";
            deleteButtonColumn.Text = "Xóa";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            dataGridViewProducts.Columns.Add(deleteButtonColumn);

            dataGridViewProducts.CellClick += dataGridViewCategories_CellClick;
            
        }

        private void LoadCategories()
        {
            string query = "SELECT id, name FROM categories";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
                cboCategory.DataSource = dataTable;
                cboCategory.DisplayMember = "name";
                cboCategory.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            string query = "SELECT products.id, products.name, categories.name as category, products.unit, products.price, products.stock_quantity, products.expiry_date " +
                           "FROM products " +
                           "JOIN categories ON products.categoryID = categories.id";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
                dataGridViewProducts.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            //txtProductId.Clear();
            txtProductName.Clear();
            txtUnit.Clear();
            txtPurchasePrice.Clear();
            txtStockQuantity.Clear();
            txtDescription.Clear();

            cboCategory.SelectedIndex = -1;
        }

        private bool ValidateInputs()
        {
            // Kiểm tra tên sản phẩm
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }

            // Kiểm tra danh mục
            if (cboCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategory.Focus();
                return false;
            }

            // Kiểm tra đơn vị tính
            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn vị tính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnit.Focus();
                return false;
            }

            // Kiểm tra giá nhập
            if (!decimal.TryParse(txtPurchasePrice.Text, out decimal purchasePrice) || purchasePrice < 0)
            {
                MessageBox.Show("Vui lòng nhập giá nhập hợp lệ (số dương).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPurchasePrice.Focus();
                return false;
            }


            // Kiểm tra số lượng tồn kho
            if (!int.TryParse(txtStockQuantity.Text, out int stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng tồn kho hợp lệ (số nguyên dương).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStockQuantity.Focus();
                return false;
            }

            // Kiểm tra ngày hết hạn
            if (dtpExpiryDate.Value < DateTime.Now)
            {
                MessageBox.Show("Ngày hết hạn không thể nhỏ hơn ngày hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpExpiryDate.Focus();
                return false;
            }

            return true;
        }
        private void AddProduct()
        {
            string query = "INSERT INTO products (name, categoryID, unit, price, stock_quantity,description, expiry_date) " +
                           "VALUES (@name, @categoryId, @unit, @price, @stock_quantity,@description ,@expiry_date)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", txtProductName.Text);
            cmd.Parameters.AddWithValue("@categoryId", cboCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@unit", txtUnit.Text);
            cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPurchasePrice.Text));
            cmd.Parameters.AddWithValue("@stock_quantity", Convert.ToInt32(txtStockQuantity.Text));
            cmd.Parameters.AddWithValue("@expiry_date", dtpExpiryDate.Value);
            cmd.Parameters.AddWithValue("@description", txtDescription.Text);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm sản phẩm thành công!");
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                AddProduct();
            }
        }

        private void dataGridViewCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng nhấn vào cột "Chỉnh sửa"
            if (e.ColumnIndex == dataGridViewProducts.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                titleCategory.Text = "Update Category";



                int id = Convert.ToInt32(dataGridViewProducts.Rows[e.RowIndex].Cells["id"].Value);
                string name = dataGridViewProducts.Rows[e.RowIndex].Cells["name"].Value.ToString();

                // Đặt giá trị vào TextBox để người dùng chỉnh sửa
                //txtCategoryId.Text = id.ToString();
                //txtCategoryName.Text = name;
                //button5.Text = "Update";
                //button5.Click -= btnAddCategory_Click;
                //button5.Click += btnUpdateCategory_Click;
            }

            // Kiểm tra nếu người dùng nhấn vào cột "Xóa"
            if (e.ColumnIndex == dataGridViewProducts.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridViewProducts.Rows[e.RowIndex].Cells["id"].Value);

                // Xác nhận xóa
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?", "Xác nhận Xóa", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    //DeleteCategory(id);
                    LoadCategories(); // Tải lại dữ liệu sau khi xóa
                }
            }
        }


    }
}
