using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.IO;
using VisionUtility;

namespace VisionDemo
{
    public class CameraPluginHelper
    {
        //此模式为程序使用到时再执行，如果写为公有字段形式，则再加载时就执行
        private static readonly CameraPluginHelper instance = new CameraPluginHelper();
        public static CameraPluginHelper Instance()
        {
            return instance;
        }
        private CameraPluginHelper()
        {
            path = VisionPath.camera;
            if (!File.Exists(path))
                return;
            try
            {
                xml = XElement.Load(path);
            }
            catch (Exception ex)
            {
                VisionMessage.MsgErrorOk(ex.Message + "\r\n" + ex.StackTrace);
            }

        }

        private readonly string path;
        private readonly XElement xml;

        public string Name
        {
            get
            {
                try
                {
                    return xml.Attribute("name").Value;
                }
                catch (Exception ex)
                {
                    VisionMessage.MsgErrorOk(ex.Message + "\r\n" + ex.StackTrace);
                    return null;
                }
            }
        }

        public string Version
        {
            get
            {
                try
                {
                    return xml.Attribute("version").Value;
                }
                catch (Exception ex)
                {
                    VisionMessage.MsgErrorOk(ex.Message + "\r\n" + ex.StackTrace);
                    return null;
                }
            }
        }

        public List<CameraPlugin> CameraPluginList
        {
            get
            {
                try
                {
                    List<CameraPlugin> cameraPluginList = new List<CameraPlugin>();

                    var xmlCameraPlugins = xml.Element("CameraPlugins");

                    foreach (var xmlCameraPlugin in xmlCameraPlugins.Descendants("CameraPlugin"))
                    {
                        CameraPlugin cameraPlugin = new CameraPlugin()
                        {
                            SdkName = xmlCameraPlugin.Element("相机SDK名称").Value,
                            SdkVersion = xmlCameraPlugin.Element("相机SDK版本").Value,
                            DllName = xmlCameraPlugin.Element("相机dll名称").Value,
                        };

                        cameraPluginList.Add(cameraPlugin);
                    }
                    return cameraPluginList;
                }
                catch (Exception ex)
                {
                    VisionMessage.MsgErrorOk(ex.Message + "\r\n" + ex.StackTrace);
                    return null;
                }
            }
        }



    }
}
