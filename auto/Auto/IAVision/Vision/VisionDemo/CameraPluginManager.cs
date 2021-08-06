using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDemo
{
    public class CameraPlugin
    {
        //private string sdkName;
        //private string sdkVersion;
        //private string dllName;

        //注意必须要写成如此形式DataGridView才能显示，写成public string SdkName形式DataGridView里面不显示
        public string SdkName { get; set; }
        public string SdkVersion { get; set; }
        public string DllName { get; set; }
    }



    public class CameraPluginManager
    {
        private static CameraPluginManager instance = new CameraPluginManager();
        public static CameraPluginManager Instance()
        {
            return instance;
        }

        public List<CameraPlugin> CameraPlugins;

        private CameraPluginManager()
        {
            CameraPlugins = CameraPluginHelper.Instance().CameraPluginList;
        }
    }
}
