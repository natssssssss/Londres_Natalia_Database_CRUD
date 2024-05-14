using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static Londres_Natalia_Database_CRUD.Form1;

namespace Londres_Natalia_Database_CRUD
{
    public partial class Form1 : Form
    {
        private DatabaseConnection dbConnection;
        private DataAccessLayer dataAccessLayer;
        private String tableName = "students";

        public Form1()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection("localhost", "adu_sample_db", "root", "");
            dataAccessLayer = new DataAccessLayer(dbConnection);
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string query = $"SELECT id, lname AS 'Last Name', fname AS 'First Name' FROM {tableName}";
            DataTable dataTable = dataAccessLayer.Select(query);
            dataGridView1.DataSource = dataTable;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string lname = textBox1.Text;
            string fname = textBox2.Text;
            try
            {
                string query = $"INSERT INTO {tableName} (lname, fname) VALUES ('{lname}', '{fname}')";
                dataAccessLayer.Insert(query);
                LoadData();
                MessageBox.Show("Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string lname = textBox1.Text;
            string fname = textBox2.Text;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                string query = $"UPDATE {tableName} SET lname='{lname}', fname='{fname}' WHERE id={id}";
                dataAccessLayer.Update(query);
                LoadData();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                string query = $"DELETE FROM {tableName} WHERE id={id}";
                dataAccessLayer.Delete(query);
                LoadData();

            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["Last Name"].Value.ToString();
                textBox2.Text = selectedRow.Cells["First Name"].Value.ToString();
            }
        }

        public class DatabaseConnection
        {
            private string connectionString;
            private MySqlConnection connection;

            public DatabaseConnection(string server, string database, string uid, string password)
            {
                connectionString = $"SERVER={server}; DATABASE={database}; UID={uid}; PASSWORD={password};";
                connection = new MySqlConnection(connectionString);
            }

            public void OpenConnection()
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
            }

            public void CloseConnection()
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            public MySqlConnection GetConnection()
            {
                return connection;
            }
        }

        public class DataAccessLayer
        {
            private DatabaseConnection dbConnection;

            public DataAccessLayer(DatabaseConnection connection)
            {
                dbConnection = connection;
            }

            public DataTable Select(string query)
            {
                using (MySqlCommand cmd = new MySqlCommand(query, dbConnection.GetConnection()))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        dbConnection.OpenConnection();
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        dbConnection.CloseConnection();
                    }
                    return dt;
                }
            }

            public void Insert(string query)
            {
                using (MySqlCommand cmd = new MySqlCommand(query, dbConnection.GetConnection()))
                {
                    try
                    {
                        dbConnection.OpenConnection();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        dbConnection.CloseConnection();
                    }
                }
            }

            public void Update(string query)
            {
                using (MySqlCommand cmd = new MySqlCommand(query, dbConnection.GetConnection()))
                {
                    try
                    {
                        dbConnection.OpenConnection();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        dbConnection.CloseConnection();
                    }
                }
            }

            public void Delete(string query)
            {
                using (MySqlCommand cmd = new MySqlCommand(query, dbConnection.GetConnection()))
                {
                    try
                    {
                        dbConnection.OpenConnection();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        dbConnection.CloseConnection();
                    }
                }
            }
        }
    }
}
