using Guna.UI2.Designer;
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
    public partial class Users : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Users()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Reset()
        {
            txtUserName.Text = "";
            DOB.Text = "";
            CBGender.SelectedIndex = -1;
            txtPhoneNo.Text = "";
            txtFullAddress.Text = "";
            txtPassword.Text = "";
        }

        private void DisplayUsers()

        {
            Con.Open();
            string query = " select * from UsersTbl ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVUsers.DataSource = ds.Tables[0];
            Con.Close();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || CBGender.SelectedIndex == -1 || txtPhoneNo.Text == "" || txtFullAddress.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Missing Information ! ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into UsersTbl(UserName,DOB,Gender,PhoneNo,Address,Password) values(@Uname,@Dob,@UGender,@Phone,@Add, @Pass)", Con);
                    cmd.Parameters.AddWithValue("@Uname", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@UGender", CBGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Dob", DOB.Value.Date);
                    cmd.Parameters.AddWithValue("@Phone", txtPhoneNo.Text);
                    cmd.Parameters.AddWithValue("@Add", txtFullAddress.Text);
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Added Successfully");
                    Con.Close();
                    DisplayUsers();
                    Reset();
                }
                catch (Exception)
                {
                    MessageBox.Show("Username Already Taken");
                }
            }

        }

        int Key = 0;
        private void DGVUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUserName.Text = DGVUsers.SelectedRows[0].Cells[1].Value.ToString();
            DOB.Text = DGVUsers.SelectedRows[0].Cells[2].Value.ToString();
            CBGender.SelectedItem = DGVUsers.SelectedRows[0].Cells[3].Value.ToString();
            txtPhoneNo.Text = DGVUsers.SelectedRows[0].Cells[4].Value.ToString();
            txtFullAddress.Text = DGVUsers.SelectedRows[0].Cells[5].Value.ToString();
            txtPassword.Text = DGVUsers.SelectedRows[0].Cells[6].Value.ToString();


            if (txtUserName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(DGVUsers.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void Users_Load(object sender, EventArgs e)
        {
            DisplayUsers();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || CBGender.SelectedIndex == -1 || txtPhoneNo.Text == "" || txtFullAddress.Text == ""|| txtPassword.Text=="")

            {
                MessageBox.Show("Missing Information ! ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update UsersTbl Set UserName=@Uname,DOB=@Dob,Gender=@UGender,PhoneNo=@Phone,Address=@Add , Password=@Pass where UserId=@UId", Con);
                    cmd.Parameters.AddWithValue("@Uname", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@Dob", DOB.Value.Date);
                    cmd.Parameters.AddWithValue("@UGender", CBGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Phone", txtPhoneNo.Text);
                    cmd.Parameters.AddWithValue("@Add", txtFullAddress.Text);
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@UId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated Successfully");
                    Con.Close();
                    DisplayUsers();
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
                MessageBox.Show("Please select a User");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from UsersTbl where UserId=@UId", Con);
                    cmd.Parameters.AddWithValue("@UId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Has Been Deleted Successfully");
                    Con.Close();
                    DisplayUsers();
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

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

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
