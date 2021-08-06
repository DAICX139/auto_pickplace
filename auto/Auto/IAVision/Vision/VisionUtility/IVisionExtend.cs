using System.Collections.Generic;
using System.Windows.Forms;

namespace VisionUtility
{
    public interface IVisionExtend
    {
        Form GetForm();
        UserControl GetControl();
        Dictionary<string, ToolStripItem[]> GetMenu();
        void SetExtendForm(Form frm);
        void SetExtendMenu(ToolStripMenuItem menu);
        void SetFlowForm(Form frm);  
        void SetControl(UserControl uc);
        void SetUI();
  
    }
}
