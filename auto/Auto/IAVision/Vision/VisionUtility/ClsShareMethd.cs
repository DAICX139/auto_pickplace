using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;



namespace MeterAssemble
{
    public static class ClsShareMethd
    {
        #region"窗体设置函数"
        /// <summary>
        /// 自动调整指定窗口大小,默认窗体大小为1366*768
        /// </summary>
        /// <param name="frm">指定窗体</param>
        public static void AutoAdjustFormSize(Form frm)
        {
            Screen screen = Screen.PrimaryScreen;
            int screenWidth = screen.Bounds.Width;
            int screenHeight = screen.Bounds.Height;

            if (screenWidth == 1920 && screenHeight == 1080)
            {
                SetFormSize(frm, 1.405F, 1.405F, 1.0F);
            }
            else if (screenWidth == 1600 && screenHeight == 900)
            {
                SetFormSize(frm, 1.171F, 1.171F, 1.0F);
            }
            else if (screenWidth == 1280 && screenHeight == 720)
            {
                SetFormSize(frm, 0.937F, 0.937F, 0.937F);
            }
            else if (screenWidth == 800 && screenHeight == 600)
            {
                SetFormSize(frm, 0.585F, 0.585F, 0.585F);
            }
        }

        /// <summary>
        /// 设置指定窗体尺寸
        /// </summary>
        /// <param name="frm">指定窗体</param>
        /// <param name="frmW">指定窗体当前宽放大倍数</param>
        /// <param name="frmH">指定窗体当前高放大倍数</param>
        /// <param name="frmFontSize">指定窗体当前字体放大倍数</param>
        public static void SetFormSize(Form frm, float frmW, float frmH, float frmFontSize)
        {

            frm.Location = new Point((int)(frm.Location.X * frmW), (int)(frm.Location.Y * frmH));
            frm.Size = new Size((int)(frm.Size.Width * frmW), (int)(frm.Size.Height * frmH));
            frm.Font = new Font(frm.Font.Name, frm.Font.Size * frmFontSize, frm.Font.Style, frm.Font.Unit, frm.Font.GdiCharSet, frm.Font.GdiVerticalFont);
            if (frm.Controls.Count != 0)
            {
                SetControlSize(frm, frmW, frmH, frmFontSize);
            }
        }

        /// <summary>
        /// 设置指定窗体上所有控件尺寸
        /// </summary>
        /// <param name="ctl">指定窗体控件</param>
        /// <param name="ctlW">指定窗体控件当前宽放大倍数</param>
        /// <param name="ctlH">指定窗体控件当前高放大倍数</param>
        /// <param name="ctlFontSize">指定窗体控件当前字体放大倍数</param>
        public static void SetControlSize(Control ctl, float ctlW, float ctlH, float ctlFontSize)
        {
            foreach (Control c in ctl.Controls)
            {
                c.Location = new Point((int)(c.Location.X * ctlW), (int)(c.Location.Y * ctlH));
                c.Size = new Size((int)(c.Size.Width * ctlW), (int)(c.Size.Height * ctlH));
                c.Font = new Font(c.Font.Name, c.Font.Size * ctlFontSize, c.Font.Style, c.Font.Unit, c.Font.GdiCharSet, c.Font.GdiVerticalFont);
                if (c.Controls.Count != 0)
                {
                    SetControlSize(c, ctlW, ctlH, ctlFontSize);
                }
            }
        }
        #endregion

        #region 消息框函数集
        /// <summary>
        /// 确认通知消息框:OK
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgOk(string s)
        {
            return MessageBox.Show(s, ClsShareMethd.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 确认通知消息框:OK,CANCEK
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgQuestionOkCancel(string s)
        {
            return MessageBox.Show(s, ClsShareMethd.AssemblyTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 确认通知消息框:YES,NO
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgQuestionYesNo(string s)
        {
            return MessageBox.Show(s, ClsShareMethd.AssemblyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 错误通知消息框:OK
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgErrorOk(string s)
        {
            return MessageBox.Show(s, ClsShareMethd.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 错误通知消息框:ABORT,RETRY,IGNORE
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgAbortRetryIgnore(string s)
        {
            return MessageBox.Show(s, ClsShareMethd.AssemblyTitle, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
        }

        #endregion

        #region 程序集特性访问器

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        #region 常用函数
        /// <summary>
        /// 打开指定名称和格式的文件函数
        /// </summary>
        /// <param name="fileName">文件名，包含文件路径</param>
        /// <returns></returns>
        public static bool OpenProcessByFileName(string fileName)
        {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            try
            {
                myProcess.StartInfo.FileName = fileName;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.Close();
                myProcess.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                myProcess.Close();
                myProcess.Dispose();
                MsgErrorOk(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 时刻转换为时间格式
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        public static string DayHourMinuteSecond(long tick)
        {
            string day,hour,minute,second;

            day=string.Format("{0:D2}",tick/86400);
            hour = string.Format("{0:D2}", tick % 86400 / 3600);
            minute = string.Format("{0:D2}", tick % 86400 % 3600 / 60);
            second = string.Format("{0:D2}", tick % 86400 % 3600 % 60);

            return day + ":" + hour + ":" + minute + ":" + second;
        }


        //GetTickCount()<<=>>System.Environment.TickCount;
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int GetTickCount();

        public static void Delay(int tick)
        {
            int t;
            t=GetTickCount();

            do
            {
                Application.DoEvents();
            }
            while(GetTickCount()-t>tick);
        }

        /// <summary>
        /// 字符转十六禁止
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static byte[] StringToHexBytes(string strHex)
        {
            strHex = strHex.Replace(" ", "");
            if ((strHex.Length % 2) != 0)
                strHex += " ";
            byte[] returnBytes = new byte[strHex.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(strHex.Substring(i * 2, 2).Replace(" ", ""), 16);
            return returnBytes;
        }

        /// <summary>
        /// 创建.csv生产日志文件
        /// </summary>
        public static void CreateLogFile()
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string fileName;
            string fileColName;

            fileName = ClsShareData.appStartupPath + @"\Log\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            fileColName = DateTime.Now.ToString("yyyy/MM/dd") + "的机台运行日志...";
            ClsShareMethd.AddRunLogInfo("创建运行日志文件[" + DateTime.Now.ToString("yyyyMMdd") + ".csv]");

            try
            {
                fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //streamWriter = New StreamWriter(fileStream)将会出现乱码，一般出现乱码都是系统编码的问题
                //将编码方式设置为如下就可以了
                streamWriter = new StreamWriter(fileStream, Encoding.Unicode);
                streamWriter.WriteLine(fileColName);

                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message);
                ClsShareMethd.AddRunLogInfo("创建运行日志文件[" + DateTime.Now.ToString("yyyyMMdd") + ".csv]失败:"+ex.Message);
                return;
            }
        }

        /// <summary>
        /// 保存生产日志到.csv文件
        /// </summary>
        public static void UpdateLogFile(string log)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string fileName;
            string fileColContent;

            fileName = ClsShareData.appStartupPath + @"\Log\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            fileColContent = log;
            //fileColContent = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:") + log;

            try
            {
                fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                //streamWriter = New StreamWriter(fileStream)将会出现乱码，一般出现乱码都是系统编码的问题
                //将编码方式设置为如下就可以了
                streamWriter = new StreamWriter(fileStream, Encoding.Unicode);
                streamWriter.WriteLine(fileColContent);
                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                ClsShareMethd.MsgErrorOk(ex.Message);
                ClsShareMethd.AddRunLogInfo(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 创建DatabaseTable
        /// </summary>
        /// <returns></returns>
        public static bool CreatDatabaseTable()
        {
            bool blnFlag=false;
            
            for (int i = 0; i < ClsShareData.DatabaseTable.Count(); i++)
            {
                ClsShareData.DatabaseTable[i] = new DataTable();
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.TableID);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.TableName);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.FieldID);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.FieldEn);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.FieldCh);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.Used);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.Hide);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.Read);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.InputMode);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.ValidChk);
                ClsShareData.DatabaseTable[i].Columns.Add(StructDatabaseTable.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读数据库表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadDatabaseTableFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName = "";//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int tableID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.DatabaseTable.Count(); i++)
                {
                    ClsShareData.DatabaseTable[i].Rows.Clear();
                }

                if (ClsShareData.Role == EnumAccountRole.管理员)//管理员
                {
                    fileName = Application.StartupPath + "\\Config\\DatabaseTable.csv";
                }
                else if (ClsShareData.Role ==  EnumAccountRole.工程师)//工程师
                {
                    fileName = Application.StartupPath + "\\Config\\DatabaseTable.csv";
                }
                else//员工
                {
                    fileName = Application.StartupPath + "\\Config\\DatabaseTable.csv";
                }

                ClsShareMethd.AddRunLogInfo("读取数据库表文件[DatabaseTable.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream,Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TABLEID")
                        { 
                            //DONOTHING
                        }
                        else
                        {
                            tableID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.DatabaseTable[tableID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.DatabaseTable[tableID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.DatabaseTable.Count(); i++)
                    {
                        ClsShareData.DatabaseTable[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[DatabaseTable.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("数据库表文件[DatabaseTable.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(+"文件[DatabaseTable.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取数据库表文件[DatabaseTable.csv]失败："+ex.Message);
                return false;
            }

            return true;
        }


        /// <summary>
        /// 创建SystemParameter表
        /// </summary>
        /// <returns></returns>
        public static bool CreatSystemParameter()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.SystemParameter.Count(); i++)
            {
                ClsShareData.SystemParameter[i] = new DataTable();
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.TypeID);
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.TypeName);
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.ParaID);
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.ParaName);
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.ParaValue);
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.Used);
                ClsShareData.SystemParameter[i].Columns.Add(StructSystemParameter.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读系统参数表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadSystemParameterFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.SystemParameter.Count(); i++)
                {
                    ClsShareData.SystemParameter[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\SystemParameter.csv";

                ClsShareMethd.AddRunLogInfo("读取系统参数表文件[SystemParameter.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break;

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.SystemParameter[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.SystemParameter[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.SystemParameter.Count(); i++)
                    {
                        ClsShareData.SystemParameter[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[SystemParameter.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("系统参数表文件[SystemParameter.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[SystemParameter.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取系统参数表文件[SystemParameter.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建报警目录表
        /// </summary>
        /// <returns></returns>
        public static bool CreatAlarmCategory()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.AlarmCategory.Count(); i++)
            {
                ClsShareData.AlarmCategory[i] = new DataTable();
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.TypeID);
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.TypeName);
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.AlarmID);
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.AlarmAddress);
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.AlarmReason);
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.AlarmSolution);
                ClsShareData.AlarmCategory[i].Columns.Add(StructAlarmCategory.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读报警目录表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadAlarmCategoryFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.AlarmCategory.Count(); i++)
                {
                    ClsShareData.AlarmCategory[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\AlarmCategory.csv";

                ClsShareMethd.AddRunLogInfo("读取报警目录表文件[AlarmCategory.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break;

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.AlarmCategory[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.AlarmCategory[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.AlarmCategory.Count(); i++)
                    {
                        ClsShareData.AlarmCategory[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[AlarmCategory.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("报警目录表文件[AlarmCategory.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[AlarmCategory.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取报警目录表文件[AlarmCategory.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建CommonButton表
        /// </summary>
        /// <returns></returns>
        public static bool CreatCommonButton()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.CommonButton.Count(); i++)
            {
                ClsShareData.CommonButton[i] = new DataTable();
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.TypeID);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.TypeName);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.ButtonID);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.ButtonAddress);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.ButtonName);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.WatchUsed);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.ControlUsed);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.WorkNo);
                ClsShareData.CommonButton[i].Columns.Add(StructCommonButton.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读CommonButton表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadCommonButtonFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.CommonButton.Count(); i++)
                {
                    ClsShareData.CommonButton[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\CommonButton.csv";

                ClsShareMethd.AddRunLogInfo("读取通用按钮表文件[CommonButton.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break;

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.CommonButton[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.CommonButton[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.CommonButton.Count(); i++)
                    {
                        ClsShareData.CommonButton[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[CommonButton.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("通用按钮表文件[CommonButton.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[CommonButton.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取通用按钮表文件[CommonButton.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建ServoButton表
        /// </summary>
        /// <returns></returns>
        public static bool CreatServoButton()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.ServoButton.Count(); i++)
            {
                ClsShareData.ServoButton[i] = new DataTable();
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.TypeID);
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.TypeName);
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.ButtonID);
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.ButtonAddress);
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.ButtonName);
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.Used);
                ClsShareData.ServoButton[i].Columns.Add(StructServoButton.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读ServoButton表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadServoButtonFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.ServoButton.Count(); i++)
                {
                    ClsShareData.ServoButton[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\ServoButton.csv";

                ClsShareMethd.AddRunLogInfo("读取伺服按钮表文件[ServoButton.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break; 

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.ServoButton[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.ServoButton[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.ServoButton.Count(); i++)
                    {
                        ClsShareData.ServoButton[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[ServoButton.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("伺服按钮表文件[ServoButton.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[ServoButton.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取伺服按钮表文件[ServoButton.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建CylinderButton表
        /// </summary>
        /// <returns></returns>
        public static bool CreatCylinderButton()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.CylinderButton.Count(); i++)
            {
                ClsShareData.CylinderButton[i] = new DataTable();
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.TypeID);
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.TypeName);
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.ButtonID);
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.ButtonAddress);
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.ButtonName);
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.Used);
                ClsShareData.CylinderButton[i].Columns.Add(StructCommonButton.WorkNo);
                ClsShareData.CylinderButton[i].Columns.Add(StructCylinderButton.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读CylinderButton表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadCylinderButtonFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.CylinderButton.Count(); i++)
                {
                    ClsShareData.CylinderButton[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\CylinderButton.csv";

                ClsShareMethd.AddRunLogInfo("读取气缸按钮表文件[CylinderButton.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break;

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.CylinderButton[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.CylinderButton[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.CylinderButton.Count(); i++)
                    {
                        ClsShareData.CylinderButton[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[CylinderButton.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("气缸按钮表文件[CylinderButton.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[CylinderButton.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取气缸按钮表文件[CylinderButton.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建IOSignal表
        /// </summary>
        /// <returns></returns>
        public static bool CreatPlcIOSignal()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.PlcIOSignal.Count(); i++)
            {
                ClsShareData.PlcIOSignal[i] = new DataTable();
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.TypeID);
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.TypeName);
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.SignalID);
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.SignalAddress);
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.SignalName);
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.Used);
                ClsShareData.PlcIOSignal[i].Columns.Add(StructPlcIOSignal.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读IOSignal表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadPlcIOSignalFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.PlcIOSignal.Count(); i++)
                {
                    ClsShareData.PlcIOSignal[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\PlcIOSignal.csv";

                ClsShareMethd.AddRunLogInfo("读取PLC IO信号表文件[PlcIOSignal.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break;

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.PlcIOSignal[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.PlcIOSignal[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.PlcIOSignal.Count(); i++)
                    {
                        ClsShareData.PlcIOSignal[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[IOSignal.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("PLC IO信号表文件[PlcIOSignal.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[PlcIOSignal.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取Plc IO信号表文件[PlcIOSignal.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建OperateCondition表
        /// </summary>
        /// <returns></returns>
        public static bool CreatOperateCondition()
        {
            bool blnFlag = false;

            for (int i = 0; i < ClsShareData.OperateCondition.Count(); i++)
            {
                ClsShareData.OperateCondition[i] = new DataTable();
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.TypeID);
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.TypeName);
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.ConditionID);
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.ConditionAddress);
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.ConditionName);
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.Used);
                ClsShareData.OperateCondition[i].Columns.Add(StructOperateCondition.Remark);
            }

            blnFlag = true;
            return blnFlag;
        }

        /// <summary>
        /// 读Condition表文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadOperateConditionFile()
        {
            FileStream fileStream;
            StreamReader streamReader;
            string fileName;//文件路径
            string readLine = "";//记录每次读取的一行记录
            string[] record = null;//记录每行记录中的各字段内容
            int typeID;
            DataRow dr;

            try
            {
                for (int i = 0; i < ClsShareData.OperateCondition.Count(); i++)
                {
                    ClsShareData.OperateCondition[i].Rows.Clear();
                }

                fileName = Application.StartupPath + "\\Config\\OperateCondition.csv";

                ClsShareMethd.AddRunLogInfo("读取运行条件信号表文件[OperateCondition.csv]");

                //Directory.Exists(fileName);//判断目录是否存在
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //streamReader = new StreamReader(fileStream, Encoding.Unicode);
                    streamReader = new StreamReader(fileStream, Encoding.Default);

                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        if (readLine == "")
                            break;

                        record = readLine.Split(',');
                        if (record[0].ToUpper() == "TYPEID")
                        {
                            //DONOTHING
                        }
                        else
                        {
                            typeID = Convert.ToInt32(record[0]);

                            dr = ClsShareData.OperateCondition[typeID].NewRow();

                            for (int i = 0; i <= record.GetUpperBound(0); i++)
                            {
                                dr[i] = record[i];
                            }
                            ClsShareData.OperateCondition[typeID].Rows.Add(dr);
                        }
                    }

                    streamReader.Close();
                    fileStream.Close();

                    for (int i = 1; i < ClsShareData.OperateCondition.Count(); i++)
                    {
                        ClsShareData.OperateCondition[i].AcceptChanges();
                    }
                }
                else
                {
                    //ClsShareMethd.MsgErrorOk("文件[OperateCondition.csv]不存在！");
                    ClsShareMethd.AddRunLogInfo("运行条件信号表文件[OperateCondition.csv]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ClsShareMethd.MsgErrorOk(ex.Message + "文件[OperateCondition.csv]读取失败！");
                ClsShareMethd.AddRunLogInfo("读取运行条件表文件[PlcIOSignal.csv]失败：" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool SaveConfigFile(StructConfigFile file)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string fileName = "";
            string fileContent = "";

            fileName = Application.StartupPath + "\\Config\\" + file.FileName + ".csv";

            try
            {
                fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                //streamWriter = New StreamWriter(fileStream)将会出现乱码，一般出现乱码都是系统编码的问题
                //将编码方式设置为如下就可以了
                //streamWriter = new StreamWriter(fileStream, Encoding.Unicode);
                streamWriter = new StreamWriter(fileStream, Encoding.Default);
                streamWriter.WriteLine(file.FileTitle);

                for (int i = 0; i < file.Table.GetLength(0); i++)
                {
                    file.Table[i].AcceptChanges();

                    for (int r = 0; r < file.Table[i].Rows.Count; r++)
                    {
                        DataRow dr = file.Table[i].Rows[r];
                        fileContent = "";

                        for (int c = 0; c < file.Table[i].Columns.Count; c++)
                        {
                            if (fileContent == "")
                            {
                                fileContent = dr[c].ToString().Trim();
                            }
                            else
                            {
                                fileContent = fileContent + "," + dr[c].ToString();
                            }
                        }

                        streamWriter.WriteLine(fileContent);
                    }
                }

                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                ClsShareMethd.MsgErrorOk(ex.Message);
                ClsShareMethd.AddRunLogInfo(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 格式化DataGridView控件
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="dt">DataGridView控件格式化的数据表</param>
        /// <param name="dm">DataGridView控件的编辑模式</param>
        public static void FormatDataGridView(DataGridView dgv, DataTable dt, EnumDataMode dm)
        {
            try
            {
                DataRow dr;

                int readMode;
                int inputMode;

                if (dt.Rows.Count == 0)
                {
                    return;
                }

                //if (dgv.Columns.Count == 0)
                //{
                //    return;
                //}

                if (dm == EnumDataMode.Add | dm == EnumDataMode.Update)
                {
                    dgv.ReadOnly = false;
                }
                else
                {
                    dgv.ReadOnly = true;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    dgv.Columns[i].HeaderText = (dr[StructDatabaseTable.FieldCh] + "").Trim();

                    if (dm == EnumDataMode.Add || dm == EnumDataMode.Update)
                    {
                        readMode = (int)Convert.ToInt32((dr[StructDatabaseTable.Read] + "").Trim());
                        inputMode = (int)Convert.ToInt32((dr[StructDatabaseTable.InputMode] + "").Trim());

                        if (readMode == (int)EnumReadMode.Yes && inputMode == (int)EnumInputMode.Manual)
                        {
                            dgv.Columns[i].ReadOnly = true;
                            dgv.Columns[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                        }
                        else if (readMode == (int)EnumReadMode.Yes && inputMode == (int)EnumInputMode.Pick)
                        {
                            dgv.Columns[i].ReadOnly = true;
                            dgv.Columns[i].DefaultCellStyle.BackColor = Color.LightCyan;
                        }
                        else if (readMode == (int)EnumReadMode.No && inputMode == (int)EnumInputMode.Manual)
                        {
                            dgv.Columns[i].ReadOnly = false;
                            dgv.Columns[i].DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        dgv.Columns[i].ReadOnly = false;
                        dgv.Columns[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }

                dgv.RowHeadersWidth = 20;
                dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dgv.ColumnHeadersVisible = true;
            }
            catch (Exception ex)
            {
                ClsShareMethd.MsgErrorOk(ex.ToString()+"");
            }
        }

        /// <summary>
        /// 禁止DataGridView列排序
        /// </summary>
        /// <param name="dgv"></param>
        public static void ForbidColumnSortable(DataGridView dgv)
        {
            for (int i=0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// 初始化复选框列表隐藏字段
        /// </summary>
        /// <param name="clb"></param>
        /// <param name="dtStruct"></param>
        public static void IniHideField(CheckedListBox clb, DataTable dtStruct)
        {
            DataRow dr;

            if (dtStruct.Rows.Count == 0)
            {
                return;
            }

            clb.Items.Clear();

            for (int i = 0; i < dtStruct.Rows.Count; i++)
            {
                dr = dtStruct.Rows[i];
                clb.Items.Add((dr[StructDatabaseTable.FieldCh] + "").Trim());

                if (Convert.ToInt32(dr[StructDatabaseTable.Hide]) == (int)EnumHide.Yes)
                {
                    clb.SetItemCheckState(i, CheckState.Checked);
                }

            }

            clb.SelectedIndex = 0;
        }

        /// <summary>
        /// 设置DataGridView的隐藏字段
        /// </summary>
        /// <param name="clb"></param>
        /// <param name="dgv"></param>
        public static void SetHideField(CheckedListBox clb, DataGridView dgv)
        {
            for (int i = 0; i < clb.Items.Count; i++)
            {
                if (i < dgv.Columns.Count)
                {
                    if (clb.GetItemChecked(i) == true)
                    {
                        dgv.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgv.Columns[i].Visible = true;
                    }
                }
            }

        }

        /// <summary>
        /// 对不能为空的字段进行有效性检查
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStruct"></param>
        /// <param name="info"></param>
        public static void DataValidCheck(DataTable dt, DataTable dtStruct, ref string info)
        {
            DataRow dr;
            DataRow drStruct;

            info = "";

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                dr = dt.Rows[r];

                for (int c = 0; c < dtStruct.Rows.Count; c++)
                {
                    drStruct = dtStruct.Rows[c];

                    if (Convert.ToInt32(drStruct[StructDatabaseTable.ValidChk]) == (int)EnumValidCheck.Yes)
                    {
                        if ((dr[c] + "").Trim() == "")
                        {
                            if (info == "")
                            {
                                info = (drStruct[StructDatabaseTable.FieldCh] + "").Trim();
                            }
                            else
                            {
                                info = info + "," + (drStruct[StructDatabaseTable.FieldCh] + "").Trim();
                            }
                        }
                    }
                }

                if (info != "")
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumeric(String strNumeric)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumeric) &&
                 !objTwoDotPattern.IsMatch(strNumeric) &&
                 !objTwoMinusPattern.IsMatch(strNumeric) &&
                 objNumberPattern.IsMatch(strNumeric);
        }

        /// <summary>
        /// 一个单元格一个的写入，龟速写入，不建议使用
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dgv"></param>
        public static void ExportToExcel(string fileName, DataGridView dgv)
        {
            if (dgv.SelectedRows.Count>0)//当前有行被选中
            {
                string saveFileName = "";

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xlsx";
                saveDialog.Filter = "Excel文件|*.xlsx";
                saveDialog.FileName = fileName;
                saveDialog.ShowDialog();

                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0) 
                    return; //被点了取消   

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
               
                if (xlApp == null)
                {
                    ClsShareMethd.MsgErrorOk("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }

                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1  

                int k=0;//当前列索引
                for (int i = 0; i < dgv.ColumnCount; i++)//写入标题 
                {
                    if (dgv.Columns[i].Visible == true)
                    {
                        //worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                        worksheet.Cells[1, k + 1] = dgv.Columns[i].HeaderText;
                        ++k;
                    }
                }

                int index = 0;
                for (int r = 0; r < dgv.Rows.Count; r++)//写入数值
                {
                    if (dgv.Rows[r].Selected)
                    {
                        k = 0;
                        for (int i = 0; i < dgv.ColumnCount; i++)
                        {
                            if (dgv.Columns[i].Visible == true)
                            {
                                //worksheet.Cells[index + 2, i + 1] = dgv.Rows[r].Cells[i].Value;
                                worksheet.Cells[index + 2, k + 1] = dgv.Rows[r].Cells[i].Value;
                                ++k;
                            }
                        }
                        ++index;
                        System.Windows.Forms.Application.DoEvents();
                    }
                }

                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应

                if (saveFileName != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(saveFileName);
                    }
                    catch (Exception ex)
                    {
                        ClsShareMethd.MsgErrorOk(ex.Message.ToString());
                    }
                }

                xlApp.Quit();
                GC.Collect();//强行销毁   
            }
            else
            {
                ClsShareMethd.MsgErrorOk("当前无行被选中，请通过行标题选择需要导出的行!");
            }

        }

        /// <summary>
        /// 结束Excel进程
        /// </summary>
        public static void KillExcelProcess(bool bAll)
        {
            if (bAll)
            {
                KillAllExcelProcess();
            }
            else
            {
                //KillSpecialExcel();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        /// <summary>
        /// 杀特殊进程的Excel
        /// </summary>
        public static void KillSpecialExcel(Excel.Application excelApp)
            
        {
            try
            {
                if (excelApp != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId((IntPtr)excelApp.Hwnd, out lpdwProcessId);

                    if (lpdwProcessId > 0)    //c-s方式
                    {
                        System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                    }
                }
            }
            catch 
            { 
            }
        }

        /// <summary>
        /// 杀Excel进程
        /// </summary>
        public static void KillAllExcelProcess()
        {
            Process[] myProcesses;
            myProcesses = Process.GetProcessesByName("Excel");

            //得不到Excel进程ID，暂时只能判断进程启动时间
            foreach (Process myProcess in myProcesses)
            {
                myProcess.Kill();
            }
        }

        public static void ImportFromExcel(ref DataTable dt, string fileName)
        {
            string strConn = "Provider = Microsoft.Ace.OLEDB.12.0;" + "Data Source=" + fileName + ";" + "Extended Properties = 'Excel 12.0; HDR=Yes; IMEX=1'";
            //OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [Sheet1$]", strConn);//Sheet1$是Excel默认的表名,如果改动,select * from [Sheet1$]"将查询失败
            //string strConn = "Provider = Microsoft.Ace.OLEDB.12.0;" + "Data Source=" + fileName + ";" + "Extended Properties = 'Excel 8.0; HDR=Yes; IMEX=1'";
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [First Sheet$]", strConn);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                ClsShareMethd.MsgErrorOk(ex.Message);
            }
        }

        public static Parity ToParity(string str = "None")
        {
            //// 摘要:
            //// 为 System.IO.Ports.SerialPort 对象指定奇偶校验位。
            //public enum Parity
            //{
            //    // 摘要:
            //    //     不发生奇偶校验检查。
            //    None = 0,
            //    //
            //    // 摘要:
            //    //     设置奇偶校验位，使位数等于奇数。
            //    Odd = 1,
            //    //
            //    // 摘要:
            //    //     设置奇偶校验位，使位数等于偶数。
            //    Even = 2,
            //    //
            //    // 摘要:
            //    //     将奇偶校验位保留为 1。
            //    Mark = 3,
            //    //
            //    // 摘要:
            //    //     将奇偶校验位保留为 0。
            //    Space = 4,
            //}

            if (str == "Odd")
                return Parity.Odd;
            else if (str == "Even")
                return Parity.Even;
            else if (str == "Mark")
                return Parity.Mark;
            else if (str == "Space")
                return Parity.Space;
            else
                return Parity.None;

        }

        public static StopBits ToStopBits(string str = "One")
        {
            //// 摘要:
            //// 指定在 System.IO.Ports.SerialPort 对象上使用的停止位数。
            //public enum StopBits
            //{
            //    // 摘要:
            //    //     不使用停止位。不支持此值。将 System.IO.Ports.SerialPort.StopBits 属性设置为 System.IO.Ports.StopBits.None
            //    //     将引发 System.ArgumentOutOfRangeException。
            //    None = 0,
            //    //
            //    // 摘要:
            //    //     使用一个停止位。
            //    One = 1,
            //    //
            //    // 摘要:
            //    //     使用两个停止位。
            //    Two = 2,
            //    //
            //    // 摘要:
            //    //     使用 1.5 个停止位。
            //    OnePointFive = 3,
            //}

            if (str == "None")
                return StopBits.None;
            else if (str == "Two")
                return StopBits.Two;
            else if (str == "OnePointFive")
                return StopBits.OnePointFive;
            else
                return StopBits.One;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public static void AddRunLogInfo(string info)
        {
            //Monitor.Enter(ClsShareData.RunLogInfo);
            if (ClsShareData.RunLogInfo.Equals(""))
            {
                ClsShareData.RunLogInfo = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss->") + info + "...";
            }
            else
            {
                ClsShareData.RunLogInfo = ClsShareData.RunLogInfo + Environment.NewLine + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss->") + info + "...";
            }
            //Monitor.Exit(ClsShareData.RunLogInfo);
        }

        #endregion

        #region 本项目通用函数

        /// <summary>
        /// 获取当前产品相关参数
        /// </summary>
        public static bool GetCurrentProduct()
        {
            try
            {
                ClsShareData.database.QueryWorksheet(ref ClsShareData.Worksheet, ClsShareData.LineID, EnumWoStatus.生产中.ToString());

                if (ClsShareData.Worksheet.Rows.Count ==1)
                {
                    DataRow dr = ClsShareData.Worksheet.Rows[0];
                    ClsShareData.ProdDatabaseID = Convert.ToInt32(dr[StructWorksheet.ID].ToString().Trim());
                    ClsShareData.ProdID = dr[StructWorksheet.ProdID].ToString().Trim();
                    ClsShareData.ProdName = dr[StructWorksheet.ProdName].ToString().Trim();
                    ClsShareData.PlanQty = Convert.ToInt32(dr[StructWorksheet.PlanQty].ToString().Trim());
                    ClsShareData.StartSN = Convert.ToInt32(dr[StructWorksheet.StartSN].ToString().Trim());
                    ClsShareData.StartDate = Convert.ToDateTime(dr[StructWorksheet.StartDate].ToString());

                    ClsShareMethd.AddRunLogInfo("当前产线发布产品为[" + ClsShareData.ProdID + "]");
                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]发布相关参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("当前产线未发布产品,请检测");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]发布相关参数异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前产品编码相关参数
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentProductCode()
        {
            try
            {
                ClsShareData.database.QueryProductCode(ref ClsShareData.ProductCode, ClsShareData.LineID, ClsShareData.ProdID);
                if (ClsShareData.ProductCode.Rows.Count > 0)
                {
                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]编码参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]编码参数失败");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]编码参数异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前产品工艺相关参数
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentProcessPara()
        {
            try
            {
                ClsShareData.database.QueryProductProcess(ref ClsShareData.ProductProcess, ClsShareData.LineID, ClsShareData.ProdID, ClsShareData.DevID);
                if (ClsShareData.ProductProcess.Rows.Count > 0)
                {
                    ClsShareData.ProcID.Clear();
                    ClsShareData.ProcName.Clear();
                    ClsShareData.ProcTargetVal.Clear();
                    ClsShareData.ProcUpperVal.Clear();
                    ClsShareData.ProcLowerVal.Clear();
                    ClsShareData.ProcKVal.Clear();
                    ClsShareData.ProcBVal.Clear();
                    ClsShareData.ProcIsUsed.Clear();

                    for (int i = 0; i < ClsShareData.ProductProcess.Rows.Count; i++)
                    {
                        DataRow dr = ClsShareData.ProductProcess.Rows[i];

                        //以下变量主要用于上位机,保持与写入伺服的参数同步即可
                        ClsShareData.ProcID.Add(dr[StructProductProcess.ProcID].ToString().Trim());
                        ClsShareData.ProcName.Add(dr[StructProductProcess.ProcName].ToString().Trim());
                        ClsShareData.ProcTargetVal.Add(Convert.ToDouble(dr[StructProductProcess.TargetVal].ToString().Trim()));
                        ClsShareData.ProcUpperVal.Add(Convert.ToDouble(dr[StructProductProcess.UpperVal].ToString().Trim()));
                        ClsShareData.ProcLowerVal.Add(Convert.ToDouble(dr[StructProductProcess.LowerVal].ToString().Trim()));
                        ClsShareData.ProcKVal.Add(Convert.ToDouble(dr[StructProductProcess.KVal].ToString().Trim()));
                        ClsShareData.ProcBVal.Add(Convert.ToDouble(dr[StructProductProcess.BVal].ToString().Trim()));
                        ClsShareData.ProcIsUsed.Add(Convert.ToBoolean(dr[StructProductProcess.IsUsed].ToString().Trim()));
                    }

                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]工艺参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]工艺参数失败");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]工艺参数异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前产品伺服参数
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentServoPara()
        {
            try
            {
                ClsShareData.database.QueryProductServo(ref ClsShareData.ProductServo, ClsShareData.LineID, ClsShareData.ProdID, ClsShareData.DevID);
                if (ClsShareData.ProductServo.Rows.Count > 0)
                {
                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]伺服参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]伺服参数失败");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]伺服参数异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前产品特征参数
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentFeaturePara()
        {
            try
            {
                ClsShareData.database.QueryProductManage(ref ClsShareData.ProductManage, ClsShareData.LineID, ClsShareData.ProdID);
                if (ClsShareData.ProductManage.Rows.Count > 0)
                {
                    DataRow dr = ClsShareData.ProductManage.Rows[0];
                    ClsShareData.ProdFeature = (dr[StructProductManage.ProdFeature].ToString()).Split(',');

                    //获取特征标识ID
                    int val1 = GetFeatureValue(ClsShareData.ProdFeature[(int)EnumProdFeature.产品类型]);
                    int val2 = GetFeatureValue(ClsShareData.ProdFeature[(int)EnumProdFeature.产品尺寸]);
                    int val3 = GetFeatureValue(ClsShareData.ProdFeature[(int)EnumProdFeature.固定环]);


                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]特征参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]特征参数失败");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]特征参数异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前产品设备参数
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentProductPara()
        {
            try
            {
                ClsShareData.database.QueryProductPara(ref ClsShareData.ProductPara, ClsShareData.LineID, ClsShareData.ProdID, ClsShareData.DevID);
               
                if (ClsShareData.ProductPara.Rows.Count > 0)
                {
                    DataRow dr = ClsShareData.ProductPara.Rows[0];
                    ClsShareData.RobotManualVel=Convert.ToDouble(dr[StructProductPara.Para0].ToString().Trim());
                    ClsShareData.RobotAutoVel = Convert.ToDouble(dr[StructProductPara.Para1].ToString().Trim());
                    ClsShareData.PlugRowNum = Convert.ToInt32(dr[StructProductPara.Para2].ToString().Trim());
                    ClsShareData.PlugColNum = Convert.ToInt32(dr[StructProductPara.Para3].ToString().Trim());
                    ClsShareData.PlugRowGap = Convert.ToDouble(dr[StructProductPara.Para4].ToString().Trim());
                    ClsShareData.PlugColGap = Convert.ToDouble(dr[StructProductPara.Para5].ToString().Trim());
                    ClsShareData.HandRowNum = Convert.ToInt32(dr[StructProductPara.Para6].ToString().Trim());
                    ClsShareData.HandEvenRowColNum = Convert.ToInt32(dr[StructProductPara.Para7].ToString().Trim());
                    ClsShareData.HandOddRowColNum = Convert.ToInt32(dr[StructProductPara.Para8].ToString().Trim());
                    ClsShareData.HandRowGap = Convert.ToDouble(dr[StructProductPara.Para9].ToString().Trim());
                    ClsShareData.HandColGap = Convert.ToDouble(dr[StructProductPara.Para10].ToString().Trim());

                    ClsShareData.HandIsGlue = Convert.ToDouble(dr[StructProductPara.Para11].ToString().Trim());
                    ClsShareData.HandIsRise = Convert.ToDouble(dr[StructProductPara.Para12].ToString().Trim());
                    ClsShareData.HandNum =  Convert.ToDouble(dr[StructProductPara.Para13].ToString().Trim());

                    ClsShareData.DownloadSoftTitle = dr[StructProductPara.Para14].ToString().Trim();
                    ClsShareData.DownloadProgramName = dr[StructProductPara.Para15].ToString().Trim();
                    ClsShareData.DebuggerConnectedString = dr[StructProductPara.Para16].ToString().Trim();
                    ClsShareData.TargetDetectedString = dr[StructProductPara.Para17].ToString().Trim();
                    ClsShareData.DownloadSuccessString = dr[StructProductPara.Para18].ToString().Trim();
                    ClsShareData.DownloadX = Convert.ToInt32(dr[StructProductPara.Para19].ToString().Trim());
                    ClsShareData.DownloadY = Convert.ToInt32(dr[StructProductPara.Para20].ToString().Trim());

                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]参数失败");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]参数异常:" + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 获取当前产品机械手参数
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentRobotPara()
        {
            try
            {
                ClsShareData.database.QueryProductRobot(ref ClsShareData.ProductRobot, ClsShareData.LineID, ClsShareData.ProdID, ClsShareData.DevID);
                if (ClsShareData.ProductRobot.Rows.Count > 0)
                {
                    ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]机械手参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]机械手参数失败");
                return false;
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("读产品[" + ClsShareData.ProdID + "]伺服参数异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 更新当前设备状态
        /// </summary>
        /// <returns></returns>
        public static bool UpdateCurrentDeviceStatus()
        {
            try
            {
                ClsShareData.database.QueryDeviceStatusByDate(ref ClsShareData.DeviceStatus, ClsShareData.LineID, ClsShareData.DevID, Convert.ToDateTime("00:00:00").ToString(), Convert.ToDateTime("23:59:59").ToString());
                if (ClsShareData.DeviceStatus.Rows.Count == 0)
                {
                    DataRow drDeviceStatus = ClsShareData.DeviceStatus.NewRow();

                    drDeviceStatus[StructDeviceStatus.LineID] = ClsShareData.LineID;
                    drDeviceStatus[StructDeviceStatus.DevID] = ClsShareData.DevID;
                    drDeviceStatus[StructDeviceStatus.Editor] = ClsShareData.User;
                    drDeviceStatus[StructDeviceStatus.EditDate] = DateTime.Now.ToString();
                    drDeviceStatus[StructDeviceStatus.TableName] = EnumDatabaseTable.device_status.ToString();

                    ClsShareData.DeviceStatus.Rows.Add(drDeviceStatus);
                    ClsShareData.database.UpdateDatabase(ClsShareData.DeviceStatus, EnumDataMode.Add, EnumDatabaseTable.device_status.ToString());
                    ClsShareData.database.QueryDeviceStatusByDate(ref ClsShareData.DeviceStatus, ClsShareData.LineID, ClsShareData.DevID, Convert.ToDateTime("00:00:00").ToString(), Convert.ToDateTime("23:59:59").ToString());
                }

                if (ClsShareData.DeviceStatus.Rows.Count == 1)
                {
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.LineID] = ClsShareData.LineID;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.ProdID] = ClsShareData.ProdID;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.DevID] = ClsShareData.DevID;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.DevName] = ClsShareData.DevName;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.ProdStatus] = ClsShareData.ProdStatus;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.PlanProd] = ClsShareData.PlanQty;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.ActualProd] = ClsShareData.ProdTotalNum;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.ProdRate] = 0.0;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.StartTime] = ClsShareData.StartTime;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.RunTime] = ClsShareData.AutoTime;
                    ClsShareData.DeviceStatus.Rows[0][StructDeviceStatus.DevRate] = 0.0;

                    ClsShareData.database.UpdateDatabase(ClsShareData.DeviceStatus, EnumDataMode.Update, EnumDatabaseTable.device_status.ToString());

                    //ClsShareMethd.AddRunLogInfo("更新设备状态参数成功");
                    return true;
                }

                ClsShareMethd.AddRunLogInfo("更新设备状态参数失败");
                return false;
            }

            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("更新设备状态参数异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前产品最新序列号
        /// </summary>
        /// <param name="codeRule"></param>
        /// <param name="prodDate"></param>
        /// <param name="prodCode"></param>
        public static void GetProductSerialNumber(DataTable codeRule, DateTime prodDate, ref string prodCode)
        {
            DataRow dr;
            int ruleID = -1;

            string prodID = "";

            string curYear = "";
            string curMonth = "";
            string curDay = "";
            //string curWeek = "";

            string ruleDesc = "";
            string codeFormat = "";
            string codeDisp = "";
            string tempValue = "";

            prodCode = "";

            for (int r = 0; r < codeRule.Rows.Count; r++)
            {
                dr = codeRule.Rows[r];
                prodID = dr[StructProductCode.ProdID].ToString().Trim();
                ruleDesc = dr[StructProductCode.CodeRule].ToString().Trim();
                codeDisp = dr[StructProductCode.IsUsed].ToString().Trim();

                if (codeDisp != "True")
                    continue;

                GetCodeRuleID(ruleDesc, ref ruleID);

                if (ruleID == -1)
                    return;

                tempValue = "";
                switch (ruleID)
                {
                    case (int)EnumCodeRule.FixedText:
                        tempValue = dr[StructProductCode.CodeValue].ToString().Trim();
                        break;
                    case (int)EnumCodeRule.YearReplaceCode:
                        curYear = prodDate.ToString("yyyy");
                        GetReplaceCode(ruleDesc, curYear, ref tempValue);
                        break;
                    case (int)EnumCodeRule.MonthReplaceCode:
                        curMonth = prodDate.ToString("MM");
                        GetReplaceCode(ruleDesc, curMonth, ref tempValue);
                        break;
                    case (int)EnumCodeRule.DayReplaceCode:
                        curDay = prodDate.ToString("dd");
                        GetReplaceCode(ruleDesc, curDay, ref tempValue);
                        break;
                    case (int)EnumCodeRule.SerialNumberByDay:
                        int maxSN = 0;
                        GetMaxserialNumber(prodID, prodDate.ToString("yyyy-MM-dd"), ref maxSN);

                        if (maxSN > 0)
                            maxSN = maxSN + 1;
                        else
                            maxSN = 1;

                        codeFormat = dr[StructProductCode.CodeFormat].ToString().Trim();
                        tempValue = maxSN.ToString(codeFormat);
                        break;
                    case (int)EnumCodeRule.Other:
                        break;
                    default:
                        break;
                }

                prodCode = prodCode + tempValue;
            }
        }

        /// <summary>
        /// 获取编码规则ID，后续可优化为数据库
        /// </summary>
        /// <param name="codeRule"></param>
        /// <returns></returns>
        public static void GetCodeRuleID(string codeRule, ref int ruleID)
        {
            ruleID = -1;

            if (codeRule.Length >= 2)
            {
                switch (codeRule.Substring(0, 2))
                {
                    case "固定":
                        ruleID = 0;
                        break;
                    case "年替":
                        ruleID = 1;
                        break;
                    case "月替":
                        ruleID = 2;
                        break;
                    case "日替":
                        ruleID = 3;
                        break;
                    case "按天":
                        ruleID = 11;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获取年、月、日替换码
        /// </summary>
        /// <param name="codeRule"></param>
        /// <param name="orig"></param>
        /// <param name="result"></param>
        public static void GetReplaceCode(string codeRule, string orig, ref string result)
        {
            int intPosi;
            int intTmpPosi;
            string tmpRuleDesc = "";

            int intStart = 0;
            string[] myRes = null;
            string[] myOrig = null;
            string tmpValue = "";

            intPosi = codeRule.IndexOf("[");

            if (intPosi > 0)
            {
                tmpRuleDesc = codeRule.Substring(intPosi + 1);
                tmpRuleDesc = tmpRuleDesc.Replace("]", "");
                myRes = tmpRuleDesc.Split(',');
            }

            intPosi = codeRule.IndexOf("(");
            if (intPosi > 0)
            {
                tmpRuleDesc = codeRule.Substring(intPosi + 1);
                intTmpPosi = tmpRuleDesc.IndexOf(")");
                if (intTmpPosi > 0)
                {
                    tmpRuleDesc = tmpRuleDesc.Substring(0, intTmpPosi);
                    myOrig = tmpRuleDesc.Split('-');
                }
            }

            if (myOrig.Length > 0 & myRes.Length > 0)
            {
                intStart = Convert.ToInt32(myOrig[0]);
                tmpValue = myRes[Convert.ToInt32(orig) - intStart];
            }

            result = tmpValue;
        }

       /// <summary>
       /// 获取产品最新序列号
       /// </summary>
       /// <param name="prodID"></param>
       /// <param name="prodDate"></param>
       /// <param name="maxSN"></param>
        public static void GetMaxserialNumber(string prodID, string prodDate, ref int maxSN)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            string str = "";
            maxSN = 0;

            try
            {
                ClsShareData.database.QueryProductStatisByDate(ref dt, ClsShareData.LineID, ClsShareData.DevID, prodID, prodDate);
                //

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    str = dr[StructProductStatis.MarkCode].ToString().Trim();
                    maxSN = Convert.ToInt32(str.Substring(str.Length - 4, 4));
                }
            }
            catch (Exception ex)
            {
                ClsShareMethd.AddRunLogInfo("获取最大序列号错误：" + ex.Message);
                return;
            }

        }


















        /// <summary>
        /// 根据当前特征值返回对应整数标识ID
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static int GetFeatureValue(string feature)
        {
            int val = -1;

            try
            {
                switch (feature)
                {
                    case "导光针":
                        val = 0;
                        break;
                    case "圆盘针":
                        val = 1;
                        break;
                    case "60mm":
                        val = 0;
                        break;
                    case "85mm":
                        val = 1;
                        break;
                    case "100mm":
                        val = 2;
                        break;
                    case "无固定环":
                        val = 0;
                        break;
                    case "有固定环":
                        val = 1;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                //val = -1;
            }
            finally
            {
            }

            return val;
        }







        #endregion



    }














    }

