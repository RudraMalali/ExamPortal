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
using System.Text.RegularExpressions;

namespace ExamPortal
{
    public partial class Register : Form
    {

        public SqlConnection con;
        public string connection;


        public Register()
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
                MessageBox.Show("Connection to Database Unsuccesfull.Please try again." + e);
            }
        }


        //function to validate form before submitting
        public bool Validate_form(string email, string conno, string passwd)
        {
            bool isCnValid, isPassValid, isEmValid;

            //Email validation
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            isEmValid = regex.IsMatch(email.Trim());

            //Password validation
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            isPassValid = hasNumber.IsMatch(passwd) && hasUpperChar.IsMatch(passwd) && hasMinimum8Chars.IsMatch(passwd);

            if (!isPassValid)
            {
                MessageBox.Show("Please enter Password as suggested!");
            }

            if (!isEmValid)
            {
                MessageBox.Show("Please enter correct Email-Id!");
            }

            //contact number validation
            if (conno.Length != 10)
            {
                isCnValid = false;
                MessageBox.Show("Please enter correct contact number!");
            }
            else
                isCnValid = true;

            if (isCnValid && isEmValid && isPassValid)
                return true;
            else
                return false;
        }

        //reset form
        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            Random randid = new Random();
            int id = randid.Next(1000, 1000000);
            String name = textBox1.Text;
            String college = textBox2.Text;
            String rollno = textBox3.Text;
            String conno = textBox4.Text;
            String email = textBox5.Text;
            String passwd = textBox6.Text;

            if ((Validate_form(email, conno, passwd)))
            {
                String sql = "insert into RegisteredStudents values(@Id,@Name,@College,@RollNo,@ContactNo,@Email,@Password)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@College", college);
                    cmd.Parameters.AddWithValue("@RollNo", rollno);
                    cmd.Parameters.AddWithValue("@ContactNo", conno);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", passwd);
                    cmd.ExecuteNonQuery();

                }

                string message = "Registered Successfully.Do you want to Login?";
                string title = "Success";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    Login login = new Login();
                    this.Hide();
                    login.ShowDialog();
                }
                else
                {
                    this.Close();
                }
                Clear();

                con.Close();

            }

        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}
