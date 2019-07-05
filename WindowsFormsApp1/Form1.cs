using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public SQLiteConnection con;
        public SQLiteDataAdapter da;
        public SQLiteCommand cmd;
        public DataSet ds;

        public void yenile()
        {
            con = new SQLiteConnection("Data Source=veritabani.sqlite;Version=3;");
            da = new SQLiteDataAdapter("select * from rapor", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "rapor");
            dataGridView1.DataSource = ds.Tables["rapor"];
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("veritabani.sqlite"))
            {
                SQLiteConnection.CreateFile("veritabani.sqlite");

                string sql = @"CREATE TABLE rapor(
                               ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                               olay           TEXT      NOT NULL
                            );";
                con = new SQLiteConnection("Data Source=veritabani.sqlite;Version=3;");
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();

            }

            //DataGridView Fill Method
            yenile();
        }

        public string durum;

        public Form1()
        {
            InitializeComponent();
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            durum = e.Name + " dosyasının içerisinde " + e.ChangeType + " değişikliği oldu.";
            yaz(durum);
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            durum = e.Name + " dosyası oluşturuldu.";
            yaz(durum);
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            durum = e.Name + " dosyası silindi.";
            yaz(durum);
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            durum = e.OldName + " dosyasının ismi " + e.Name + " olarak değiştirildi.";
            yaz(durum);
        }

        public void yaz(string durum)
        {
            cmd = new SQLiteCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "insert into rapor(olay) values ('" + durum + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            yenile();
        }
    }
}
