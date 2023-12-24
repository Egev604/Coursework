using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace PC_club
{
    static class LoadData
    {
        static string  con = conectionString.connectionString;
        public static string[] mas = new string[1];
        public static string[] mas2 = new string[1];
        static public DataTable Load(string query)
        {
            MySqlConnection connection = new MySqlConnection(con);
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, con);
                dataAdapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            connection.Close();
            return table;
        }

        static public void Ins(string query)
        {
            MySqlConnection connection = new MySqlConnection(con);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, con);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            connection.Close();
        }
        static public void Reder(string query)
        {
            MySqlConnection connection = new MySqlConnection(conectionString.connectionString);
            try
            {
                int i = 0;
                int skip = 0;
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, con);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                foreach (DataRow row in table.Rows)
                {
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                    {
                        if(skip==0)
                        {
                            skip++;
                            continue;
                        }     
                        mas[i] = cell.ToString();
                        if (i == mas.Length - 1)
                        {
                            Array.Resize(ref mas, mas.Length + 1);
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Array.Resize(ref mas, mas.Length - 1);
            connection.Close();
        }
        static public void ComboBox(string query)
        {
            MySqlConnection connection = new MySqlConnection(conectionString.connectionString);
            try
            {
                int i = 0;
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mas2[i] = reader.GetValue(0).ToString();
                    Array.Resize(ref mas2, mas2.Length + 1);
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Array.Resize(ref mas2, mas2.Length - 1);
            connection.Close();
        }
        static public string StrOne(string query)
        {
            string returns = "";
            MySqlConnection connection = new MySqlConnection(conectionString.connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returns = reader.GetValue(0).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            return returns;
        }
    }
}
