using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pet_Shop_Management_System
{
    public partial class Billings : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Billings()
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
            txtCustomerName.Text = "";
            txtProductName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
        }

        private void DisplayPets()

        {
            Con.Open();
            string query = " select * from PetsTbl ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVPets.DataSource = ds.Tables[0];
            Con.Close();

        }

        private void DisplayPetFood()

        {
            Con.Open();
            string query = " select * from PetFoodsTbl ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVPetFood.DataSource = ds.Tables[0];
            Con.Close();

        }

        private void FillCustomerId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT CustId FROM CustomersTbl", Con);
            SqlDataReader Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId", typeof(int));
            dt.Load(Rdr);
            Con.Close();

            CBCustomerId.ValueMember = "CustId";
            CBCustomerId.DataSource = dt;

            // Add a blank (no selection) item at start
            CBCustomerId.SelectedIndex = -1;
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
                Con.Open();
                string query = "SELECT CustName FROM CustomersTbl WHERE CustId = @CustId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@CustId", CBCustomerId.SelectedValue);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtCustomerName.Text = reader["CustName"].ToString();
                }
                else
                {
                    txtCustomerName.Text = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Con.Close();
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
            FillCustomerId(); // new  
        }
        private void DGVPets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProductName.Text= DGVPets.SelectedRows[0].Cells[1].Value.ToString();
            txtPrice.Text= DGVPets.SelectedRows[0].Cells[4].Value.ToString();
            if(txtProductName.Text == "")
            {
                Key = 0;
                Stock = 0;
            }
            else
            {
                Key = Convert.ToInt32(DGVPets.SelectedRows[0].Cells[0].Value.ToString());
               Stock = Convert.ToInt32(DGVPets.SelectedRows[0].Cells[3].Value.ToString());

            }

        }

        private void DGVPetFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProductName.Text = DGVPetFood.SelectedRows[0].Cells[1].Value.ToString();
            txtPrice.Text = DGVPetFood.SelectedRows[0].Cells[4].Value.ToString();
            if (txtProductName.Text == "")
            {
                Key = 0;
                Stock = 0;
            }
            else
            {
                Key = Convert.ToInt32(DGVPetFood.SelectedRows[0].Cells[0].Value.ToString());
                Stock = Convert.ToInt32(DGVPetFood.SelectedRows[0].Cells[3].Value.ToString());

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            // 1️⃣ Determine which grid has a selection and get selected key/stock
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
                MessageBox.Show("Please select a Pet or Pet Food to add!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Get input values
            string productName = txtProductName.Text.Trim();
            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Product name is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid price!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Invalid quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (quantity > selectedStock)
            {
                MessageBox.Show("Not enough stock available!", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = price * quantity;

            // 3️⃣ Add to DGVCustomerBill
            DGVCustomerBill.Rows.Add(productName, price, quantity, txtCustomerName.Text, total);

            // 4️⃣ Insert into database and update stock
            try
            {
                using (SqlConnection con = new SqlConnection(Con.ConnectionString))
                {
                    con.Open();

                    // Insert transaction
                    string insertQuery = @"INSERT INTO TransactionTbl 
                                   (ProdName, Price, Quantity, CustName, TotalPrice, TransDate)
                                   VALUES (@ProdName, @Price, @Quantity, @CustName, @TotalPrice, @TransDate)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ProdName", productName);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CustName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@TotalPrice", total);
                        cmd.Parameters.AddWithValue("@TransDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    // Update stock
                    int newQuantity = selectedStock - quantity;
                    string idColumn = tableName == "PetsTbl" ? "PId" : "FoodId";

                    string updateQuery = $"UPDATE {tableName} SET Quantity = @NewQuantity WHERE {idColumn} = @Id";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con))
                    {
                        cmdUpdate.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        cmdUpdate.Parameters.AddWithValue("@Id", selectedKey);
                        cmdUpdate.ExecuteNonQuery();
                    }
                } // Connection automatically closed here
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 5️⃣ Refresh the correct grid
            if (tableName == "PetsTbl")
                DisplayPets();
            else
            {
                DisplayPetFood();
                DGVPetFood.ClearSelection(); // Prevent old selection from staying
            }

            // 6️⃣ Update grand total
            decimal grandTotal = 0;
            foreach (DataGridViewRow row in DGVCustomerBill.Rows)
            {
                if (row.Cells["Total"].Value != null)
                    grandTotal += Convert.ToDecimal(row.Cells["Total"].Value);
            }
            lblTotalAmount.Text = grandTotal.ToString("0.00");

            // 7️⃣ Reset input fields
            txtProductName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            Key = 0;
            Stock = 0;
        }

        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PRINTED");
            DGVCustomerBill.Rows.Clear();
            Reset();
        }

        private void btnPets_Click(object sender, EventArgs e)
        {
            Pets obj = new Pets();
            obj.Show();
            this.Hide();
        }

        private void btnPetFood_Click(object sender, EventArgs e)
        {
            Pet_Food obj = new Pet_Food();
            obj.Show();
            this.Hide();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show(); 
            this.Hide();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
            obj.Show();
            this.Hide();
        }

        private void btnBillings_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            Transaction obj = new Transaction();
            obj.Show();
            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard obj = new Dashboard();
            obj.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Users_Login obj = new Users_Login();
            obj.Show();
            this.Hide();
        }
    }
}
