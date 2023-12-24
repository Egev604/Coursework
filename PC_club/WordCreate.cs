using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
namespace PC_club
{
    class WordCreate
    {
        private readonly string FileName = Directory.GetCurrentDirectory() + @"\check.docx";
        public bool WriteWord(string allInfo)
        {
            string[] arr = allInfo.Split(' ');
            var application = new Word.Application();
            application.Visible = false;
            try
            {
                var wordDocument = application.Documents.Open(FileName);
                ReplaceWordStub("{client}", arr[0], wordDocument);
                ReplaceWordStub("{staff}", arr[1], wordDocument);
                ReplaceWordStub("{№PC}", arr[2], wordDocument);
                ReplaceWordStub("{date}", arr[3]+" "+arr[4], wordDocument);
                ReplaceWordStub("{time}", arr[5], wordDocument);
                ReplaceWordStub("{price}", arr[6], wordDocument);
                wordDocument.SaveAs2(Directory.GetCurrentDirectory() + @"\\чек_" + arr[0] + ".docx");
                application.Visible = true;
            }
            catch
            {
                return false;
            }
            return true;
        }
        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
    }
}
