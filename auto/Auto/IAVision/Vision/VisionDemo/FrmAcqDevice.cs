using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using VisionControls;
using VisionUtility;
using VisionModules;
using VisionCamera.Daheng;
using VisionCamera.Basler;

namespace VisionDemo
{
    public partial class FrmAcqDevice : FrmBase
    {
        private HImage image = new HImage();
        private ImageWindow imageWindow = new ImageWindow();

        private ContextMenuStrip cmsDgvDeviceList = new ContextMenuStrip();//
        private List<VisionCameraInfo> cameraInfoList = new List<VisionCameraInfo>();

        public FrmAcqDevice()
        {
            InitializeComponent();
            //
            InitDgv(dgvDeviceList);
            DgvDeviceListBindingProperty();
            dgvDeviceList.CellClick += OnCellClick;
            InitDgv(dgvSdk);
            DgvSdkBindingProperty();
            InitCmbDeviceType();
            IniCmbTriggerMode();
            IniCmsDgvDeviceList();
            IniImageWindow(pnlImage);
        }


        private void FrmAcqDevice_Load(object sender, EventArgs e)
        {
            foreach (VisionCameraBase item in VisionModulesManager.CameraList)
                item.DisplayImage += OnDisplayImage;

            //dgvDeviceList.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);
            dgvDeviceList.DataSource = VisionModulesManager.CameraList;
            dgvSdk.DataSource = CameraPluginManager.Instance().CameraPlugins;

            if (dgvDeviceList.Rows.Count >0)
            {
                dgvDeviceList.Rows[0].Selected = true;
                int Index = dgvDeviceList.CurrentCell.RowIndex;
                VisionCameraBase camera = VisionModulesManager.CameraList[Index];
                UpdateUI(camera);
            }

            image.ReadImage(VisionPath.image);
            imageWindow.Image = image;
            imageWindow.FitSize();
        }

        private void FrmAcqDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (VisionCameraBase item in VisionModulesManager.CameraList)
                item.DisplayImage -= OnDisplayImage;
        }

        private void cmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDeviceType.Items.Count == 0)
                return;

            try
            {
                switch ((CameraType)Enum.Parse(typeof(CameraType), cmbDeviceType.Text))
                {
                    case CameraType.File:
                        cameraInfoList = null;
                        break;
                    case CameraType.Folder:
                        cameraInfoList = null;
                        break;
                    case CameraType.Basler:
                        VisionCamera.Basler.Basler.EnumerateCameras(out cameraInfoList);
                        break;
                    case CameraType.Daheng:
                        Daheng.EnumerateCameras(out cameraInfoList);
                        break;
                    case CameraType.Hikvision:
                        cameraInfoList = null;
                        return;
                    default:
                        cameraInfoList = null;
                        break;
                }

                cmbDeviceName.DataSource = cameraInfoList;
                cmbDeviceName.DisplayMember = "CameraUserID";
            }
            catch(Exception ex)
            {
                cameraInfoList = null;
                cmbDeviceName.DataSource = cameraInfoList;
            }
        }

        private void btnAddToDeviceList_Click(object sender, EventArgs e)
        {
            if (cmbDeviceName.SelectedIndex < 0)
                return;

            VisionCameraBase camera;

            int index = VisionModulesManager.CameraList.FindIndex(c => c.CameraSerialNumber == cameraInfoList[cmbDeviceName.SelectedIndex].CameraSerialNumber);
            if (index >= 0)
            {
                MessageBox.Show("该设备已经添加列表");
                return;
            }
            switch ((CameraType)Enum.Parse(typeof(CameraType), cmbDeviceType.Text))
            {
                case CameraType.File:
                    break;
                case CameraType.Folder:
                    break;
                case CameraType.Basler:
                    camera = new VisionCamera.Basler.Basler(CameraType.Basler);
                    camera.CameraUserID = cameraInfoList[cmbDeviceName.SelectedIndex].CameraUserID;
                    camera.CameraSerialNumber = cameraInfoList[cmbDeviceName.SelectedIndex].CameraSerialNumber;
                    camera.CameraRemark = cameraInfoList[cmbDeviceName.SelectedIndex].CameraRemark;
                    camera.DisplayImage += OnDisplayImage;
                    VisionModulesManager.CameraList.Add(camera);
                    break;
                case CameraType.Daheng:
                    camera = new Daheng(CameraType.Daheng);
                    camera.CameraUserID = cameraInfoList[cmbDeviceName.SelectedIndex].CameraUserID;
                    camera.CameraSerialNumber = cameraInfoList[cmbDeviceName.SelectedIndex].CameraSerialNumber;         
                    camera.CameraRemark = cameraInfoList[cmbDeviceName.SelectedIndex].CameraRemark;
                    camera.CameraMACAdress = cameraInfoList[cmbDeviceName.SelectedIndex].CameraMACAddress;
                    camera.DisplayImage += OnDisplayImage; ;
                    VisionModulesManager.CameraList.Add(camera);
                    break;

                case CameraType.Hikvision:
                    return;

                default:
                    break;

            }

            dgvDeviceList.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);
        }

        private void cmbTriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvDeviceList.CurrentCell != null)
            {
                var camera = VisionModulesManager.CameraList[dgvDeviceList.CurrentCell.RowIndex];
                camera.SetTriggerMode((TriggerMode)cmbTriggerMode.SelectedIndex);
                UpdateUI(camera);
            }
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (dgvDeviceList.CurrentCell!=null)
            {
                int index = dgvDeviceList.CurrentCell.RowIndex;
                VisionCameraBase camera = VisionModulesManager.CameraList[index];
                camera.Connect();
                dgvDeviceList.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);

                dgvDeviceList.Rows[index].Selected = true;
                UpdateUI(camera);             
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (dgvDeviceList.CurrentCell != null)
            {
                int index = dgvDeviceList.CurrentCell.RowIndex;
                VisionCameraBase camera = VisionModulesManager.CameraList[index];
                camera.DisConnect();
                dgvDeviceList.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);

                dgvDeviceList.Rows[index].Selected = true;
                UpdateUI(camera);
            }
        }

        private void btnAcqImage_Click(object sender, EventArgs e)
        {
            if (dgvDeviceList.CurrentCell != null)
            {
                int index = dgvDeviceList.CurrentCell.RowIndex;
                VisionCameraBase camera = VisionModulesManager.CameraList[index];

                camera.SetExposureTime(Convert.ToDouble(txtExposureTime.Text));
                camera.SetGain(Convert.ToDouble(txtGain.Text));
                camera.CaptureImage();

                UpdateUI(camera);
            }
        }

        public override void btnExecute_Click(object sender, EventArgs e)
        {
            base.btnExecute_Click(sender, e);
        }

        public override void btnOk_Click(object sender, EventArgs e)
        {
            base.btnOk_Click(sender, e);
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            base.btnCancel_Click(sender, e);
        }


        #region"Helper"

        private void InitDgv(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ScrollBars = ScrollBars.Vertical;
            dgv.Dock = DockStyle.Fill;
        }

        private void DgvDeviceListBindingProperty()
        {
            dgvDeviceList.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.HeaderText = "名称";
            col1.DataPropertyName = "CameraUserID";
            col1.Name = "CameraUserID";
            dgvDeviceList.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.HeaderText = "连接状态";
            col2.DataPropertyName = "IsConnected";
            col2.Name = "IsConnected";
            dgvDeviceList.Columns.Add(col2);

            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            col3.HeaderText = "备注";
            col3.DataPropertyName = "CameraRemark";
            col3.Name = "CameraRemark";
            dgvDeviceList.Columns.Add(col3);
        }

        private void DgvSdkBindingProperty()
        {
            dgvSdk.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.HeaderText = "设备型号";
            col1.DataPropertyName = "SdkName";
            col1.Name = "SdkName";
            dgvSdk.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.HeaderText = "SDK版本";
            col2.DataPropertyName = "SdkVersion";
            col2.Name = "SdkVersion";
            dgvSdk.Columns.Add(col2);

            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            col3.HeaderText = "Dll名称";
            col3.DataPropertyName = "DllName";
            col3.Name = "DllName";
            dgvSdk.Columns.Add(col3);
        }

        private void InitCmbDeviceType()
        {
            List<string> deviceTypeList = new List<string>();
            foreach (string s in Enum.GetNames(typeof(CameraType)))
            {
                deviceTypeList.Add(s);
            }
            cmbDeviceType.DataSource = deviceTypeList;
        }

        private void IniCmbTriggerMode()
        {
            List<string> triggerMode = new List<string>();
            foreach (string s in Enum.GetNames(typeof(TriggerMode)))
            {
                triggerMode.Add(s);
            }
            cmbTriggerMode.DataSource = triggerMode;
        }

        private void IniCmsDgvDeviceList()
        {
            ToolStripMenuItem delete = new ToolStripMenuItem("删除");
            delete.Click += new EventHandler((s, e) => OnDelete(s, e));
            cmsDgvDeviceList.Items.Add(delete);

            ToolStripMenuItem deleteAll = new ToolStripMenuItem("删除所有");
            deleteAll.Click += new EventHandler((s, e) => OnDeleteAll(s, e));
            cmsDgvDeviceList.Items.Add(deleteAll);

            dgvDeviceList.ContextMenuStrip = cmsDgvDeviceList;
        }

        protected virtual void IniImageWindow(Panel pnlImage)
        {
            imageWindow.Dock = DockStyle.Fill;
            imageWindow.Show();
            pnlImage.Controls.Add(imageWindow);
        }

        private void OnDelete(object sender, EventArgs e)
        {
            if (dgvDeviceList.CurrentCell!=null)
            {
                int index = dgvDeviceList.CurrentCell.RowIndex;
                VisionModulesManager.CameraList[index].DisConnect();
                VisionModulesManager.CameraList.RemoveAt(index);
                dgvDeviceList.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);

                if (index > 0)
                    dgvDeviceList.CurrentCell = dgvDeviceList.Rows[index - 1].Cells[0];
            }
        }

        private void OnDeleteAll(object sender, EventArgs e)
        {
            foreach(VisionCameraBase visionCamera in VisionModulesManager.CameraList)
                visionCamera.DisConnect();

            VisionModulesManager.CameraList.Clear();
            dgvDeviceList.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);
            VisionCameraBase.LastCameraID = 0;
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDeviceList.CurrentCell != null)
            {
               int index = dgvDeviceList.CurrentCell.RowIndex;
                VisionCameraBase camera = VisionModulesManager.CameraList[index];
                UpdateUI(camera);
            }

        }

        private void OnDisplayImage(HImage image)
        {
            imageWindow.Image = image;
        }

        private void UpdateUI(VisionCameraBase camera)
        {
            txtCurrDevice.Text = camera.CameraUserID;
            txtExposureTime.Text = camera.ExposureTime.ToString();
            txtGain.Text = camera.Gain.ToString();

            btnConnect.Enabled = !camera.IsConnected;
            btnDisConnect.Enabled = camera.IsConnected;
            btnAcqImage.Enabled = camera.IsConnected;

            txtCurrDevice.Enabled= camera.IsConnected;
            txtExposureTime.Enabled= camera.IsConnected;
            txtGain.Enabled= camera.IsConnected;
            cmbTriggerMode.Enabled= camera.IsConnected; ;
        }

        #endregion

        private void cmbDeviceName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dgvDeviceList.CurrentCell != null)
            {
                int index = dgvDeviceList.CurrentCell.RowIndex;
                VisionCameraBase camera = VisionModulesManager.CameraList[index];

                camera.Reset();

                UpdateUI(camera);
            }
        }
    }
}
