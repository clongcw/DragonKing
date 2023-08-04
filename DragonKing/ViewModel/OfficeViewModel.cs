using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Windows.Documents;
using System.IO.Packaging;
using System.Windows.Xps.Serialization;
using System.Drawing.Printing;
using Spire.Doc;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using Microsoft.Win32;

namespace DragonKing.ViewModel
{
    public partial class OfficeViewModel : ObservableObject
    {
        [ObservableProperty]
        private object _word;
        public OfficeViewModel()
        {


            ////创建 Document 类的对象
            //Document doc = new Document();

            ////载入Word文档
            //doc.LoadFromFile(Environment.CurrentDirectory + @"\666.docx");

            ////将文档保存为XPS文件
            //doc.SaveToFile(Environment.CurrentDirectory + @"\Word转XPS.xps", FileFormat.XPS);


            LoadXpsDocument(Environment.CurrentDirectory + @"\Word转XPS.xps");

        }


        private void LoadXpsDocument(string xpsFilePath)
        {
            // 创建 XpsDocument 对象并打开 XPS 文件
            using (XpsDocument xpsDocument = new XpsDocument(xpsFilePath, FileAccess.Read))
            {
                // 获取 XPS 文档的 FixedDocumentSequence
                FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();

                // 设置 DocumentViewer 的 Document 属性为 XPS 文档
                Word = fixedDocSeq;
            }
        }

        void ImportDocument()
        {
            // Initializes the open file dialog.
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "All supported files (*.docx,*.doc,*.htm,*.html,*.rtf,*.txt,*.xaml)|*.docx;*.doc;*.htm;*.html;*.rtf;*.txt;*.xaml|Word Document (*.docx)|*.docx|Word 97 - 2003 Document (*.doc)|*.doc|Web Page (*.htm,*.html)|*.htm;*.html|Rich Text File (*.rtf)|*.rtf|Text File (*.txt)|*.txt|Xaml File (*.xaml)|*.xaml",
                FilterIndex = 1,
                Multiselect = false
            };

            if ((bool)openFileDialog.ShowDialog())
            {
                // Loads the file into RichTextBoxAdv.
                //richTextBoxAdv.Load(openFileDialog.FileName);
                // Sets the File name as Document Title.
                //richTextBoxAdv.DocumentTitle = openFileDialog.FileName.Remove(openFileDialog.FileName.LastIndexOf("."));
            }
        }


    }
}
