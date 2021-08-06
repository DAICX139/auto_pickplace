using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionUtility
{
   public class VisionMessage
    {
        /// <summary>
        /// 确认通知消息框:OK
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgOk(string s)
        {
            return MessageBox.Show(s, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 确认通知消息框:OK,CANCEK
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgQuestionOkCancel(string s)
        {
            return MessageBox.Show(s, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 确认通知消息框:YES,NO
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgQuestionYesNo(string s)
        {
            return MessageBox.Show(s, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 错误通知消息框:OK
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgErrorOk(string s)
        {
            return MessageBox.Show(s, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 错误通知消息框:ABORT,RETRY,IGNORE
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DialogResult MsgAbortRetryIgnore(string s)
        {
            return MessageBox.Show(s, "", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
        }
    }
}
