

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pet_Shop_Management_System
{
    public partial class Users_Login : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");


        public Users_Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text?.Trim();
            string Password = txtPassword.Text;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use a using block so the connection is always closed and disposed.
            try
            {
                using (var con = new SqlConnection(Con.ConnectionString))
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM UsersTbl WHERE UserName = @UserName AND Password = @Password", con))
                {
                    // Use explicit parameter types instead of AddWithValue for better predictability.
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Password;

                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Billing_User billingForm = new Billing_User();
                        billingForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid UserName or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // If connection fails (Named Pipes/Instance not found) provide actionable hint.
                if (sqlEx.Number == 53 || sqlEx.Number == 40)
                {
                    MessageBox.Show("Cannot connect to SQL Server instance.\n" +
                                    "Check that SQL Server is running, the instance name is correct, and network protocols are enabled.\n" +
                                    $"Detailed error: {sqlEx.Message}", "SQL Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Database error: " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Admin_login obj = new Admin_login();
            obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblForgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forget_Password obj = new Forget_Password();
            obj.Show();
            this.Hide();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (CBShowPass.Checked)
            {
                // Show the password
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                // Hide the password
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void Users_Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}