using Aspose.Cells;
using Aspose.Slides;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Enum;
using DragonKing.Utils;
using MiniExcelLibs;
using MoonPdfLib;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;
using CT_Border = NPOI.OpenXmlFormats.Wordprocessing.CT_Border;
using Document = Aspose.Words.Document;
using ISlide = Aspose.Slides.ISlide;
using MessageBox = System.Windows.Forms.MessageBox;
using Presentation = Aspose.Slides.Presentation;
using PrintDialog = System.Windows.Forms.PrintDialog;

namespace DragonKing.ViewModel
{
    public partial class OfficeViewModel : ObservableObject
    {
        [ObservableProperty]
        private object? _document;
        [ObservableProperty]
        private string _excelPath = @"C:\Users\63214\Desktop\副本资金测算_公式.xlsx";
        [ObservableProperty]
        private string _templateExcelPath = @"C:\Users\63214\Desktop\模板12.xlsx";
        [ObservableProperty]
        private string _tmpPath = Environment.CurrentDirectory + @"\Report\";
        [ObservableProperty]
        private string _mergedName = @"C:\Users\63214\Desktop\小工具\ICON输出\merged.xlsx";
        [ObservableProperty]
        private string _sheetName = "Sheet2";
        [ObservableProperty]
        //纵向选择显示
        private bool _isControl1Visible = true;
        [ObservableProperty]
        //横向选择显示
        private bool _isControl2Visible = false;

        private Options _options = Options.纵向数据源;
        public Options Options
        {
            get { return _options; }
            set
            {
                _options = value;
                if (_options == Options.纵向数据源)
                {
                    IsControl1Visible = true;
                    IsControl2Visible = false;
                }
                else if (_options == Options.横向数据源)
                {
                    IsControl1Visible = false;
                    IsControl2Visible = true;
                }
                else
                {
                    IsControl1Visible = false;
                    IsControl2Visible = false;
                }
            }
        }
        [ObservableProperty]
        private TemplateOptions _templateOptions = TemplateOptions.附件12;
        [ObservableProperty]
        /// <summary>
        /// 开始列
        /// </summary>
        private string _startColumn = "B";
        [ObservableProperty]
        /// <summary>
        /// 结束列
        /// </summary>
        private string _endColumn = "Z";
        [ObservableProperty]
        /// <summary>
        /// 开始行
        /// </summary>
        private int _startRow = 3;
        [ObservableProperty]
        /// <summary>
        /// 结束行
        /// </summary>
        private int _endRow = 10;

        //生成文件进度
        private double schedule;
        public double Schedule
        {
            get => schedule;
            set => schedule = double.Parse(string.Format("{0:f2}", value));
        }

        public bool? IsEnabled = true;



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

            //PdfPanel = new MoonPdfPanel();

        }

        #region Excel
        [RelayCommand]
        public void OpenExcel()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ExcelPath = openFileDialog.FileName;
            }
        }

        [RelayCommand]
        public void OpenTemplateExcel()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                TemplateExcelPath = openFileDialog.FileName;
            }
        }

        [RelayCommand]
        public void OpenOutputDic()
        {
            // 创建 FolderBrowserDialog 对象
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // 显示选择文件夹对话框并获取结果
            DialogResult result = folderBrowserDialog.ShowDialog();

            // 处理结果
            if (result == DialogResult.OK)
            {
                // 用户选择了文件夹
                TmpPath = $"{folderBrowserDialog.SelectedPath}\\";
                // 执行处理逻辑
            }
        }

        [RelayCommand]
        public void Export()
        {
            //配置文件目录
            string dict = null;
            //IOUtil.Exists(dict);
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog()
            {
                Title = "请选择导出配置文件...",            //对话框标题
                Filter = "Excel Files(*.xlsx)|*.xlsx",    //文件格式过滤器
                FilterIndex = 1,                          //默认选中的过滤器
                FileName = "merged",                      //默认文件名
                DefaultExt = "xlsx",                      //默认扩展名
                InitialDirectory = dict,                  //指定初始的目录
                OverwritePrompt = true,                   //文件已存在警告
                AddExtension = true,                      //若用户省略扩展名将自动添加扩展名
            };
            if (sfd.ShowDialog() == true)
            {
                MergedName = sfd.FileName;
            }
        }


        [RelayCommand]
        public async Task OutputExcelsWithHorizontal()
        {
            List<string> sheetNames = MiniExcel.GetSheetNames(ExcelPath);

            var rows = MiniExcel.Query(ExcelPath, sheetName: SheetName).Cast<IDictionary<string, object>>().ToList();
            rows = rows.Skip(StartRow - 1).Take(EndRow - StartRow + 1).ToList();

            List<string> newdatas = new List<string>();
            Dictionary<string, object> value = new Dictionary<string, object>();
            int count = 0;
            foreach (var row in rows)
            {
                foreach (var item in row)
                {
                    if (item.Value != null)
                    {
                        newdatas.Add(item.Value.ToString());
                    }
                }

                for (int k = 0; k < newdatas.Count; k++)
                {
                    string key = "A" + k.ToString();
                    value.Add(key, newdatas[k]); // null代表暂时没有值
                }
                MiniExcel.SaveAsByTemplate(TmpPath + newdatas[0] + ".xlsx", TemplateExcelPath, value);
                await Task.Delay(10);
                value.Clear();
                newdatas.Clear();
                count++;
                Schedule = 50d / rows.Count * count;
            }

        }



        [RelayCommand]
        public async Task OutputExcelsWithVertical()
        {
            List<string> sheetNames = MiniExcel.GetSheetNames(ExcelPath);


            // 读取数据源文件
            var templateRows = MiniExcel.Query(TemplateExcelPath).ToList();
            List<string> newdatas = new List<string>();
            Dictionary<string, object> value = new Dictionary<string, object>();
            /*foreach (string sheetName in sheetNames)
            {*/
            //var rows = MiniExcel.Query(ExcelPath, sheetName: sheetName).ToList();
            //var rows = MiniExcel.Query(ExcelPath, sheetName: sheetName).Cast<IDictionary<string, object>>().ToList();
            //暂时只需要第一张表
            var rows = MiniExcel.Query(ExcelPath, sheetName: SheetName).Cast<IDictionary<string, object>>().ToList();
            /*string[] columns = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                 "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ"};*/

            string[] columns = ExcelHelper.GetColmunNames(StartColumn, EndColumn);

            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < rows.Count; j++)
                {
                    var row = rows[j];
                    if (row[columns[i]] != null)
                    {
                        var result = row[columns[i]].ToString();
                        newdatas.Add(result);
                    }
                    else
                    {
                        newdatas.Add("");
                    }
                }

                if (TemplateOptions == TemplateOptions.附件13)
                {
                    value.Add("name", newdatas[2]);
                    value.Add("id", newdatas[1]);

                    if (newdatas[0].Contains("2023"))
                    {
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "A" + (k - 1).ToString();
                            value.Add(key, newdatas[k]); // null代表暂时没有值
                        }
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "B" + (k - 1).ToString();
                            value.Add(key, null); // null代表暂时没有值
                        }
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "C" + (k - 1).ToString();
                            value.Add(key, null); // null代表暂时没有值
                        }
                    }

                    if (newdatas[0].Contains("2024"))
                    {
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "A" + (k - 1).ToString();
                            value.Add(key, null); // null代表暂时没有值
                        }
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "B" + (k - 1).ToString();
                            value.Add(key, newdatas[k]); // null代表暂时没有值
                        }
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "C" + (k - 1).ToString();
                            value.Add(key, null); // null代表暂时没有值
                        }
                    }

                    if (newdatas[0].Contains("2025"))
                    {
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "A" + (k - 1).ToString();
                            value.Add(key, null); // null代表暂时没有值
                        }
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "B" + (k - 1).ToString();
                            value.Add(key, null); // null代表暂时没有值
                        }
                        for (int k = 3; k < newdatas.Count; k++)
                        {
                            string key = "C" + (k - 1).ToString();
                            value.Add(key, newdatas[k]); // null代表暂时没有值
                        }
                    }

                    MiniExcel.SaveAsByTemplate(TmpPath + newdatas[2] + ".xlsx", TemplateExcelPath, value);
                }
                else
                {
                    value.Add("Name", newdatas[0]);
                    value.Add("Id", newdatas[1]);
                    for (int k = 3; k < newdatas.Count; k++)
                    {
                        string key = "A" + (k - 3).ToString();
                        value.Add(key, newdatas[k]); // null代表暂时没有值
                    }


                    MiniExcel.SaveAsByTemplate(TmpPath + newdatas[0] + ".xlsx", TemplateExcelPath, value);
                }



                value.Clear();
                newdatas.Clear();
                await Task.Delay(50);

                Schedule = 50d / (columns.Length - 1) * i;
            }
            /*}*/
        }

        [RelayCommand]
        public async Task MergeExcelFiles()
        {
            #region 生成Excel
            // 创建一个新的 Excel 文件
            var newFile = new FileInfo(MergedName);
            if (File.Exists(MergedName))
            {
                // 如果文件存在，删除文件
                File.Delete(MergedName);
            }

            // 创建一个新的工作簿，用于存储合并后的数据
            Workbook mergedWorkbook = new Workbook();
            mergedWorkbook.Worksheets.Clear();

            // 获取目录下所有 Excel 文件信息，并按照创建时间排序
            var files = new DirectoryInfo(TmpPath).GetFiles("*.xlsx")
                                                         .OrderBy(f => f.CreationTime)
                                                         .ToList();
            //文件数
            int count = 0;

            foreach (var file in files)
            {
                // 加载 Excel 文件
                Workbook inputWorkbook = new Workbook(file.ToString());

                // 遍历输入工作簿的每个工作表
                foreach (Worksheet worksheet in inputWorkbook.Worksheets)
                {
                    // 检查文件是否存在
                    if (File.Exists(Path.GetFileNameWithoutExtension(file.FullName)))
                    {
                        // 如果文件存在，删除文件
                        File.Delete(Path.GetFileNameWithoutExtension(file.FullName));
                    }

                    await Task.Delay(1);
                    count++;
                    //实时进度
                    Schedule = 50 + 50d / files.Count * count;
                    mergedWorkbook.Worksheets.Add(file.Name).Copy(worksheet);
                }
            }
            // 保存新文件
            mergedWorkbook.Save(MergedName);
            #endregion

            #region 将Excel转换为XPS
            Aspose.Cells.XpsSaveOptions xpsOptions = new Aspose.Cells.XpsSaveOptions();

            // Set the PrintType to FitSheetOnOnePage
            xpsOptions.OnePagePerSheet = true;

            // 设置输出文件路径和名称
            string xpsFilePath = Environment.CurrentDirectory + "\\Merged.xps";

            // 使用工作簿和 XPS 选项进行转换
            mergedWorkbook.Save(xpsFilePath, xpsOptions);
            LoadXpsDocument(xpsFilePath);
            #endregion




        }
        
        [RelayCommand]
        public async Task GenerateNewExcelFile()
        {
            Schedule = 0;
            if (!Regex.IsMatch(MergedName, "[a-gA-G]"))
            {
                App.Current.Dispatcher.Invoke((System.Action)(() =>
                {
                    MessageBoxX.Show(null, "请先点击左侧按钮！", "消息提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Warning);
                }));
                return;
            }

            if (string.IsNullOrEmpty(StartColumn) || string.IsNullOrEmpty(EndColumn))
            {
                App.Current.Dispatcher.Invoke((System.Action)(() =>
                {
                    MessageBoxX.Show(null, "请选择生成列数！", "消息提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Warning);
                }));
                return;
            }


            IsEnabled = false;
            //删除Tmp文件夹下所有文件
            //IOUtil.Exists(TmpPath);
            DirectoryInfo directory = new DirectoryInfo(TmpPath);
            if (directory != null)
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }

            if (Options == Options.横向数据源)
            {
                await OutputExcelsWithHorizontal();
            }
            else if (Options == Options.纵向数据源)
            {
                await OutputExcelsWithVertical();
            }
            else
            {
                App.Current.Dispatcher.Invoke((System.Action)(() =>
                {
                    MessageBoxX.Show(null, "当先数据源未定义！", "消息提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Warning);
                }));
                IsEnabled = true;
                return;
            }

            await MergeExcelFiles();

            App.Current.Dispatcher.Invoke((System.Action)(() =>
            {
                MessageBoxX.Show(null, "新文件已生成！", "消息提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Success);
            }));
            IsEnabled = true;
        }
        #endregion

        #region PPT



        private void LoadXpsDocument(string xpsFilePath)
        {
        Preview:
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto Preview;
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

            LoadXpsDocument(Environment.CurrentDirectory + @"\669.xps");



            //reportView.DataContext= this;
            //reportView.Show();
        }

        [RelayCommand]
        public void ChangeReport()
        {
            Document doc = new Document();
            //#region 破解spire的license
            //var Lic = new Spire.License.InternalLicense();
            //Lic.LicenseType = Spire.License.LicenseType.Runtime;
            //Lic.AssemblyList = new string[] { "Spire.DocViewer.Wpf" };
            //var InternalLicense = doc.GetType().GetProperty("InternalLicense", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //InternalLicense.SetValue(doc, Lic);
            //#endregion
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
                                    para.ReplaceText("<a" + i + ">", data[i]);
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

                string saveFile = Path.Combine(reportfolderpath, "测试报告" + ".docx");
                FileStream out1 = new FileStream(saveFile, FileMode.Create);
                doc.Write(out1);
                out1.Close();

                MessageBoxX.Show("报告生成结束!", "提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Success, DefaultButton.YesOK, 5);
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
            Spire.Doc.Document document = new Spire.Doc.Document();

            #region 破解spire的license
            //var Lic = new Spire.License.InternalLicense();
            //Lic.LicenseType = Spire.License.LicenseType.Runtime;
            //Lic.AssemblyList = new string[] { "Spire.DocViewer.Wpf" };
            //var InternalLicense = document.GetType().GetProperty("InternalLicense", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //InternalLicense.SetValue(document, Lic);
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
                MessageBoxX.Show("打印成功!", "提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Success, DefaultButton.YesOK, 5);
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


            MessageBoxX.Show("Asopose测试成功!", "提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Success, DefaultButton.YesOK, 5);
        }

        [RelayCommand]
        public async void ModifyPPT()
        {
            await Task.Run(() =>
             {
                 using (Presentation ppt = new Presentation(Environment.CurrentDirectory + @"\ppt.pptx"))
                 {

                     for (int i = 3; i < ppt.Slides.Count; i++)
                     {
                         ISlide slide = ppt.Slides[i];

                         foreach (IPictureFrame pictureFrame in slide.Shapes.OfType<IPictureFrame>())
                         {
                             pictureFrame.PictureFrameLock.AspectRatioLocked = false;//设置纵横比是否锁定
                                                                                     // 设置图片位置和大小
                             pictureFrame.X = 70;  // 设置横向位置
                             pictureFrame.Y = 120;  // 设置纵向位置
                             pictureFrame.Width = 820;  // 设置宽度
                             pictureFrame.Height = 400; // 设置高度
                         }
                     }
                     //ppt.Save("668.pptx", Aspose.Slides.Export.SaveFormat.Pptx);
                     ppt.Save("669.xps", Aspose.Slides.Export.SaveFormat.Xps);
                 }
                 App.Current.Dispatcher.Invoke((System.Action)(() =>
                 {
                     MessageBoxX.Show("PPT修改成功!", "提示", MessageBoxButton.OK, Panuon.WPF.UI.MessageBoxIcon.Success, DefaultButton.YesOK, 5);
                 }));

             });


        }
        #endregion

        #region MoonPdf

        [ObservableProperty]
        private MoonPdfPanel _pdfPanel;



        [RelayCommand]
        public void OpenFile()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog { Title = "Select PDF file...", DefaultExt = ".pdf", Filter = "PDF file (.pdf)|*.pdf", CheckFileExists = true };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    PdfPanel.OpenFile(dlg.FileName);
                    PdfPanel.ZoomToWidth();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("An error occured: " + ex.Message));
                }
            }
        }

        #endregion

    }
}
