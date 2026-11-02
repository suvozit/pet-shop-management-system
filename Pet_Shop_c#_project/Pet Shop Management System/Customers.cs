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
using System.Windows.Input;

namespace Pet_Shop_Management_System
{
    public partial class Customers : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Customers()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == "" || CBGender.SelectedIndex == -1 || txtPhoneNo.Text == "" || txtFullAddress.Text == "")
            {
                MessageBox.Show("Missing Information ! ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CustomersTbl(CustName,Gender,PhoneNo,Address) values(@Cname,@Gender,@Phone,@Add)", Con);
                    cmd.Parameters.AddWithValue("@Cname", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@Gender", CBGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Phone", txtPhoneNo.Text);
                    cmd.Parameters.AddWithValue("@Add", txtFullAddress.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Added Successfully");
                    Con.Close();
                    DisplayCustomers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void Reset()
        {
            txtCustomerName.Text = "";
            CBGender.SelectedItem = -1;
            txtPhoneNo.Text = "";
            txtFullAddress.Text = "";
        }

        private void DisplayCustomers()

        {
            Con.Open();
            string query = " select * from CustomersTbl ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVCustomers.DataSource = ds.Tables[0];
            Con.Close();

        }
        int Key = 0;
        private void DGVCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerName.Text = DGVCustomers.SelectedRows[0].Cells[1].Value.ToString();
            CBGender.SelectedItem = DGVCustomers.SelectedRows[0].Cells[2].Value.ToString();
            txtPhoneNo.Text = DGVCustomers.SelectedRows[0].Cells[3].Value.ToString();
            txtFullAddress.Text = DGVCustomers.SelectedRows[0].Cells[4].Value.ToString();

            if (txtCustomerName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(DGVCustomers.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            DisplayCustomers();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == "" || CBGender.SelectedIndex == -1 || txtPhoneNo.Text == "" || txtFullAddress.Text == "")
            {
                MessageBox.Show("Missing Information ! ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update CustomersTbl Set CustName=@Cname, Gender=@Gender, PhoneNo=@Phone, Address=@Add where CustId=@CId", Con);
                    cmd.Parameters.AddWithValue("@Cname", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@Gender", CBGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Phone", txtPhoneNo.Text);
                    cmd.Parameters.AddWithValue("@Add", txtFullAddress.Text);
                    cmd.Parameters.AddWithValue("@CId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated Successfully");
                    Con.Close();
                    DisplayCustomers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please select a Customer");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomersTbl where CustId=@CId", Con);
                    cmd.Parameters.AddWithValue("@CId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Has Been Deleted Successfully");
                    Con.Close();
                    DisplayCustomers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
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
            Reset();
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
    }
}
