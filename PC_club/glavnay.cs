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
using System.IO;
using System.Text.RegularExpressions;
namespace PC_club
{
    public partial class Главная : Form
    {
        bool result;
        public Главная(bool resul)
        {
            InitializeComponent();
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            result = resul;
            if(!resul)
            {
                button8.Enabled = false;
                button4.Enabled = false;
            }
            string[] dirs;
            try
            {
                dirs = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\\backup\\");
            }
            catch(FileNotFoundException)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory()+@"\\backup\\");
                dirs = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\\backup\\");
            }
            for (int i = 0; i < dirs.Length; i++)
            {
                dirs[i] = dirs[i].Replace(Directory.GetCurrentDirectory() + "\\\\backup\\\\", "");
                comboBox1.Items.Add(dirs[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите выйти из приложения?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(res==DialogResult.Yes)
            {
                string con = conectionString.connectionString;
                string backup = "backup" + DateTime.Now + ".sql";
                backup = backup.Replace(":", "-");
                string file = Directory.GetCurrentDirectory() + "\\backup\\" + backup;
                using (MySqlConnection conn = new MySqlConnection(con))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(file);
                            conn.Close();
                        }
                    }
                }
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text=="")
            {
                MessageBox.Show("Выберите файл");
                return;
            }
            string con = conectionString.connectionString;
            try
            {
                string file = Directory.GetCurrentDirectory() + "\\backup\\" + comboBox1.Text;
                using (MySqlConnection conn = new MySqlConnection(con))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ImportFromFile(file);
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            MessageBox.Show("База успешно восстановлена");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Otchet q = new Otchet(result);
            q.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Control q in this.Controls)
            {
                q.Enabled = false;
            }
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int min = Convert.ToInt32(textBox1.Text);
            int sale= Convert.ToInt32(textBox2.Text);
            LoadData.Ins(string.Format("Insert into sale_minuts (minuts, sale) values({0},{1})", min, sale));
            MessageBox.Show("Скидка создана");
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (Control q in this.Controls)
            {
                q.Enabled = true;
            }
            groupBox1.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (Control q in this.Controls)
            {
                q.Enabled = false;
            }
            groupBox2.Enabled = true;
            groupBox2.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (Control q in this.Controls)
            {
                q.Enabled = true;
            }
            groupBox2.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            autorizacia q = new autorizacia();
            q.Show();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            sale q = new sale(result);
            q.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[1-9 0]|[\b]").Success)
            {
                e.Handled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text.Length==0)
            {
                return;
            }
            if(Convert.ToInt32(textBox2.Text)>100)
            {
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
                textBox2.SelectionStart = 2;
                return;
            }
        }
    }
}
