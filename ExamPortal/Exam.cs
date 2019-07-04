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
using Timers=System.Timers;
using System.Timers;
using MetroFramework.Forms;

namespace ExamPortal
{
    
    public partial class Exam : MetroForm
    {
        Timers.Timer timer=null;
        string connstring;
        SqlConnection con;
        SqlCommand command;
        SqlDataAdapter da = new SqlDataAdapter();
        SqlDataReader dr;
        public Exam()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            timer.SynchronizingObject = this;
            timer.AutoReset = true;
            //timer = new Timers.Timer(10000);
            //timer.Elapsed += this.nxtq;
            //timer.SynchronizingObject = this;

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }
        private void nxtq(Object source,Timers.ElapsedEventArgs e)
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
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            global.cnt -= 1;
            if(global.cnt==0)
            {
                testfn();
                global.cnt = 10;
            }
            label2.Text = Convert.ToString(global.cnt);
            
        }
        private void testfn()
        {
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
            string q = "SELECT * FROM quiz where question = '" + qvar + "'";
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
                con.Close();

            }
            //Test Finished Indicator Block
            else
            {
                label2.Visible=false;
                //button1.Text = Convert.ToString(global.marks);
                string msg = "Your result is :- "+global.marks;
                string title = "Test Finished";
                MessageBoxButtons buttons = MessageBoxButtons.RetryCancel;
                DialogResult obj = MessageBox.Show(msg,title,buttons);
                if(obj== DialogResult.Retry)
                {
                    Login lg = new Login();
                    this.Hide();
                    lg.ShowDialog();
                }
                else
                {
                    con.Close();
                    Application.Exit();
                }
                
            }
            con.Close();
    }

        private void Button1_Click(object sender, EventArgs e)
        {
            timer.Stop();
            global.cnt = 10;
            timer.Start();
            testfn();
        }  
        

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    static class global
    {
        public static int opt, marks,cnt=10;
        public static string sopt;
    }
}
