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
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace PC_club
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;database=mydb;uid=root;pwd=root;charset=utf8";
        bool result;
        string rdr = "users";
        string lol = "";
        public void users()
        {
            LoaddData("users", "id_users,login,passwd,(select surname from employees where id_employees=user_name) as user_name, data_create");
            dataGridView1.Columns["id_users"].Visible = false;
            dataGridView1.Columns["login"].HeaderText = "Логин";
            dataGridView1.Columns["passwd"].HeaderText = "Пароль";
            dataGridView1.Columns["user_name"].HeaderText = "Фамилия";
            dataGridView1.Columns["data_create"].HeaderText = "Дата создания";
            int rows = dataGridView1.Rows.Count;
            label4.Text = "Количество строк: " + rows;

        }
        void LoaddData(string selectTable, string column)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "select " + column + " from " + selectTable + ";";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connectionString);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                dataGridView1.DataSource = table;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            connection.Close();
        }
        public Form1(bool resul)
        {
            result = resul;
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(253, 189, 64);
            users();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select surname from employees");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                int a = 0;
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if(row.Cells["user_name"].Value.ToString()!=LoadData.mas2[i])
                    {
                        a++;
                    }
                }
                if(a==dataGridView1.RowCount)
                {
                    comboBox1.Items.Add(LoadData.mas2[i]);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text=="")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            if (textBox12.Text == "")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            if (textBox13.Text == "")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            string name = comboBox1.Text;
            string login = textBox13.Text;
            string passwd = textBox12.Text;
            LoadDataа("INSERT INTO users(login,passwd,user_name,data_create) values(" + '"' + login + '"' + ',' + '"' + passwd + '"' + ',' + "(select id_employees from employees where surname='" + name + "'),'"+DateTime.Now.ToString("yyyy-MM-dd") + "');");
            comboBox1.SelectedIndex = -1;
            textBox12.Clear();
            textBox13.Clear();
            LoaddData("users", "id_users,login,passwd,(select surname from employees where id_employees=user_name) as user_name, data_create");
            dataGridView1.Columns["id_users"].Visible = false;
            dataGridView1.Columns["login"].HeaderText = "Логин";
            dataGridView1.Columns["passwd"].HeaderText = "Пароль";
            dataGridView1.Columns["user_name"].HeaderText = "Фамилия";
            dataGridView1.Columns["data_create"].HeaderText = "Дата создания";
            int rows = dataGridView1.Rows.Count;
            label4.Text = "Количество строк: " + rows;
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select surname from employees");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                int a = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["user_name"].Value.ToString() != LoadData.mas2[i])
                    {
                        a++;
                    }
                }
                if (a == dataGridView1.RowCount)
                {
                    comboBox1.Items.Add(LoadData.mas2[i]);
                }
            }
        }
        void LoadDataа(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connectionString);
                try
                {
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Delete("Delete from users where id_" + rdr + "=" + '"' + lol + '"' + ';');
            }
            else
            {
                return;
            }
            LoaddData("users", "id_users,login,passwd,(select surname from employees where id_employees=user_name) as user_name, data_create");
            dataGridView1.Columns["id_users"].Visible = false;
            dataGridView1.Columns["login"].HeaderText = "Логин";
            dataGridView1.Columns["passwd"].HeaderText = "Пароль";
            dataGridView1.Columns["user_name"].HeaderText = "Фамилия";
            dataGridView1.Columns["data_create"].HeaderText = "Дата создания";
            int rows = dataGridView1.Rows.Count;
            label4.Text = "Количество строк: " + rows;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string code;
            int rowIndex = e.RowIndex;
            try
            {
                code = dataGridView1.Rows[rowIndex].Cells["id_" + rdr].Value.ToString();
            }
            catch
            {
                return;
            }
            string strCmd = "Select * from users Where id_" + rdr + "=" + '"' + code + '"' + ";";
            lol = code;
            Reader(strCmd);
        }
        void Reader(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox3.Text = reader.GetString("login");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Delete(string ins)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(ins, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(ins, connectionString);
                DataTable table = new DataTable();

                dataAdapter.Fill(table);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = "zxcv54bn234masdfVBNMASDFGH18gh321jklqw3261ertyuiop156789ZXCJKLQWERTYUIOP";
            Random pop = new Random();
            int k = 0;
            char[] pass = str.ToCharArray();
            char[] pass2 = new char[20];
            for (int i = 0; i < 20; i++)
            {
                k = pop.Next(71);
                pass2[i] = pass[k];
            }
            string value = String.Concat<char>(pass2);
            MessageBox.Show("Ваш пароль(обязательно запишите):" + value);
            textBox12.Text = value;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string str = "zxcv54bn234masdfVBNMASDFGH18gh321jklqw3261ertyuiop156789ZXCJKLQWERTYUIOP";
            Random pop = new Random();
            int k = 0;
            char[] pass = str.ToCharArray();
            char[] pass2 = new char[20];
            for (int i = 0; i < 20; i++)
            {
                k = pop.Next(71);
                pass2[i] = pass[k];
            }
            string value = String.Concat<char>(pass2);
            MessageBox.Show("Ваш пароль(обязательно запишите):" + value);
            textBox12.Text = value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            string login = "";
            login = textBox3.Text;
            for (int i = 0; i < login.Length; i++)
            {
                if (login[i] >= '0' && login[i] <= '9')
                {
                    MessageBox.Show("в строке есть цифры");
                    textBox3.Clear();
                    return;
                }
            }
            string passwd = textBox2.Text;
            LoadDataа("update users Set passwd=" + '"' + passwd + '"' + ", login=" + '"' + login + '"' + " WHERE id_users=" + '"' + lol + '"' + ";");
            textBox2.Clear();
            textBox3.Clear();
            LoaddData("users", "id_users,login,passwd,(select surname from employees where id_employees=user_name) as user_name, data_create");
            dataGridView1.Columns["id_users"].Visible = false;
            dataGridView1.Columns["login"].HeaderText = "Логин";
            dataGridView1.Columns["passwd"].HeaderText = "Пароль";
            dataGridView1.Columns["user_name"].HeaderText = "Фамилия";
            dataGridView1.Columns["data_create"].HeaderText = "Дата создания";
            int rows = dataGridView1.Rows.Count;
            label4.Text = "Количество строк: " + rows;
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            LoaddData("users", "id_users,login,passwd,(select surname from employees where id_employees=user_name) as user_name, data_create");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]|[\b]").Success)
            {
                e.Handled = true;
            }
        }
    }
}
