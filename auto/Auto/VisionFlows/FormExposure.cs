using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionFlows
{
    public partial class FormExposure : Form
    {
        public FormExposure()
        {
            InitializeComponent();
        }
        private void FormExposure_Load(object sender, EventArgs e)
        {
            //下相机
            txtExposure_DownCamScan.Text = ImagePara.Instance.Exposure_DownCamScan.ToString("f0");
            txtExposure_LeftCamShotTray.Text = ImagePara.Instance.Exposure_LeftCamGetDUT.ToString("f0");
            txtExposure_LeftCamPutTray.Text = ImagePara.Instance.Exposure_LeftCamGetDUT.ToString("f0");
            txtExposure_RightCamCheckTray.Text = ImagePara.Instance.Exposure_RightCamCheckTray.ToString("f0");
            txtExposure_RightCamPutTray.Text = ImagePara.Instance.Exposure_RightCamGetDUT.ToString("f0");

            //
            txtExposure_LeftCamPutSocket.Text = ImagePara.Instance.Exposure_LeftCamPutSocket.ToString("f0");
            txtExposure_CheckSocket.Text = ImagePara.Instance.Exposure_LeftCamCheckSocket.ToString("f0");
            txtExposure_RightCamGetSocket.Text = ImagePara.Instance.Exposure_RightCamGetSocket.ToString("f0");
            txtExposure_RightCamCheckSocket.Text = ImagePara.Instance.Exposure_RightCamCheckSocket.ToString("f0");
            txtExposure_LeftCamCheckTray.Text= ImagePara.Instance.Exposure_LeftCamCheckTray.ToString("f0");
        }
        private void button_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }
        private void txtExposure_LeftCamShotTray_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_LeftCamGetDUT = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_RightCamGetSocket_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_RightCamGetSocket = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_DownCamScan_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_DownCamScan = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_RightCamCheckSocket_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_RightCamCheckSocket = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_LeftCamPutSocket_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_LeftCamPutSocket = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_RightCamPutTray_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_RightCamPutDUT = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_CheckSocket_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_LeftCamCheckSocket = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_RightCamCheckTray_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_RightCamCheckTray = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_LeftCamPutTray_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_LeftCamPutDUT = Convert.ToInt32((sender as TextBox).Text);
        }

        private void txtExposure_LeftCamCheckTray_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber((sender as TextBox).Text))
                return;
            ImagePara.Instance.Exposure_LeftCamCheckTray = Convert.ToInt32((sender as TextBox).Text);
        }
    }
}
