using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using System.Drawing;
using AlcUtility;

namespace Poc2Auto.GUI
{
    public partial class UCRunLog : UserControl
    {
        public UCRunLog()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            this.richboxLog.ReadOnly = true;
            EventCenter.ProcessInfo += AddText;
        }

        private readonly object WriteLock = new object();

        private void AddText(string txt, ErrorLevel level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { AddRowData(txt, level); }));
                return;
            }
            AddRowData(txt, level);
        }

        private void AddRowData(string msg, ErrorLevel level)
        {
            //return;
            lock (WriteLock)
            {
                Color color = Color.Black;
                switch (level)
                {
                    case ErrorLevel.DEBUG:
                        color = Color.Blue;
                        break;
                    case ErrorLevel.WARNING:
                        color = Color.Brown;
                        break;
                    case ErrorLevel.INFO:
                        color = Color.Green;
                        break;
                    case ErrorLevel.FATAL:
                        color = Color.Red;
                        break;
                    default:
                        break;
                }
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");
                var msgs = $"{time} {level}-{msg}\r\n";
                richboxLog.SelectionStart = richboxLog.TextLength;
                richboxLog.SelectionLength = 0;
                richboxLog.SelectionColor = color;
                richboxLog.AppendText(msgs);
                richboxLog.SelectionColor = richboxLog.ForeColor;
            }

            var msgStr = $"{level} - {msg}";
            AlcSystem.Instance.Log(msgStr, "运行日志");
        }

        private void richboxLog_TextChanged(object sender, EventArgs e)
        {
            //LimitLine(500);
            if (this.richboxLog.Lines.Length > 500)
            {
                this.richboxLog.Clear();
            }
            richboxLog.SelectionStart = richboxLog.TextLength;
            richboxLog.ScrollToCaret();
        }

        private void LimitLine(int maxLine)
        {
            if (this.richboxLog.Lines.Length > maxLine)
            {
                string[] sLines = richboxLog.Lines;
                string[] sNewLines = new string[sLines.Length - maxLine];

                Array.Copy(sLines, maxLine, sNewLines, 0, sNewLines.Length);
                richboxLog.Lines = sNewLines;
            }
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            richboxLog.SelectAll();
        }

        private void CopyText_Click(object sender, EventArgs e)
        {
            richboxLog.Copy();
        }
    }
}
