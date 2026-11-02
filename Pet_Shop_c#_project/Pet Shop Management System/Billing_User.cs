using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pet_Shop_Management_System
{
    public partial class Billing_User : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Billing_User()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int Key = 0;
        int Stock = 0;

        private void Reset()
        {
            txtCustomerName.Clear();
            txtProductName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void DisplayPets()
        {
            using (SqlConnection con = new SqlConnection(Con.ConnectionString))
            using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM PetsTbl", con))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                DGVPets.DataSource = dt;
            }
        }

        private void DisplayPetFood()
        {
            using (SqlConnection con = new SqlConnection(Con.ConnectionString))
            using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM PetFoodsTbl", con))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                DGVPetFood.DataSource = dt;
            }
        }

        private void FillCustomerId()
        {
            using (SqlConnection con = new SqlConnection(Con.ConnectionString))
            using (SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId FROM CustomersTbl", con))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                CBCustomerId.ValueMember = "CustId";
                CBCustomerId.DataSource = dt;
                CBCustomerId.SelectedIndex = -1;
            }
        }

        private void GetCustomerName()
        {
            if (CBCustomerId.SelectedValue == null)
            {
                txtCustomerName.Text = "";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(Con.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT CustName FROM CustomersTbl WHERE CustId = @CustId", con))
                {
                    cmd.Parameters.AddWithValue("@CustId", CBCustomerId.SelectedValue);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                        txtCustomerName.Text = reader["CustName"].ToString();
                    else
                        txtCustomerName.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CBCustomerId_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustomerName();
        }

        private void Billings_Load(object sender, EventArgs e)
        {
            DGVCustomerBill.Columns.Add("ProductName", "Product Name");
            DGVCustomerBill.Columns.Add("Price", "Price");
            DGVCustomerBill.Columns.Add("Quantity", "Quantity");
            DGVCustomerBill.Columns.Add("CustomerName", "Customer Name");
            DGVCustomerBill.Columns.Add("Total", "Total");

            lblTotalAmount.Text = "0";

            DisplayPetFood();
            DisplayPets();
            FillCustomerId();
        }

        private void DGVPets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectItemFromGrid(DGVPets);
        }

        private void DGVPetFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectItemFromGrid(DGVPetFood);
        }

        private void SelectItemFromGrid(DataGridView grid)
        {
            if (grid.SelectedRows.Count == 0) return;

            txtProductName.Text = grid.SelectedRows[0].Cells[1].Value.ToString();
            txtPrice.Text = grid.SelectedRows[0].Cells[4].Value.ToString();

            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                Key = 0;
                Stock = 0;
            }
            else
            {
                Key = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
                Stock = Convert.ToInt32(grid.SelectedRows[0].Cells[3].Value);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            string tableName = "";
            int selectedKey = 0;
            int selectedStock = 0;

            if (DGVPets.SelectedRows.Count > 0)
            {
                tableName = "PetsTbl";
                selectedKey = Convert.ToInt32(DGVPets.SelectedRows[0].Cells[0].Value);
                selectedStock = Convert.ToInt32(DGVPets.SelectedRows[0].Cells[3].Value);
            }
            else if (DGVPetFood.SelectedRows.Count > 0)
            {
                tableName = "PetFoodsTbl";
                selectedKey = Convert.ToInt32(DGVPetFood.SelectedRows[0].Cells[0].Value);
                selectedStock = Convert.ToInt32(DGVPetFood.SelectedRows[0].Cells[3].Value);
            }
            else
            {
                MessageBox.Show("Please select a Pet or Pet Food to add!");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || !int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Invalid price or quantity!");
                return;
            }

            if (quantity > selectedStock)
            {
                MessageBox.Show("Not enough stock!");
                return;
            }

            decimal total = price * quantity;
            DGVCustomerBill.Rows.Add(txtProductName.Text, price, quantity, txtCustomerName.Text, total);

            // Database insert & stock update
            try
            {
                using (SqlConnection con = new SqlConnection(Con.ConnectionString))
                {
                    con.Open();

                    string insertQuery = @"INSERT INTO TransactionTbl 
                                           (ProdName, Price, Quantity, CustName, TotalPrice, TransDate)
                                           VALUES (@ProdName, @Price, @Quantity, @CustName, @TotalPrice, @TransDate)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ProdName", txtProductName.Text);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CustName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@TotalPrice", total);
                        cmd.Parameters.AddWithValue("@TransDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    string idColumn = tableName == "PetsTbl" ? "PId" : "FoodId";
                    string updateQuery = $"UPDATE {tableName} SET Quantity = @NewQuantity WHERE {idColumn} = @Id";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@NewQuantity", selectedStock - quantity);
                        cmd.Parameters.AddWithValue("@Id", selectedKey);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }

            // Refresh grids
            if (tableName == "PetsTbl") DisplayPets();
            else DisplayPetFood();

            txtProductName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Users_Login obj = new Users_Login();
            obj.Show();
            this.Hide();

        }

        private void btnPets_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pets Information Not Available for USers");
        }

        private void btnPetFood_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pet Food Information Not Available for USers");
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Users Information Not Available for USers");
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Customers Information Not Available for USers");
        }

        private void btnBillings_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Transaction Information Not Available for USers");
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dashboard Information Not Available for USers");

        }

        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PRINTED");
            DGVCustomerBill.Rows.Clear();
            Reset();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
