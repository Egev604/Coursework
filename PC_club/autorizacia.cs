using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Threading;
namespace PC_club
{
    public partial class autorizacia : Form
    {
        int pop = 0;
        int lol = 0;
        string[] mas = new string[1];
        public autorizacia()
        {
            InitializeComponent();
            Reader("SELECT (select surname from employees where id_employees=user_name)as user_name FROM mydb.users;");
            textBox2.UseSystemPasswordChar = true;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            while (mas.Length-1>lol)
            {
                comboBox1.Items.Add(mas[lol]);
                lol++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pop == 0)
            {
                textBox2.UseSystemPasswordChar = false;
                pop = 1;
                button3.Text = "Скрыть пароль";
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
                pop = 0;
                button3.Text = "Показать пароль";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Поле должно быть заполнено");
                return;
            }
            string login = string.Empty;
            string passwd=string.Empty;
            MySqlConnection connection = new MySqlConnection(conectionString.connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("select login, passwd from users where user_name =(select id_employees from employees where surname='{0}')", comboBox1.Text), connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    login = reader.GetString("login");
                    passwd = reader.GetString("passwd");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (textBox2.Text == passwd)
            {
                if (LoadData.StrOne("select position from employees where surname='"+comboBox1.Text+"'") == "1")
                {
                    this.Hide();
                    Главная glavnay = new Главная(true);
                    glavnay.Show();
                }
                else
                {
                    this.Hide();
                    Главная glavnay = new Главная(false);
                    glavnay.Show();
                }
            }
            else
            {
                MessageBox.Show("Логин или пороль неверный");
                comboBox1.SelectedIndex = -1;
                textBox2.Clear();
            }
        }
        public void Reader(string query)
        {
            MySqlConnection connection = new MySqlConnection(conectionString.connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    mas[i] = reader.GetString("user_name");
                    i++;
                    Array.Resize(ref mas, mas.Length + 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
