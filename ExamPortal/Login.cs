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

namespace ExamPortal
{
    public partial class Login : Form
    {
        public SqlConnection con;
        public string connection;
        public Login()
        {
            InitializeComponent();
            connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=testrud";
            con = new SqlConnection(connection);
            try
            {
                con.Open();

            }
            catch (Exception e)
            {
                MessageBox.Show("Connection to Database Unsuccesful.Please try again." + e);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            String passwd = textBox2.Text;


            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM RegisteredStudents WHERE Email='" + username + "' AND Password='" + passwd + "'", con);

            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("Login Successfull");
                Exam ex = new Exam();
                this.Hide();
                ex.ShowDialog();
            }
            else
                MessageBox.Show("Invalid username or password");

        }


        private void Button2_Click(object sender, EventArgs e)
        {
            Register reg = new Register();
            this.Hide();
            reg.ShowDialog();
        }
    }
}
