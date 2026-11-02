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

    public partial class Pet_Food : Form
    {
        // Using a double backslash or a string literal '@'
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-9AFTTF7Q\SQLEXPRESS;Initial Catalog=PetShopManagement;Integrated Security=True;TrustServerCertificate=True");

        public Pet_Food()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Reset()
        {
            txtPetFoodName.Text = "";
            CBCategory.SelectedItem = -1;
            txtQuantity.Text = "";
            txtPrice.Text = "";
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
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (txtPetFoodName.Text == "" || CBCategory.SelectedIndex == -1 || txtQuantity.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Missing Information ! ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into PetFoodsTbl(foodName,Category,Quantity,Price) values(@Fname,@Cate,@Qty,@Price)", Con);
                    cmd.Parameters.AddWithValue("@Fname", txtPetFoodName.Text);
                    cmd.Parameters.AddWithValue("@Cate", CBCategory.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Qty", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Pet Food Added Successfully");
                    Con.Close();
                    DisplayPetFood();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int Key = 0;
        private void DGVPetFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPetFoodName.Text = DGVPetFood.SelectedRows[0].Cells[1].Value.ToString();
            CBCategory.SelectedItem = DGVPetFood.SelectedRows[0].Cells[2].Value.ToString();
            txtQuantity.Text = DGVPetFood.SelectedRows[0].Cells[3].Value.ToString();
            txtPrice.Text = DGVPetFood.SelectedRows[0].Cells[4].Value.ToString();

            if (txtPetFoodName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(DGVPetFood.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtPetFoodName.Text == "" || CBCategory.SelectedIndex == -1 || txtQuantity.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Missing Information ! ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update PetFoodsTbl Set foodName=@Fname, Category=@Cate,Quantity=@Qty, Price=@Price where FoodId=@FId", Con);
                    cmd.Parameters.AddWithValue("@Fname", txtPetFoodName.Text);
                    cmd.Parameters.AddWithValue("@Cate", CBCategory.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Qty", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@FId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Pet Food Updated Successfully");
                    Con.Close();
                    DisplayPetFood();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }

            }

        private void Pet_Food_Load(object sender, EventArgs e)
        {
            DisplayPetFood();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please select Pet Food First");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from PetFoodsTbl where FoodId=@FId", Con);
                    cmd.Parameters.AddWithValue("@FId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Pet Food Has Been Deleted Successfully");
                    Con.Close();
                    DisplayPetFood();
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

        private void txtPetFoodName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void btnPets_Click(object sender, EventArgs e)
        {
            Pets obj = new Pets();
            obj.Show();
            this.Hide();
        }

        private void btnPetFood_Click(object sender, EventArgs e)
        {
            Reset();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
