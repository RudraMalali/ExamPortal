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
using System.Timers;

namespace ExamPortal
{
    
    public partial class Exam : Form
    {
        private static System.Timers.Timer timer;
        string connstring;
        SqlConnection con;
        SqlCommand command;
        SqlDataAdapter da = new SqlDataAdapter();
        SqlDataReader dr;
        public Exam()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            global.marks = 0;
            connstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=testrud";
            con = new SqlConnection(connstring);
            con.Open();
            command = new SqlCommand("UPDATE quiz SET flag = 0", con);
            da.UpdateCommand = new SqlCommand("UPDATE quiz SET flag = 0", con);
            da.UpdateCommand.ExecuteNonQuery();
            da.Dispose();
            command = new SqlCommand("SELECT TOP 1 * FROM quizview ORDER BY NEWID()", con);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                label1.Text = Convert.ToString(dr.GetValue(1));
                radioButton1.Text = Convert.ToString(dr.GetValue(2));
                radioButton2.Text = Convert.ToString(dr.GetValue(3));
                radioButton3.Text = Convert.ToString(dr.GetValue(4));
                radioButton4.Text = Convert.ToString(dr.GetValue(5));
                global.sopt = Convert.ToString(dr.GetValue(0));


            }
            dr.Close();
            da.UpdateCommand = new SqlCommand("UPDATE quiz SET flag = 1 where id=" +global.sopt, con);
            da.UpdateCommand.ExecuteNonQuery();
            da.Dispose();

            
            command.Dispose();
            con.Close();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            tlabl:
            if (radioButton1.Checked == true)
            {
                global.opt = 1;
                radioButton1.Checked = false;
            }
            else if (radioButton2.Checked == true)
            {
                global.opt = 2;
                radioButton2.Checked = false;
            }
            else if (radioButton3.Checked == true)
            {
                global.opt = 3;
                radioButton3.Checked = false;
            }
            else if (radioButton4.Checked == true)
            {
                global.opt = 4;
                radioButton4.Checked = false;
            }
            else
                global.opt = 99;
        
            con.Open();
            string qvar = label1.Text;
            string q = "SELECT * FROM quiz where question = '" + qvar+"'";
            command = new SqlCommand(q, con);
            dr = command.ExecuteReader();
            
                while (dr.Read())
                {
                    if (global.opt == Convert.ToInt32(dr.GetValue(6)))
                        global.marks += 1;
                }
                dr.Close();
                command.Dispose();

                command = new SqlCommand("SELECT TOP 1 * FROM quizview where flag=0 ORDER BY NEWID()", con);
                dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    label1.Text = Convert.ToString(dr.GetValue(1));
                    radioButton1.Text = Convert.ToString(dr.GetValue(2));
                    radioButton2.Text = Convert.ToString(dr.GetValue(3));
                    radioButton3.Text = Convert.ToString(dr.GetValue(4));
                    radioButton4.Text = Convert.ToString(dr.GetValue(5));
                    global.sopt = Convert.ToString(dr.GetValue(0));
                }
                dr.Close();
                da.UpdateCommand = new SqlCommand("UPDATE quiz SET flag = 1 where id=" + global.sopt, con);
                da.UpdateCommand.ExecuteNonQuery();
                da.Dispose();


                command.Dispose();
                
            }
            else
            {
                button1.Text = Convert.ToString(global.marks);
            }
            con.Close();
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //label2.Text = e.SignalTime.ToString();
            //goto tlabl;
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    static class global
    {
        public static int opt, marks;
        public static string sopt;
    }
}
