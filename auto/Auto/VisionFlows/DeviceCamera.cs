using AlcUtility;
using AlcUtility.Language;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionSDK;

namespace VisionFlows
{
    public partial class DeviceCamera : Form
    {
        private bool IsLoaded;
        public int WorkIndex;
        public DeviceCamera()
        {
            InitializeComponent();

            AlcSystem.Instance.LanguageChanged += LanguageChanged;
        }

        private void DeviceCamera_Load(object sender, EventArgs e)
        {
            foreach (var item in CameraManager.CameraList)
            {
                this.comboBox_imagelist.Items.Add(item.CameraID);
                item.ImageEvent_vido -= Item_ImageEvent_vido;
                item.ImageEvent_vido += Item_ImageEvent_vido;
            }
            if (CameraManager.CameraList.Count>0)
            {
                this.comboBox_imagelist.SelectedIndex = 0;
                trackBar_expor1.Enabled = true;
                trackBar_gen1.Enabled = true;
                numericUpDown_expor1.Enabled = true;
                numericUpDown_gen1.Enabled = true;
                button_continiu.Enabled = true;
                button_onece.Enabled = true;
                button_open.Text = "断开";
                button_open.Enabled = true;
            }
            IsLoaded = true;
        }

        private void LanguageChanged(string target, string current)
        {
            LanguageSwitch.SetForm(this,target,current);
        }

        private void Item_ImageEvent_vido(HImage hImage, string arg2)
        {
            try
            {
                this.superWind1.image = hImage.CopyImage();
                hImage.Dispose();
            }
            catch (Exception ex)
            {

            }

        }

        private void comboBox_imagelist_SelectedIndexChanged(object sender, EventArgs e)
        {

            {
                if (!IsLoaded || comboBox_imagelist.SelectedIndex < 0)
                {
                    button_open.Enabled = false;
                    return;
                }
                if (CameraManager.CameraById(comboBox_imagelist.SelectedItem.ToString()) != null && CameraManager.CameraById(comboBox_imagelist.SelectedItem.ToString()).IsConnected)
                {
                    button_open.Enabled = true;
                    trackBar_expor1.Enabled = true;
                    trackBar_gen1.Enabled = true;
                    numericUpDown_expor1.Enabled = true;
                    numericUpDown_gen1.Enabled = true;
                    button_continiu.Enabled = true;
                    button_onece.Enabled = true;
                    button_open.Text = "断开";
                }
                else
                {
                    button_open.Enabled = true;
                    trackBar_expor1.Enabled = false;
                    trackBar_gen1.Enabled = false;
                    numericUpDown_expor1.Enabled = false;
                    numericUpDown_gen1.Enabled = false;
                    button_continiu.Enabled = false;
                    button_onece.Enabled = false;
                    button_open.Text = "连接";
                }
            }
        }

        private void trackBar_expor1_Scroll(object sender, EventArgs e)
        {
            if (!IsLoaded)
                return;
            CameraManager.CameraById(comboBox_imagelist.SelectedItem.ToString()).ShuterCur = (long)trackBar_expor1.Value;
            numericUpDown_expor1.Value = trackBar_expor1.Value;
        }

        private void trackBar_gen1_Scroll(object sender, EventArgs e)
        {
            if (!IsLoaded)
                return;
            CameraManager.CameraById(comboBox_imagelist.SelectedItem.ToString()).GainCur = (long)trackBar_gen1.Value;
            numericUpDown_gen1.Value = trackBar_gen1.Value;
        }

        private void numericUpDown_expor1_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoaded)
                return;
            CameraManager.CameraById(comboBox_imagelist.SelectedItem.ToString()).ShuterCur = (long)numericUpDown_expor1.Value;
            trackBar_expor1.Value = (int)numericUpDown_expor1.Value;
        }

        private void numericUpDown_gen1_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoaded)
                return;
            CameraManager.CameraById(comboBox_imagelist.SelectedItem.ToString()).GainCur = (long)numericUpDown_gen1.Value;
            trackBar_gen1.Value = (int)numericUpDown_gen1.Value;
        }

        private void button_continiu_Click(object sender, EventArgs e)
        {
            var cc = CameraManager.CameraList;
            if (this.comboBox_imagelist.SelectedItem.ToString() == "RightTop")
            {
                WorkIndex = 2;
            }
            else if (this.comboBox_imagelist.SelectedItem.ToString() == "LeftTop")
            {
                WorkIndex = 1;
            }
            else if (this.comboBox_imagelist.SelectedItem.ToString() == "Bottom")
            {
                WorkIndex = 3;
            }
            if (button_continiu.Text == "连续")
            {
                button_open.Enabled = false;
                button_continiu.BackColor = Color.Red;
                button_onece.Enabled = false;
                CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString()).ShuterCur = (long)numericUpDown_expor1.Value;
                Plc.SetIO(WorkIndex, true);
                CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString())?.ContinuousShot();
                superWind1.image = CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString()).GrabImage(3000);
                button_continiu.Text = "停止";
            }
            else
            {
                Plc.SetIO(WorkIndex, false);
                button_open.Enabled = true;
                button_continiu.BackColor = Color.WhiteSmoke;
                button_onece.Enabled = true;
                CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString())?.SetSoftTrigger();
                button_continiu.Text = "连续";
            }
        }

        private void button_onece_Click(object sender, EventArgs e)
        {
            var camera = CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString());
            if (this.comboBox_imagelist.SelectedItem.ToString() == "RightTop")
            {
                WorkIndex = 2;
            }
            else if (this.comboBox_imagelist.SelectedItem.ToString() == "LeftTop")
            {
                WorkIndex = 1;
            }
            else if (this.comboBox_imagelist.SelectedItem.ToString() == "Bottom")
            {
                WorkIndex = 3;
            }
            if (camera != null)
            {
                Plc.SetIO(WorkIndex, true);
                var image = camera.GrabImage(2000);
                Plc.SetIO(WorkIndex, false);
                if (image == null)
                {
                    MessageBox.Show("抓拍失败");
                    return;
                }
                this.superWind1.image = image;

            }
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            if (button_open.Text == "连接")
            {
                var state = (bool)CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString())?.Connect();
                if (!state)
                {
                    MessageBox.Show("连接失败");
                    button_continiu.Enabled = false;
                    button_onece.Enabled = false;
                    trackBar_expor1.Enabled = false;
                    trackBar_gen1.Enabled = false;
                    return;
                }
                button_open.Text = "断开";
                button_continiu.Enabled = true;
                button_onece.Enabled = true;
                trackBar_expor1.Enabled = true;
                trackBar_gen1.Enabled = true;
            }
            else
            {
                button_open.Text = "连接";
                button_continiu.Enabled = false;
                button_onece.Enabled = false;
                trackBar_expor1.Enabled = false;
                trackBar_gen1.Enabled = false;
                CameraManager.CameraById(this.comboBox_imagelist.SelectedItem.ToString())?.Close();

            }
        }
    }
}
