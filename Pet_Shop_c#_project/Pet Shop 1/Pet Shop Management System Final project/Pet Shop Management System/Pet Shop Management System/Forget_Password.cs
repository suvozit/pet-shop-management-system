using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management_System
{

    public partial class Forget_Password : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Forget_Password()
        {
            InitializeComponent();
        }

        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

        private void Forget_Password_Load(object sender, EventArgs e)
        {
            
            //txtPass.ReadOnly = true;
            txtPass.UseSystemPasswordChar = true;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (CBShowPass.Checked)
            {
                // Show the password
                txtPass.UseSystemPasswordChar = false;
            }
            else
            {
                // Hide the password
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string phone = txtPhone.Text;

            try
            {
                Con.Open();
                string query = "SELECT COUNT(*) FROM UsersTbl WHERE UserName = @username AND PhoneNo = @phone";

                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@phone", phone);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("User verified successfully!", "Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPass.Enabled = true;   
                }
                else
                {
                    MessageBox.Show("Invalid user or number!", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Enabled = false;  
                }
                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string newPassword = txtPass.Text;

            try {
            Con.Open();
            string query = "UPDATE UsersTbl SET Password = @password WHERE UserName = @username";

            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.Parameters.AddWithValue("@password", newPassword);
            cmd.Parameters.AddWithValue("@username", username);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPass.Enabled = false; 
                txtPass.Clear();
            }
            else
            {
                MessageBox.Show("Username not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Con.Close() ;
            
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
}

        private void lblGoBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Users_Login obj = new Users_Login();
            obj.Show();
            this.Hide();
        }
    }
}
