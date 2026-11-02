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
    public partial class Dashboard : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TotalDogs()
        {
            string Cat = "Dog";
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from PetsTbl where Category ='"+ Cat + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            lblTotalDogs.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        private void TotalCats()
        {
            string Cat = "Cat";
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from PetsTbl where Category ='" + Cat + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            lblTotalCats.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        private void TotalBirds()
        {
            string Cat = "Bird";
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from PetsTbl where Category ='" + Cat + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            lblTotalBirds.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        private void TotalUsers()
        {
            
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(UserName) from UsersTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            lblTotalUsers.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        private void TotalCustomers()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(CustName) from CustomersTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            lblTotalCustomers.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }


        //private void SumAmount()


        private void Dashboard_Load(object sender, EventArgs e)
        {
            TotalDogs();
            TotalCats();
            TotalBirds();
            TotalUsers();
            TotalCustomers();
            GetTotalAmount();
           // SumAmount();
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
            Users_Login obj = new Users_Login();
            obj.Show();
            this.Hide();
        }
        private void GetTotalAmount()
        {
            try
            {
                Con.Open();
                string query = "SELECT SUM(TotalPrice) FROM TransactionTbl";
                SqlCommand cmd = new SqlCommand(query, Con);
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    lblTotalAmount.Text = result.ToString();
                }
                else
                {
                    lblTotalAmount.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == System.Data.ConnectionState.Open)
                    Con.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }

}

