using System.Windows.Forms;

namespace Poc2Auto.GUI.UCModeUI
{
    public class LocationNameMgr
    {
        public static string Name { get; set; }

        public static void ShowNameEditor(FormStartPosition position = FormStartPosition.CenterParent)
        {
            new FormAxisPosName { StartPosition = position }.ShowDialog();
        }
    }
}
