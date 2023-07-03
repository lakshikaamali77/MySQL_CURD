using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace mysql_connection
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;

        MySqlConnection sqlConn = new MySqlConnection();
        MySqlCommand sqlCmd = new MySqlCommand();   
        DataTable sqlDt = new DataTable();
        String sqlQuery;
        MySqlDataAdapter DtA = new MySqlDataAdapter();
        MySqlDataReader sqlRd;


        DataSet Ds = new DataSet();

        String server = "localhost";
        String username = "root";
        String password = "";
        String database = "studocu";
        public Form1()
        {
            InitializeComponent();
        }

        private void upLoadData()
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;

            sqlConn.Open();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = "SELECT * FROM test1";

            sqlRd = sqlCmd.ExecuteReader();
            sqlDt.Load(sqlRd);
            sqlRd.Close();
            sqlConn.Close();
            dataGridView1.DataSource = sqlDt;
        } 

        private void Form1_Load(object sender, EventArgs e)
        {
            upLoadData();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult iExit;

            try
            {


                iExit = MessageBox.Show("Do you want exit", "MySql Connecter",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (iExit == DialogResult.Yes)
                {
                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in panel3.Controls)
                {
                    if (c is TextBox)
                    ((TextBox)c).Clear();
                }
                 txtSearch.Text ="";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                int height = dataGridView1.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 2;
                bitmap = new Bitmap(dataGridView1.Width, dataGridView1.Height);
                dataGridView1.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
                printPreviewDialog1.PrintPreviewControl.Zoom = 1;
                printPreviewDialog1.ShowDialog();
                dataGridView1.Height = height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                e.Graphics.DrawImage (bitmap, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;

            try
            {
                sqlConn.Open();
                sqlQuery = "insert into test1 (id, firstname, lastname, address,postalCode, phoneNo)" + "values('" + student_txt.Text + "','" + fname_txt.Text + "','" + lName_txt.Text + "','" + address_txt.Text + "','" + postalCode_txt.Text + "','" + phone_txt.Text + "')";


                sqlCmd = new MySqlCommand(sqlQuery, sqlConn);
                    sqlRd = sqlCmd.ExecuteReader();

                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConn.Close ();
            }
            upLoadData();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + 
                "password=" + password + ";" + "database=" + database;
            sqlConn.Open ();

            try
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.Connection = sqlConn;
                   
                sqlCmd.CommandText = "UPDATE test1 SET id=@id,firstName=@firstName,lastName=@lastName,address=@address,postalCode=@postalCode,phoneNo=@phoneNo WHERE id = @id";

                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@id", student_txt.Text);
                sqlCmd.Parameters.AddWithValue("@firstName", fname_txt.Text);
                sqlCmd.Parameters.AddWithValue("@lastName", lName_txt.Text);
                sqlCmd.Parameters.AddWithValue("@address", address_txt.Text);
                sqlCmd.Parameters.AddWithValue("@postalCode", postalCode_txt.Text);
                sqlCmd.Parameters.AddWithValue("@phoneNo", phone_txt.Text);

                sqlCmd.ExecuteNonQuery ();
                sqlConn.Close ();

                upLoadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                student_txt.Text = dataGridView1.SelectedRows [0].Cells[0].Value.ToString();
                fname_txt.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                lName_txt.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                address_txt.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                postalCode_txt.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                phone_txt.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" +
    "password=" + password + ";" + "database=" + database;
            sqlConn.Open();

            sqlCmd.Connection = sqlConn;

            sqlCmd.CommandText = "DELETE FROM test1 WHERE id = @id";
            sqlCmd = new MySqlCommand(sqlQuery,sqlConn);
            sqlConn.Close();

            foreach(DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
            foreach (Control c in panel3.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
            }
            txtSearch.Text = "";
            upLoadData();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter =String.Format("Firstname Like '%(0)%'",txtSearch.Text);
                dataGridView1.DataSource = dv.ToTable();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
    }
}
