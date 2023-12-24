using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PC_club
{
    public partial class arendaPC : Form
    {
        bool result;
        string rdr = "rent";
        int lol;
        int allCount = 0;
        int countPage = 0;
        int numberPage = 1;
        int rowIndex = 0;
        WordCreate word = new WordCreate();
        //Функция Инициализации
        public arendaPC(bool resul)
        {
            result = resul;
            InitializeComponent();
            allCount = Convert.ToInt32(LoadData.StrOne("select Count(*) from rent"));
            countPage = (int)Math.Truncate(allCount / 15.0);
            if(countPage==0)
            {
                countPage =1;
            }
            label5.Visible = false;
            UpdateUI();
            Obnova();
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button8.Enabled = false;
            button8.Visible = false;
            groupBox1.Visible = false;
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select surname from clients");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox1.Items.Add(LoadData.mas2[i]);
            }
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select number_pc from pc where cost=0");   
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox2.Items.Add(LoadData.mas2[i]);
            }
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select surname from employees");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox3.Items.Add(LoadData.mas2[i]);
            }
            Proverka();
            SaleCheck();
        }
        //Функция обновления данных
        public void Obnova()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_rent, clients.surname, out_data_rent, time,pc.number_pc,employees.surname, price from rent inner join clients on clients.id_clients=rent.idclients inner join pc on pc.id_pc=rent.idpc inner join employees on employees.id_employees=rent.idemployees Limit " + ((numberPage - 1) * 15) + ", 15");
            dataGridView1.Columns["id_rent"].Visible = false;
            dataGridView1.Columns["surname"].HeaderText = "Фамилия Клиента";
            dataGridView1.Columns["out_data_rent"].HeaderText = "Окончание аренды";
            dataGridView1.Columns["time"].HeaderText = "Время";
            dataGridView1.Columns["number_pc"].HeaderText = "Номер ПК";
            dataGridView1.Columns["surname1"].HeaderText = "Фамилия Сотрудника";
            dataGridView1.Columns["price"].HeaderText = "Цена";
        }
        //Функция проверки скидки
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
        }
        //Функция проверки на освободившиеся пк
        public void Proverka()
        {
            string oldTime = "";
            DateTime time = DateTime.Now;
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select out_data_rent from rent");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                oldTime = LoadData.mas2[i];
                if (oldTime != "")
                {
                    string[] arrayDate = oldTime.Split(' ');
                    string[] Date = arrayDate[0].Split('.');
                    string[] Time = arrayDate[1].Split(':');
                    DateTime oldRent = new DateTime(Convert.ToInt32(Date[2]), Convert.ToInt32(Date[1]), Convert.ToInt32(Date[0]), Convert.ToInt32(Time[0]), Convert.ToInt32(Time[1]), Convert.ToInt32(Time[2]));
                    TimeSpan diff1 = oldRent.Subtract(time);
                    if (diff1.ToString()[0] == '-')
                    {
                        Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
                        LoadData.ComboBox("select id_rent from rent where out_data_rent='" + oldTime + "'");
                        LoadData.Ins(String.Format("Insert into save_rent (id_rent, client, out_data_rent, time, number_pc,staff, price) VAlues({0}, (Select surname from clients where id_clients=(select idclients from rent where id_rent ={1})), (select out_data_rent from rent where id_rent ={2}), (select time from rent where id_rent ={3}),(Select number_pc from pc where id_pc=(select idpc from rent where id_rent ={4})),(Select surname from employees where id_employees=(select idemployees from rent where id_rent = {5})), (select price from rent where id_rent = {6}));", LoadData.mas2[0], LoadData.mas2[0], LoadData.mas2[0], LoadData.mas2[0], LoadData.mas2[0], LoadData.mas2[0], LoadData.mas2[0]));
                        LoadData.Ins("Update pc Set cost=0 where id_pc=(select idpc from rent where id_rent=" + LoadData.mas2[0] + ")");
                        LoadData.Ins("Delete from rent where id_rent=" + LoadData.mas2[0]);
                        LoadData.Ins("update save_rent set status='Оплачен' where id_rent=" + LoadData.mas2[0]);
                        Obnova();
                    }
                }
            }
        }
        //Функция для кнопки перехода
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }
        //Функция для кнопки добавления
        private void button1_Click(object sender, EventArgs e)
        {
            string lastId;
            if(textBox1.Text=="")
            {
                LoadData.Ins(String.Format("Insert into rent (idclients, out_data_rent, idpc,idemployees) VAlues((select id_clients from clients where surname='{0}'), '{1}', (select id_pc from pc where number_pc={2}),(select id_employees from employees where surname='{3}'));", comboBox1.Text, textBox1.Text, comboBox2.Text, comboBox3.Text));
                lastId = LoadData.StrOne("SELECT LAST_INSERT_ID();");
                Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
                LoadData.ComboBox(String.Format("select id_pc from pc where number_pc={0}", comboBox2.Text));
                LoadData.Ins(String.Format("update pc set cost=1 where id_pc={0}", LoadData.mas2[0]));
                Obnova();
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox2.Items.Clear();
                Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
                LoadData.ComboBox("Select number_pc from pc where cost=0");
                for (int i = 0; i < LoadData.mas2.Length; i++)
                {
                    comboBox2.Items.Add(LoadData.mas2[i]);
                }
                return;
            }
            string time = DateTime.Now.AddMinutes(Convert.ToDouble(textBox1.Text)).ToString();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("Select price_onemin from rate where id_rate=(select id_rate from pc where number_pc={0})", comboBox2.Text));
            string[] zxc = label5.Text.Split(' ');
            double price = Convert.ToDouble(zxc[1]);
            var res = MessageBox.Show("Ожидание оплаты. К оплате "+ Convert.ToInt32(price) + " руб.", "Оплата", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.No)
            {
                return;
            }
            LoadData.Ins(String.Format("Insert into rent (idclients, out_data_rent, time, idpc,idemployees, price) VAlues((select id_clients from clients where surname='{0}'), '{1}', '{2}', (select id_pc from pc where number_pc={3}),(select id_employees from employees where surname='{4}'), {5});", comboBox1.Text, time, textBox1.Text,comboBox2.Text, comboBox3.Text, price.ToString().Replace(',', '.')));
            lastId = LoadData.StrOne("SELECT LAST_INSERT_ID();");
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("select id_pc from pc where number_pc={0}", comboBox2.Text));
            LoadData.Ins(String.Format("update pc set cost=1 where id_pc={0}", LoadData.mas2[0]));

            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("select minutes from clients where surname='{0}'", comboBox1.Text));
            int min = Convert.ToInt32(LoadData.mas2[0]) + Convert.ToInt32(textBox1.Text);

            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("select id_clients from clients where surname='{0}'", comboBox1.Text));

            LoadData.Ins(String.Format("update clients set minutes={0} where id_clients={1}", min, LoadData.mas2[0]));

            Obnova();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.Items.Clear();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select number_pc from pc where cost=0");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox2.Items.Add(LoadData.mas2[i]);
            }
            InsOtchet(lastId);
            var result = MessageBox.Show("Создать чек", "Чек", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                if(!Peredacha(lastId))
                {
                    MessageBox.Show("Ошибка создания документа");
                }
            }
            else
            {
                return;
            }
        }
        //Функция для записи в документ
        public bool Peredacha(string lastId)
        {
            string client = LoadData.StrOne("(select surname from clients where id_clients = (select idclients from rent where id_rent ="+lastId+"))");
            string staff = LoadData.StrOne("(select surname from employees where id_employees=(select idemployees from rent where id_rent=" + lastId + "))");
            string pc = LoadData.StrOne("(select number_pc from pc where id_pc=(select idpc from rent where id_rent=" + lastId + "))");
            string date = LoadData.StrOne("(select out_data_rent from rent where id_rent =" + lastId + ")");
            string time = LoadData.StrOne("(select time from rent where id_rent =" + lastId + ")");
            string price = LoadData.StrOne("(select price from rent where id_rent =" + lastId + ")");
            return word.WriteWord(""+client+" "+staff+ " "+pc+ " "+date+ " "+time+ " "+price);
        }
        //Функция для кнопки удаление записи
        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                LoadData.Ins("Delete from rent where id_" + rdr + "=" + '"' + lol + '"' + ';');
                LoadData.Ins("update save_rent set status='Удален' where id_rent=" + lol);
                Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
                LoadData.ComboBox(String.Format("select id_pc from pc where number_pc={0}", comboBox2.Text));
                LoadData.Ins(String.Format("update pc set cost=0 where id_pc={0}", LoadData.mas2[0]));
            }
            else
            {
                return;
            }
            Obnova();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.Items.Clear();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select number_pc from pc where cost=0");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox2.Items.Add(LoadData.mas2[i]);
            }
            button2.Enabled = false;
            button3.Enabled = false;
        }
        //Функция для чтения клика по Данным
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string code;
            rowIndex = e.RowIndex;
            try
            {
                code = dataGridView1.Rows[rowIndex].Cells["id_" + rdr].Value.ToString();         
            }
            catch
            {
                return;
            }
            textBox1.Enabled = false;
            string strCmd = "Select id_rent, (select surname from clients where clients.id_clients=rent.idclients), (select number_pc from pc where pc.id_pc=rent.idpc), (select surname from employees where employees.id_employees=rent.idemployees), price   from rent Where id_" + rdr + "=" + '"' + code + '"' + ";";
            lol = Convert.ToInt32(code);
            LoadData.Reder(strCmd);
            string[] arr = new string[LoadData.mas.Length];
            arr = LoadData.mas;
            comboBox1.Text = arr[0];
            comboBox2.Items.Add(arr[1]);
            comboBox2.Text = arr[1];
            comboBox3.Text = arr[2];
            textBox1.Text =dataGridView1.Rows[rowIndex].Cells["time"].Value.ToString();
            button2.Enabled = true;
            button3.Enabled = true;
            button5.Enabled = true;
            label4.Visible = true;
            textBox1.Visible = true;
            label5.Visible = true;
            button8.Visible = false;
            if (dataGridView1.Rows[rowIndex].Cells["time"].Value.ToString() == "")
            {
                button8.Enabled = true;
                button2.Enabled = false;
                button5.Enabled = false;
                label4.Visible = false;
                textBox1.Visible = false;
                label5.Visible = false;
                button8.Visible = true;
            }
        }
        //Функция для кнопки изменения
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("Поля не заполнены");
            }
            LoadData.Ins(String.Format("Update pc SET cost=0 where number_pc={0}", dataGridView1.Rows[rowIndex].Cells["number_pc"].Value.ToString()));
            LoadData.Ins(String.Format("Update rent SET idpc=(select id_pc from pc where number_pc={0}), idclients=(select id_clients from clients where surname='{1}'), idemployees=(select id_employees from employees where surname='{2}'), where id_rent={3};", comboBox2.Text, comboBox1.Text,comboBox3.Text, lol));
            LoadData.Ins(String.Format("Update pc SET cost=1 where number_pc={0}", comboBox2.Text));
            Obnova();
            textBox1.Clear();
            comboBox2.Items.Clear();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select number_pc from pc where cost=0");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox2.Items.Add(LoadData.mas2[i]);
            }
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            button2.Enabled = false;
            button3.Enabled = false;
        }
        //Функция для отслеживания введенных данных
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[0-9]|[\b]").Success)
            {
                e.Handled = true;
            }
        }
        //Функция для кнопки создать чек
        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Создать чек", "Чек", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                if (!Peredacha(lol.ToString()))
                {
                    MessageBox.Show("Ошибка создания документа");
                }
            }
            else
            {
                return;
            }
        }
        //Функция для отправки на отчет
        public void InsOtchet(string lastId)
        {   
            LoadData.Ins(String.Format(@"Insert into save_rent (client, out_data_rent, time, number_pc,staff, price, status,id_rent) 
                                                                VAlues((select surname from clients where id_clients=(select idclients from rent where id_rent={0})), 
                                                                (select out_data_rent from rent where id_rent={1}), 
                                                                (select time from rent where id_rent={2}),
                                                                (select number_pc from pc where id_pc=(select idpc from rent where id_rent={3})),
                                                                (select surname from employees where id_employees=(select idemployees from rent where id_rent={4})), 
                                                                (select price from rent where id_rent={5}),
                                                                'Оплачен',
                                                                (select id_rent from rent where id_rent={6}));", lastId, lastId, lastId, lastId, lastId, lastId, lastId));
        }
        //Функция для отслежвания цены
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.TextLength==0)
            {
                label5.Visible = false;
                label8.Text = "Цена: ";
            }
            else
            {
                if (comboBox2.SelectedItem != null)
                {
                    label5.Visible = true;
                    Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
                    LoadData.ComboBox(String.Format("Select price_onemin from rate where id_rate=(select id_rate from pc where number_pc={0})", comboBox2.Text));
                    int skidka = 0;
                    if (comboBox1.SelectedItem != null)
                    {
                        string sale = "";
                        sale+= LoadData.StrOne("Select sale from sale_minuts where id_sale=(select idsale from clients where surname='" + comboBox1.Text + "')");
                        if (sale!="")
                        {
                            skidka = Convert.ToInt32(sale);
                        }
                    }
                    double price = Convert.ToDouble(textBox1.Text) * Convert.ToDouble(LoadData.mas2[0]);
                    price = price - (price * skidka/100);
                    label5.Text = "Цена: " + Convert.ToInt32(price) + " руб.";
                    label8.Text = "Цена: " + Convert.ToInt32(price) + " руб.";
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        //Функция для пагинации(назад на страницу)
        private void button6_Click(object sender, EventArgs e)
        {
            numberPage--;
            UpdateUI();
            Obnova();
        }
        //Функция для отслеживания кнопок для пагинации
        private void UpdateUI()
        {
            button6.Enabled = true;
            button7.Enabled = true;
            if (numberPage == 1)
                button6.Enabled = false;
            if (numberPage == countPage)
                button7.Enabled = false;
            label6.Text = numberPage + "/" + countPage;
        }
        //Функция для пагинации(вперед на страницу)
        private void button7_Click(object sender, EventArgs e)
        {
            numberPage++;
            UpdateUI();
            Obnova();
        }
        //Функция для открытия окна
        private void button8_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }
        //Функция для отслеживания цены
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Length != 0)
                {
                    Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
                    LoadData.ComboBox(String.Format("Select price_onemin from rate where id_rate=(select id_rate from pc where number_pc={0})", comboBox2.Text));
                    int skidka = 0;
                    double price;
                    if (comboBox1.SelectedItem != null)
                    {
                        string sale = "";
                        sale += LoadData.StrOne("Select sale from sale_minuts where id_sale=(select idsale from clients where surname='" + comboBox1.Text + "')");
                        if (sale != "")
                        {
                            skidka = Convert.ToInt32(sale);
                        }
                    }
                    price = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(LoadData.mas2[0]);
                    price = price - (price * skidka / 100);
                    label8.Text = "Цена: " + Convert.ToInt32(price) + " руб.";
                }
            }
            catch
            {
                label8.Text = "Цена: " + " руб.";
            }     
        }
        //Функция для открытия окна
        private void button9_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
        //Функция для оплаты
        private void button10_Click(object sender, EventArgs e)
        {
            string[] zxc = label8.Text.Split(' ');
            double price = Convert.ToDouble(zxc[1]);
            var res = MessageBox.Show("Ожидание оплаты. К оплате " + Convert.ToInt32(price) + " руб.", "Оплата", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.No)
            {
                return;
            }
            LoadData.Ins(String.Format("Update rent Set price={0}, out_data_rent='{1}', time={2} where id_rent={3}", price.ToString().Replace(',', '.'), DateTime.Now, textBox2.Text, lol));

            string lastId = lol.ToString();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("select id_pc from pc where number_pc={0}", comboBox2.Text));
            LoadData.Ins(String.Format("update pc set cost=0 where id_pc={0}", LoadData.mas2[0]));

            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("select minutes from clients where surname='{0}'", comboBox1.Text));
            int min = Convert.ToInt32(LoadData.mas2[0]) + Convert.ToInt32(textBox2.Text);

            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox(String.Format("select id_clients from clients where surname='{0}'", comboBox1.Text));

            LoadData.Ins(String.Format("update clients set minutes={0} where id_clients={1}", min, LoadData.mas2[0]));

            MessageBox.Show("ПК освобожден");
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.Items.Clear();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select number_pc from pc where cost=0");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox2.Items.Add(LoadData.mas2[i]);
            }
            InsOtchet(lastId);
            var result = MessageBox.Show("Создать чек", "Чек", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                if (!Peredacha(lastId))
                {
                    MessageBox.Show("Ошибка создания документа");
                }
            }
            LoadData.Ins("Delete from rent where id_rent=" + lol);
            Obnova();
            groupBox1.Visible = false;
            button8.Visible = false;
            label4.Visible = true;
            textBox1.Visible = true;
            button3.Enabled = false;
            textBox2.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            button3.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = true;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            textBox1.Text = "";
            textBox1.Visible = true;
            label4.Visible = true;
            button8.Visible = false;
            comboBox2.Items.Clear();
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select number_pc from pc where cost=0");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox2.Items.Add(LoadData.mas2[i]);
            }
        }
    }
}
