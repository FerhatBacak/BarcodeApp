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

namespace BarcodeApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string cs = "DATA SOURCE=.;INITIAL CATALOG=PRODUCTION;INTEGRATED SECURITY=TRUE";
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
  
        private void button1_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = cs;
           string sql = "SELECT * FROM PRODUCTION.DBO.BARCODES WHERE BARCODE='" + textBox1.Text + "'";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count==0) 
            {
                lblMsg.Text="Böyle bir barkod bulunamadı!";
                return;
            }

            sql = "SELECT * FROM PRODUCTION.DBO.PRODPLAN WHERE ITEMID IN(SELECT ITEMID FROM BARCODES WHERE BARCODE='" + textBox1.Text + "')"; 
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count==0)
            {
                lblMsg.Text = "Bu ürüne ait üretim planı bulunamadı!";
                return;
            }

            sql = "SELECT * FROM PRODUCTION.DBO.BARCODE_PRODUCTION WHERE BARCODE='" + textBox1.Text + "'";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Bu barkod daha önce okutulmuş!";
                return;
            }

            sql = "INSERT INTO PRODUCTION.DBO.BARCODE_PRODUCTION (BARCODE,DATE_,STATION) VALUES ('" + textBox1.Text + "',GETDATE(),1)";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            lblMsg.Text = "Barkod başarı ile eklendi";
            textBox1.Text = "";
            textBox1.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                conn.ConnectionString = cs;
                string sql = "EXEC BARCODE_INSERT @BARCODE='" + textBox1.Text + "',@STATIONID=1";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                ds.Clear();
                ds.Reset();
                da.Fill(ds);
                lblMsg.Text = "Barkod başarı ile eklenmiştir.";
                lblMsg.Text = "";
                lblMsg.Focus();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }

}
