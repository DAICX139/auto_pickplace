using System;
using System.ComponentModel;
using System.Windows.Forms;
using VisionModules;
using VisionControls;
using VisionUtility;
using System.Diagnostics;

namespace VisionDemo
{
    public partial class FrmItemAcqImage : FrmItemBase
    {
        protected ImageWindow imageWindow = new ImageWindow();
        protected ItemAcqImage curItem = null;

        private FrmItemAcqImage()
        {
            InitializeComponent();
        }

        public FrmItemAcqImage(Item item) : base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemAcqImage;
            IniCmbDevice();
            IniImageWindow(pnlImage);
        }

        private void FrmItemAcqImage_Load(object sender, EventArgs e)
        {
            curItem.Image.ReadImage(VisionPath.image);
            imageWindow.Image = curItem.Image;
            imageWindow.FitSize();
            IniAcqMode();
        }

        private void FrmItemAcqImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            imageWindow.Dispose();
        }

        private void rdbFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFile.Checked)
            {
                tbpCamera.Parent = null;
                tbpFile.Parent = tbcDevice;
                tbpFile.Text = "指定图像";
                curItem.AcqMode = AcqMode.File;
            }
            else if (rdbFolder.Checked)
            {
                tbpCamera.Parent = null;
                tbpFile.Parent = tbcDevice;
                tbpFile.Text = "文件目录";
                curItem.AcqMode = AcqMode.Folder;
            }
            else//相机
            {
                tbpCamera.Parent = tbcDevice;
                tbpFile.Parent = null;
                curItem.AcqMode = AcqMode.Camera;
            }
        }
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.DefaultExt = ".png";
                ofd.Filter = "PNG文件（*.png)|*.png|所有文件（*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Title = "打开图片";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    curItem.ImageName = ofd.FileName;
                    txtPath.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void cmbCameraUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = cmbCameraUserID.SelectedIndex;
                if (index < 0)
                    return;

                VisionCameraBase camera = VisionModulesManager.CameraList[index];
                txtExposureTime.Text = camera.ExposureTime.ToString();
                txtGain.Text = camera.Gain.ToString();
                chkReverseX.Checked = camera.ReverseX;
                chkReverseY.Checked = camera.ReverseY;
            }
            catch (Exception ex)
            {
            }
        }
        public override void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                imageWindow.HWindow.ClearWindow();

                switch (curItem.AcqMode)
                {
                    case AcqMode.Camera:
                        curItem.CameraUserID = cmbCameraUserID.Text;
                        int index = VisionModulesManager.CameraList.FindIndex(c => c.CameraUserID == curItem.CameraUserID);
                        if (index < 0)
                            return;

                        VisionCameraBase camera = VisionModulesManager.CameraList[index];
                        camera.SetExposureTime(Convert.ToDouble(txtExposureTime.Text));
                        camera.SetGain(Convert.ToDouble(txtGain.Text));
                        camera.SetReverseX(chkReverseX.Checked);
                        camera.SetReverseY(chkReverseY.Checked);
                        camera.CaptureImage();
                        camera.CaptureSignal.WaitOne(1000);
                        curItem.Image = camera.Image;
                        imageWindow.Image = camera.Image;
                        imageWindow.FitSize();
                        break;
                    case AcqMode.File:
                        curItem.Image.ReadImage(curItem.ImageName);
                        imageWindow.Image = curItem.Image;
                        imageWindow.FitSize();
                        break;
                    case AcqMode.Folder:
                        curItem.Image.ReadImage(curItem.ImageName);
                        imageWindow.Image = curItem.Image;
                        imageWindow.FitSize();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public override void btnOk_Click(object sender, EventArgs e)
        {
            base.btnOk_Click(sender, e);
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            base.btnCancel_Click(sender, e);
        }

        private void IniCmbDevice()
        {
            cmbCameraUserID.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);
            cmbCameraUserID.DisplayMember = "CameraUserID";
        }

        public override void IniImageWindow(Panel pnlImage)
        {
            imageWindow.Dock = DockStyle.Fill;
            imageWindow.Repaint += new Action(OnRepaint);
            pnlImage.Controls.Add(imageWindow);
            imageWindow.Show();
        }

        private void IniAcqMode()
        {
            if (curItem.AcqMode == AcqMode.File)
            {
                rdbFile.Checked = true;
                rdbFolder.Checked = false;
                rdbCamera.Checked = false;
                txtPath.Text = curItem.ImageName;
            }

            else if (curItem.AcqMode == AcqMode.Folder)
            {
                rdbFolder.Checked = true;
                rdbFile.Checked = false;
                rdbCamera.Checked = false;
                txtPath.Text = curItem.ImageName;
            }
            else
            {
                rdbCamera.Checked = true;
                rdbFile.Checked = false;
                rdbFolder.Checked = false;
                cmbCameraUserID.Text = curItem.CameraUserID;
            }
        }

        private void cmbTriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
