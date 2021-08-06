using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionUtility
{
   public class VisionPath
    {
        //应用程序默认图形文件路径
        public static string image = Application.StartupPath + @"\demo.png";
        //应用程序相机配置文件路径
        public static string camera = Application.StartupPath + @"\cameras\CameraPlugins.xml";
        //应用程序用户手册文件路径
        public static string manual = Application.StartupPath + @"\manuals\";
    }
}
