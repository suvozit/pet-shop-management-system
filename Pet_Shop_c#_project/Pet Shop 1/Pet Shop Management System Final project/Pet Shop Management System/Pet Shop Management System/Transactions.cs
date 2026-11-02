using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management_System
{

    public partial class Transaction : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Transaction()
        {
            InitializeComponent();
        }

        private void DGVTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
        private void Transaction_Load(object sender, EventArgs e)
        {
            LoadTransaction();
        }

        private void LoadTransaction()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Con.ConnectionString))
                {
                    con.Open();
                    string query = "SELECT TransId, ProdName, Price, Quantity, CustName, TotalPrice, TransDate FROM TransactionTbl ORDER BY TransDate DESC";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        DGVTransactions.DataSource = dt;

                        // Optional: set column headers nicely
                        DGVTransactions.Columns["TransId"].HeaderText = "Transaction ID";
                        DGVTransactions.Columns["ProdName"].HeaderText = "Product Name";
                        DGVTransactions.Columns["Price"].HeaderText = "Price";
                        DGVTransactions.Columns["Quantity"].HeaderText = "Quantity";
                        DGVTransactions.Columns["CustName"].HeaderText = "Customer Name";
                        DGVTransactions.Columns["TotalPrice"].HeaderText = "Total Price";
                        DGVTransactions.Columns["TransDate"].HeaderText = "Date";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching transactions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            Billings obj = new Billings();
            obj.Show();
            this.Hide();
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
            Admin_login obj = new Admin_login();
            obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
