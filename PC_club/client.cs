using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace PC_club
{
    public partial class client : Form
    {
        bool result;
        string rdr = "clients";
        int lol;
        string poisk;
        string sort;
        public client(bool resul)
        {
            InitializeComponent();
            result = resul;
            Loadd();
            button2.Enabled = false;
            button3.Enabled = false;
            if(!resul)
            {
                button5.Visible = false;
                button6.Visible = false;
            }
            SaleCheck();
        }
        public void Loadd()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_clients, surname as 'Фамилия',name as 'Имя', patronymic as 'Отчество', dateofbirth as 'Дата рождения', minutes as 'Минуты', (select sale from sale_minuts where idsale=id_sale) as 'Скидка' from clients");
            dataGridView1.Columns["id_clients"].Visible = false;
        }
        public void SaleCheck()
        {

            DataTable table2 = new DataTable();
            DataTable table = new DataTable();
            int d;
            table2 = LoadData.Load("select * from clients inner join sale_minuts where sale_minuts.id_sale=clients.idsale");
            table = LoadData.Load("select * from sale_minuts");
            foreach (DataRow roww in table2.Rows)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (Convert.ToInt32(row["minuts"]) < Convert.ToInt32(roww["minutes"]))
                    {
                        if (Convert.ToInt32(row["sale"]) > Convert.ToInt32(roww["sale"]))
                        {
                            d = Convert.ToInt32(roww["idsale"]);
                            if (Convert.ToInt32(row["id_sale"]) != d)
                            {
                                LoadData.Ins("Update clients Set idsale=" + row["id_sale"] + " where id_clients=" + roww["id_clients"] + "");
                                SaleCheck();
                            }
                        }
                    }

                }
            }
            Loadd();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }

        private void button1_Click(object sender, EventArgs e)
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
            LoadData.Ins(String.Format("Insert into clients (surname,name,patronymic,dateofbirth,minutes, idsale) values('{0}', '{1}', '{2}', '{3}.{4}.{5}', 0, 1);", textBox1.Text, textBox2.Text, textBox3.Text, mas[2], mas[1], mas[0]));
            Loadd();
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 1)
            {
                textBox1.Text = textBox1.Text.ToUpper();
                textBox1.SelectionStart = 1;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]|[\b]").Success)
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
            LoadData.Ins(String.Format("Update clients SET surname='{0}', name='{1}', patronymic='{2}', dateofbirth='{3}.{4}.{5}' where id_clients={6};", textBox1.Text, textBox2.Text, textBox3.Text, mas[2], mas[1], mas[0], lol));
            Loadd();
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Clear();
            }
            button2.Enabled = false;
            button3.Enabled = false;
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
            string strCmd = "Select * from clients Where id_" + rdr + "=" + '"' + code + '"' + ";";
            lol = Convert.ToInt32(code);
            LoadData.Reder(strCmd);
            string[] arr = new string[LoadData.mas.Length];
            arr = LoadData.mas;
            int i = 0;
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                if (w.Tag.ToString() != "nevaz")
                {
                    w.Text = arr[i];
                    i++;
                }
            }
            string[] str = arr[3].Split(' ');
            string[] str1 = str[0].Split('.');
            dateTimePicker1.Value = new DateTime(Convert.ToInt32(str1[2]), Convert.ToInt32(str1[1]), Convert.ToInt32(str1[0]));
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                LoadData.Ins("Delete from clients where id_" + rdr + "=" + '"' + lol + '"' + ';');
            }
            else
            {
                return;
            }
            Loadd();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == 0 || textBox4.Text.Length == 1)
            {
                poisk = "";
                Sinxron();
                return;
            }
            if (textBox4.Text.Length == 1)
            {
                textBox4.Text = textBox4.Text.ToUpper();
                textBox4.SelectionStart = 1;
            }
            poisk="Where surname like '%"+textBox4.Text+"%' ";
            Sinxron();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sort = "Order by surname asc";
            Sinxron();
        }
        private void Sinxron()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_clients, surname as 'Фамилия',name as 'Имя', patronymic as 'Отчество', dateofbirth as 'Дата рождения', minutes as 'Минуты', (select sale from sale_minuts where idsale=id_sale) as 'Скидка' from clients " + poisk+sort);
            dataGridView1.Columns["id_clients"].Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sort = "Order by surname desc";
            Sinxron();
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

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы действительно хотите произвести экспорт в csv файл?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

            if (dr == DialogResult.Yes)
            {
                DataTable dt = new DataTable();
                dt = LoadData.Load("Select * from clients");
                string fileName = "Клиенты.csv";
                FileStream fs = null;
                fs = new FileStream(fileName, FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs, Encoding.Unicode);
                for (int i = 0, len = dt.Columns.Count - 1; i <= len; ++i)
                {
                    writer.Write(dt.Columns[i].ColumnName);
                    if (i != len)
                        writer.Write(";");
                }
                writer.Write("\n");
                foreach (DataRow dataRow in dt.Rows)
                {
                    string r = String.Join(";", dataRow.ItemArray);
                    writer.WriteLine(r);
                }
                writer.Close();
                MessageBox.Show("Выгружено " + dt.Rows.Count + " строк", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы действительно хотите произвести импорт из csv файла?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

            if (dr == DialogResult.Yes)
            {
                StreamReader FILE = null;

                try
                {
                    string read = "";
                    string[] array;
                    FILE = new StreamReader("Клиенты.csv");
                    int row = 0;
                    string insert = "";
                    while ((read = FILE.ReadLine()) != null)
                    {
                        if (row == 0)
                        {
                            row++;
                            continue;
                        }
                        if (row > 1)
                        {
                            insert += ',';
                        }
                        array = read.Split(';');
                        string[] arr = array[4].Split(' ');
                        string[] arr2 = arr[0].Split('.');
                        insert += "('" + array[1] + "', '" + array[2] + "','" + array[3] + "','" + arr2[2]+"-"+arr2[1]+"-"+arr2[0] + "'," + array[5]+")";
                        row++;
                    }

                    LoadData.Ins("INSERT INTO clients (surname,name,patronymic,dateofbirth,minutes) VALUES " + insert);
                    Loadd();
                    MessageBox.Show("Импорт произведён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                FILE.Close();
            }
        }
    }
}
