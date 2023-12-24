using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
namespace PC_club
{
    public partial class staff : Form
    {
        bool result;
        string rdr = "employees";
        int lol;
        string poisk;
        string sort;

        public staff(bool resul)
        {
            result = resul;
            InitializeComponent();
            Loadd();
            button2.Enabled = false;
            button3.Enabled = false;
        }
        public void Loadd()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_employees, (select roleName from role where id_role=position) as position, surname, name, patronymic, dateofbirth,phone from employees");
            dataGridView1.Columns["id_employees"].Visible = false;
            dataGridView1.Columns["position"].HeaderText = "Должность";
            dataGridView1.Columns["surname"].HeaderText = "Фамилия";
            dataGridView1.Columns["name"].HeaderText = "Имя";
            dataGridView1.Columns["patronymic"].HeaderText = "Отчество";
            dataGridView1.Columns["dateofbirth"].HeaderText = "Дата рождения";
            dataGridView1.Columns["phone"].HeaderText = "Телефон";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            foreach(TextBox q in this.Controls.OfType<TextBox>())
            {
                if (q.Tag.ToString() == "vaz")
                {
                    if (q.Text == string.Empty)
                    {
                        MessageBox.Show("Заполните поля");
                        foreach (TextBox w in this.Controls.OfType<TextBox>())
                        {
                            w.Clear();
                        }
                        return;
                    }
                }
            }
            string[] mas = dateTimePicker1.Value.ToShortDateString().Split('.');
            LoadData.Ins(String.Format("Insert into employees (surname,name,patronymic,dateofbirth,phone, position) values( '{0}', '{1}', '{2}', '{3}.{4}.{5}', '{6}', (select id_role from role where roleName='{7}'));", textBox1.Text, textBox2.Text, textBox3.Text, mas[2], mas[1], mas[0], textBox4.Text, comboBox1.Text));
            Loadd();
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length==1)
            {
                textBox1.Text = textBox1.Text.ToUpper();
                textBox1.SelectionStart = 1;
            }
        }

        public void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]|[\b]").Success)
            {
                e.Handled = true;
            }
        }

        public void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[1-9]|[\b]").Success)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (TextBox q in this.Controls.OfType<TextBox>())
            {
                if (q.Tag.ToString() == "vaz")
                {
                    if (q.Text == string.Empty)
                    {
                        MessageBox.Show("Заполните поля");
                        foreach (TextBox w in this.Controls.OfType<TextBox>())
                        {
                            w.Clear();
                        }
                        return;
                    }
                }
            }

            string[] mas = dateTimePicker1.Value.ToShortDateString().Split('.');
            LoadData.Ins(String.Format("Update employees SET surname='{0}', name='{1}', patronymic='{2}', dateofbirth='{3}.{4}.{5}', phone='{6}', position=(select id_role from role where roleName='{7}') where id_employees={8};", textBox1.Text, textBox2.Text, textBox3.Text, mas[2], mas[1], mas[0], textBox4.Text, comboBox1.Text, lol));
            Loadd();
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Clear();
            }
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string code;
            int rowIndex = e.RowIndex;
            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
            try
            {
                code = dataGridView1.Rows[rowIndex].Cells["id_" + rdr].Value.ToString();
            }
            catch
            {
                return;
            }
            string strCmd = "Select * from employees Where id_" + rdr + "=" + '"' + code + '"' + ";";
            lol = Convert.ToInt32(code);
            Reder(strCmd);
            button2.Enabled = true;
            button3.Enabled = true;
        }
        public void Reder(string query)
        {
            string[] mas = new string[10];
            MySqlConnection connection = new MySqlConnection(conectionString.connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader.GetString("surname");
                    textBox2.Text = reader.GetString("name");
                    textBox3.Text = reader.GetString("patronymic");
                    string[] str = reader.GetString("dateofbirth").Split(' ');
                    string[] str1 = str[0].Split('.');
                    dateTimePicker1.Value = new DateTime(Convert.ToInt32(str1[2]), Convert.ToInt32(str1[1]), Convert.ToInt32(str1[0]));
                    textBox4.Text = reader.GetString("phone");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                LoadData.Ins("Delete from employees where id_" + rdr + "=" + '"' + lol + '"' + ';');
            }
            else
            {
                return;
            }
            Loadd();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 1)
            {
                textBox2.Text = textBox2.Text.ToUpper();
                textBox2.SelectionStart = 1;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 1)
            {
                textBox3.Text = textBox3.Text.ToUpper();
                textBox3.SelectionStart = 1;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sort = "Order by surname asc";
            Sinxron();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sort = "Order by surname desc";
            Sinxron();
        }
        private void Sinxron()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_employees, (select roleName from role where id_role=position) as position, surname, name, patronymic, dateofbirth,phone from employees " + poisk + sort);
            dataGridView1.Columns["id_employees"].Visible = false;
            dataGridView1.Columns["position"].HeaderText = "Должность";
            dataGridView1.Columns["surname"].HeaderText = "Фамилия";
            dataGridView1.Columns["name"].HeaderText = "Имя";
            dataGridView1.Columns["patronymic"].HeaderText = "Отчество";
            dataGridView1.Columns["dateofbirth"].HeaderText = "Дата рождения";
            dataGridView1.Columns["phone"].HeaderText = "Телефон";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length == 0 || textBox5.Text.Length == 1)
            {
                poisk = "";
                Sinxron();
                return;
            }
            if (textBox5.Text.Length == 1)
            {
                textBox5.Text = textBox5.Text.ToUpper();
                textBox5.SelectionStart = 1;
            }
            poisk = "Where surname like '%" + textBox5.Text + "%' ";
            Sinxron();
        }

        private void staff_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Администратор");
            comboBox1.Items.Add("Сотрудник");
        }
    }
}
