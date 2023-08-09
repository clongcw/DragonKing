using Aspose.Cells;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;
using CT_Border = NPOI.OpenXmlFormats.Wordprocessing.CT_Border;
using Document = Spire.Doc.Document;
using MessageBox = System.Windows.Forms.MessageBox;
using PrintDialog = System.Windows.Forms.PrintDialog;

namespace DragonKing.ViewModel
{
    public partial class OfficeViewModel : ObservableObject
    {
        [ObservableProperty]
        private object? _document;
        public OfficeViewModel()
        {


            ////创建 Document 类的对象
            //Document doc = new Document();

            ////载入Word文档
            //doc.LoadFromFile(Environment.CurrentDirectory + @"\666.docx");

            ////将文档保存为XPS文件
            //doc.SaveToFile(Environment.CurrentDirectory + @"\Word转XPS.xps", FileFormat.XPS);


            //LoadXpsDocument(Environment.CurrentDirectory + @"\666.xps",Document);

            //GenerateReportByTemplate();

        }


        private void LoadXpsDocument(string xpsFilePath)
        {
            // 创建 XpsDocument 对象并打开 XPS 文件
            using (XpsDocument xpsDocument = new XpsDocument(xpsFilePath, FileAccess.ReadWrite))
            {
                // 获取 XPS 文档的 FixedDocumentSequence
                FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();

                // 设置 DocumentViewer 的 Document 属性为 XPS 文档
                Document = fixedDocSeq;
            }
        }



        [RelayCommand]
        public void PreviewReport()
        {
            //Document doc = new Document();
            //#region 破解spire的license
            //var Lic = new Spire.License.InternalLicense();
            //Lic.LicenseType = Spire.License.LicenseType.Runtime;
            //Lic.AssemblyList = new string[] { "Spire.DocViewer.Wpf" };
            //var InternalLicense = doc.GetType().GetProperty("InternalLicense", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //InternalLicense.SetValue(doc, Lic);
            //#endregion
            //doc.LoadFromFile(Environment.CurrentDirectory + @"\666.docx");
            //doc.SaveToFile(Environment.CurrentDirectory + @"\666.xps", FileFormat.XPS);
            LoadXpsDocument(Environment.CurrentDirectory + @"\666.xps");

            //reportView.DataContext= this;
            //reportView.Show();
        }

        [RelayCommand]
        public void ChangeReport()
        {
            Document doc = new Document();
            #region 破解spire的license
            var Lic = new Spire.License.InternalLicense();
            Lic.LicenseType = Spire.License.LicenseType.Runtime;
            Lic.AssemblyList = new string[] { "Spire.DocViewer.Wpf" };
            var InternalLicense = doc.GetType().GetProperty("InternalLicense", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            InternalLicense.SetValue(doc, Lic);
            #endregion
            //doc.LoadFromFile(Environment.CurrentDirectory + @"\Report\20230808\666.docx");
            //doc.SaveToFile(Environment.CurrentDirectory + @"\Report\20230808\666.xps", FileFormat.XPS);

            LoadXpsDocument(Environment.CurrentDirectory + @"\Report\20230808\666.xps");


        }

        [RelayCommand]
        public void GenerateReportByTemplate()
        {
            string[] data = new string[500];
            data[0] = "0";
            data[1] = "张广达";
            data[2] = "测试诊断";
            data[3] = "测试真的";
            data[4] = "消化内科";
            data[5] = "男";
            data[6] = "消化内科";
            data[7] = "36565656";
            data[8] = "36";
            data[9] = "测试诊断";
            data[10] = "54646556";
            data[11] = "54646556";



            for (int i = 12; i < 500; i++)
            {
                data[i] = DateTime.Now.ToString();
            }

            using (FileStream stream = File.OpenRead(Environment.CurrentDirectory + "\\Configuration\\Template.docx"))
            {
                XWPFDocument doc = new XWPFDocument(stream);

                int iRow = 0; //表中行的循环索引
                int iCell = 0; //表中列的循环索引
                //遍历表格
                var tables = doc.Tables;
                foreach (XWPFTableRow row in tables[0].Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph para in cell.Paragraphs)
                        {
                            for (int i = 1; i < 12; i++)
                            {
                                if (para.Text == "<a" + i + ">")
                                {
                                    para.ReplaceText("<a" + i + ">", data[i]);
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < 24; i++)
                {
                    // 在表格下面新增一行
                    XWPFTableRow newRow = tables[1].CreateRow();
                    // 在新行中设置单元格内容（示例，可根据实际需求修改）
                    newRow.GetCell(0).SetParagraph(SetTableParagraph(tables[1], "hpiv1"));
                    newRow.GetCell(1).SetParagraph(SetTableParagraph(tables[1], "22.78"));
                    newRow.GetCell(2).SetParagraph(SetTableParagraph(tables[1], "阴性"));
                    newRow.GetCell(3).SetParagraph(SetTableParagraph(tables[1], "30-35"));
                }


                // 设置最后一行的单元格边框样式为黑线
                CT_Border border = new CT_Border
                {
                    val = ST_Border.single,
                    color = "000000" // 使用黑色 "#000000"
                };

                foreach (XWPFTableCell cell in tables[1].Rows.First().GetTableCells())
                {
                    CT_TcPr tcPr = cell.GetCTTc().tcPr;
                    if (tcPr == null)
                    {
                        tcPr = cell.GetCTTc().AddNewTcPr();
                    }
                    CT_VMerge vMerge = tcPr.AddNewVMerge();
                    vMerge.val = ST_Merge.restart;
                    tcPr.tcBorders = new CT_TcBorders();
                    tcPr.tcBorders.top = border;
                    tcPr.tcBorders.bottom = border;
                }

                foreach (XWPFTableCell cell in tables[1].Rows.Last().GetTableCells())
                {
                    CT_TcPr tcPr = cell.GetCTTc().tcPr;
                    if (tcPr == null)
                    {
                        tcPr = cell.GetCTTc().AddNewTcPr();
                    }
                    CT_VMerge vMerge = tcPr.AddNewVMerge();
                    vMerge.val = ST_Merge.restart;
                    tcPr.tcBorders = new CT_TcBorders();
                    tcPr.tcBorders.bottom = border;
                }


                foreach (XWPFTableRow row in tables[2].Rows)
                {

                    //iRow = tables[0].Rows.IndexOf(row);//获取该循环在List集合中的索引
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        //iCell = row.GetTableCells().IndexOf(cell);//获取该循环在List集合中的索引
                        foreach (XWPFParagraph para in cell.Paragraphs)
                        {
                            for (int i = 12; i < 20; i++)
                            {
                                if (para.Text == "<a" + i + ">")
                                {
                                    para.ReplaceText("<a" + i + ">", data[i]); //V1-V8
                                }
                            }
                        }
                    }
                }




                string reportfolderpath = Environment.CurrentDirectory + @"\Report\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(reportfolderpath))
                {
                    Directory.CreateDirectory(reportfolderpath);
                }

                string saveFile = Path.Combine(reportfolderpath, "测试报告" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx");
                FileStream out1 = new FileStream(saveFile, FileMode.Create);
                doc.Write(out1);
                out1.Close();

                MessageBox.Show("报告生成结束！");
            }
        }

        public void CreateWordReport()
        {
            // 保存文档
            using (FileStream stream = new FileStream("test.docx", FileMode.Create))
            {
                // 创建空白文档
                XWPFDocument doc = new XWPFDocument();

                //创建段落对象
                XWPFParagraph p1 = doc.CreateParagraph();
                XWPFParagraph p2 = doc.CreateParagraph();

                XWPFRun runTitle = p1.CreateRun();
                p1.Alignment = ParagraphAlignment.CENTER;
                runTitle.IsBold = true;
                runTitle.SetText("报告单");
                runTitle.FontSize = 22;
                runTitle.SetFontFamily("宋体", FontCharRange.None);//设置雅黑字体

                XWPFRun runTitle2 = p2.CreateRun();
                p2.Alignment = ParagraphAlignment.CENTER;
                runTitle2.IsBold = false;
                runTitle2.SetText("核酸扩增荧光定量PCR检验报告");
                runTitle2.FontSize = 8;
                runTitle2.SetFontFamily("宋体", FontCharRange.None);//设置雅黑字体


                XWPFTable table = doc.CreateTable(3, 8);
                table.Width = 5000;//总宽度
                table.SetColumnWidth(0, 625); /* 设置列宽 */
                table.SetColumnWidth(1, 625); /* 设置列宽 */
                table.SetColumnWidth(2, 625); /* 设置列宽 */
                table.SetColumnWidth(3, 625); /* 设置列宽 */
                table.SetColumnWidth(4, 625); /* 设置列宽 */
                table.SetColumnWidth(5, 625); /* 设置列宽 */
                table.SetColumnWidth(6, 625); /* 设置列宽 */
                table.SetColumnWidth(7, 625); /* 设置列宽 */




                table.GetRow(0).GetCell(0).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(1).GetCell(0).SetParagraph(SetTableParagraph(table, "患者类型"));
                table.GetRow(2).GetCell(0).SetParagraph(SetTableParagraph(table, "临床诊断"));

                table.GetRow(0).GetCell(1).SetParagraph(SetTableParagraph(table, "666"));
                table.GetRow(1).GetCell(1).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(2).GetCell(1).SetParagraph(SetTableParagraph(table, "姓名"));

                table.GetRow(0).GetCell(2).SetParagraph(SetTableParagraph(table, "性别"));
                table.GetRow(1).GetCell(2).SetParagraph(SetTableParagraph(table, "科室"));
                table.GetRow(2).GetCell(2).SetParagraph(SetTableParagraph(table, "住院号"));

                table.GetRow(0).GetCell(3).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(1).GetCell(3).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(2).GetCell(3).SetParagraph(SetTableParagraph(table, "姓名"));

                table.GetRow(0).GetCell(4).SetParagraph(SetTableParagraph(table, "年龄"));
                table.GetRow(1).GetCell(4).SetParagraph(SetTableParagraph(table, "样本类型"));
                table.GetRow(2).GetCell(4).SetParagraph(SetTableParagraph(table, "病历号"));

                table.GetRow(0).GetCell(5).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(1).GetCell(5).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(2).GetCell(5).SetParagraph(SetTableParagraph(table, "姓名"));

                table.GetRow(0).GetCell(6).SetParagraph(SetTableParagraph(table, "样本编号"));
                table.GetRow(1).GetCell(6).SetParagraph(SetTableParagraph(table, "病床/床号"));
                table.GetRow(2).GetCell(6).SetParagraph(SetTableParagraph(table, "采样日期"));

                table.GetRow(0).GetCell(7).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(1).GetCell(7).SetParagraph(SetTableParagraph(table, "姓名"));
                table.GetRow(2).GetCell(7).SetParagraph(SetTableParagraph(table, "姓名"));



                doc.Write(stream);
                stream.Close();
            }
        }

        /// <summary>  
        /// 创建Word文档中表格段落实例和设置表格段落文本的基本样式（字体大小，字体，字体颜色，字体对齐位置）
        /// </summary>  
        /// <param name="document">document文档对象</param>  
        /// <param name="table">表格对象</param>  
        /// <param name="fillContent">要填充的文字</param>  
        /// <param name="paragraphAlign">段落排列（左对齐，居中，右对齐）</param>
        /// <param name="textPosition">设置文本位置（设置两行之间的行间,从而实现表格文字垂直居中的效果），从而实现table的高度设置效果  </param>
        /// <param name="isBold">是否加粗（true加粗，false不加粗）</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontColor">字体颜色--十六进制</param>
        /// <param name="isItalic">是否设置斜体（字体倾斜）</param>
        /// <returns></returns>  
        public XWPFParagraph SetTableParagraph(XWPFTable table, string fillContent, ParagraphAlignment paragraphAlign = ParagraphAlignment.CENTER, bool isBold = false, int fontSize = 10, string fontColor = "000000", bool isItalic = false)
        {
            var para = new CT_P();
            //设置单元格文本对齐
            para.AddNewPPr().AddNewTextAlignment();


            XWPFParagraph paragraph = new XWPFParagraph(para, table.Body);//创建表格中的段落对象
            paragraph.Alignment = paragraphAlign;//文字显示位置,段落排列（左对齐，居中，右对齐）
            paragraph.VerticalAlignment = NPOI.XWPF.UserModel.TextAlignment.CENTER;//字体垂直对齐
            //paragraph.FontAlignment =Convert.ToInt32(ParagraphAlignment.CENTER);
            //字体在单元格内显示位置与 paragraph.Alignment效果相似

            XWPFRun xwpfRun = paragraph.CreateRun();//创建段落文本对象
            xwpfRun.SetText(fillContent);//内容填充
            xwpfRun.FontSize = fontSize;//字体大小
            xwpfRun.SetColor(fontColor);//设置字体颜色--十六进制
            xwpfRun.IsItalic = isItalic;//是否设置斜体（字体倾斜）
            xwpfRun.IsBold = isBold;//是否加粗
            xwpfRun.SetFontFamily("宋体", FontCharRange.None);//设置字体（如：微软雅黑,华文楷体,宋体）
            //xwpfRun.TextPosition=24;//设置文本位置（设置两行之间的行间），从而实现table的高度设置效果 
            return paragraph;
        }


        [RelayCommand]
        public void Print()
        {
            // 打开一个现有的Word文档
            Document document = new Document();

            #region 破解spire的license
            var Lic = new Spire.License.InternalLicense();
            Lic.LicenseType = Spire.License.LicenseType.Runtime;
            Lic.AssemblyList = new string[] { "Spire.DocViewer.Wpf" };
            var InternalLicense = document.GetType().GetProperty("InternalLicense", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            InternalLicense.SetValue(document, Lic);
            #endregion

            document.LoadFromFile(Environment.CurrentDirectory + @"\Report\20230807\888.docx");

            //初始化PrintDialog实例
            PrintDialog dialog = new PrintDialog();

            //设置打印对话框属性
            dialog.AllowPrintToFile = true;
            dialog.AllowCurrentPage = true;
            dialog.AllowSomePages = true;

            //设置文档打印对话框
            //doc = dialog;

            //显示打印对话框并点击确定执行打印
            PrintDocument printDoc = document.PrintDocument;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }


        [RelayCommand]
        public void Test()
        {
            WorkbookDesigner wb = new WorkbookDesigner(new Workbook());
            var style = new CellsFactory().CreateStyle();
            style.Borders.SetColor(System.Drawing.Color.Red);
            style.Font.Color = System.Drawing.Color.Red;
            wb.Workbook.Worksheets[0].Cells[0, 0].SetStyle(style);
            wb.Workbook.Worksheets[0].Cells[0, 0].PutValue("激活检测仅供学习参考！");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xlsx");
            wb.Workbook.Save(path);

        }
    }
}
