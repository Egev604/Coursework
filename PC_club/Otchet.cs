using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
namespace PC_club
{
    public partial class Otchet : Form
    {
        bool result;
        public Otchet(bool resul)
        {
            result = resul;
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Главная q = new Главная(result);
            q.Show();
        }

        private void Otchet_Load(object sender, EventArgs e)
        {
            DataTable tb = LoadData.Load("Select * from save_rent");
            DataTable zxc=tb;
            foreach (DataRow row in tb.Rows)
            {
                if (row["out_data_rent"].ToString().Split(' ')[0].Split('.')[1] != DateTime.Now.ToString("MM"))
                {
                    row.Delete();
                }
            }
            dataGridView1.DataSource = tb;
            dataGridView1.Columns["id_rent"].Visible = false;
            dataGridView1.Columns["id_save_rent"].Visible = false;
            dataGridView1.Columns["client"].HeaderText = "Клиент";
            dataGridView1.Columns["out_data_rent"].HeaderText = "Дата окончания";
            dataGridView1.Columns["time"].HeaderText = "Время";
            dataGridView1.Columns["number_pc"].HeaderText = "№PC";
            dataGridView1.Columns["staff"].HeaderText = "Сотрудник";
            dataGridView1.Columns["price"].HeaderText = "Цена";
            dataGridView1.Columns["status"].HeaderText = "Статус";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["status"].Value.ToString()=="Оплачен")
                {
                    dataGridView1.Rows[i].Cells["status"].Style.BackColor = Color.Green;
                }
                if (dataGridView1.Rows[i].Cells["status"].Value.ToString() == "Удален")
                {
                    dataGridView1.Rows[i].Cells["status"].Style.BackColor = Color.Red;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Word.Application application = new Word.Application();
            Word.Document document = application.Documents.Add();
            document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
            document.PageSetup.TopMargin = application.InchesToPoints(0.4f);
            document.PageSetup.LeftMargin = application.InchesToPoints(0.4f);
            document.PageSetup.RightMargin = application.InchesToPoints(0.4f);
            document.PageSetup.BottomMargin = application.InchesToPoints(0.4f);
            Word.Paragraph animalsPar = document.Paragraphs.Add();
            Word.Range animalsRange = animalsPar.Range;
            animalsRange.Font.Size = 14;
            animalsRange.Text = DateTime.Now.ToString("MM.yyyy");
            animalsPar.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            animalsRange.Bold = 1;
            animalsRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, 1, 7);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            tableParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            Word.Range cellRange;
            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Клиент";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Дата окончания";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Время";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "№PC";
            cellRange = paymentsTable.Cell(1, 5).Range;
            cellRange.Text = "Сотрудник";
            cellRange = paymentsTable.Cell(1, 6).Range;
            cellRange.Text = "Цена";
            cellRange = paymentsTable.Cell(1, 7).Range;
            cellRange.Text = "Статус";
            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            int y = 2;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            { 
                paymentsTable.Rows.Add();
                paymentsTable.Rows[y].Range.Bold = 0;
                cellRange = paymentsTable.Cell(y, 1).Range;
                cellRange.Text = row.Cells["client"].Value.ToString();
                cellRange = paymentsTable.Cell(y, 2).Range;
                cellRange.Text = row.Cells["out_data_rent"].Value.ToString();
                cellRange = paymentsTable.Cell(y, 3).Range;
                cellRange.Text = row.Cells["time"].Value.ToString();
                cellRange = paymentsTable.Cell(y, 4).Range;
                cellRange.Text = row.Cells["number_pc"].Value.ToString();
                cellRange = paymentsTable.Cell(y, 5).Range;
                cellRange.Text = row.Cells["staff"].Value.ToString();
                cellRange = paymentsTable.Cell(y, 6).Range;
                cellRange.Text = row.Cells["price"].Value.ToString();
                cellRange = paymentsTable.Cell(y, 7).Range;
                cellRange.Text = row.Cells["status"].Value.ToString();
                y++;
            }
            application.Visible = true;
            try
            {
                document.SaveAs(Directory.GetCurrentDirectory() + @"\Отчет за период"+ DateTime.Now.ToString("MM.yyyy") + ".docx");
            }
            catch
            {
                MessageBox.Show("Документ с таким названием и с такими же данными уже сущестует, можете выбрать другой период", "Внимание", MessageBoxButtons.OK);
            }
        }
    }
}
