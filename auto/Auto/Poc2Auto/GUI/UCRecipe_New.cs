using AlcUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Poc2Auto.GUI
{
    public partial class UCRecipe_New : UserControl
    {
        public UCRecipe_New()
        {
            InitializeComponent();
            AxisNames = new Dictionary<string, int>();
            InitView();
        }

        [Description("标题"), Category("自定义")]
        public string Title { get => grpbxTitle.Text; set => grpbxTitle.Text = value; }

        private string _filePath { get; set; }
        /// <summary>
        /// 配方文件路径
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                _filePath = value;
            }
        }

        private ushort AxisCount { get; set; }
        private IPlcDriver _plcDriver { get; set; }
        public IPlcDriver PlcDriver
        {
            get => _plcDriver;
            set
            {
                if (null == value)
                    return;
                _plcDriver = value;
                if (value.IsInitOk)
                    InitData();
                else
                    value.OnInitOk += () => { InitData(); };
                
            }
        }
        /// <summary>
        /// 配方保存后触发
        /// </summary>
        public event Action<string> RecipeSave;
        /// <summary>
        /// 启动后默认配方文件
        /// </summary>
        public string DefaultRecipe { get; set; } = "";
        private ParamsConfig Config { get; set; }
        /// <summary>
        /// 权限控制属性
        /// </summary>
        public bool AuthorityCtrl
        {
            set
            {
                dataGridView1.Columns[dataGridView1.ColumnCount - 1].Visible = value;
                dataGridView1.Columns[dataGridView1.ColumnCount - 2].ReadOnly = !value;
                dataGridView1.Columns[dataGridView1.ColumnCount - 3].ReadOnly = !value;
                btnSave.Enabled = value;
                btnDelete.Enabled = value;
                cbxRecipeName.Enabled = value;
            }
        }
        /// <summary>
        /// 从PLC加载上来的配方
        /// </summary>
        private ParamsConfig _uploadData;
        /// <summary>
        ///是否覆盖当前配方
        /// </summary>
        private bool _isOverride = true;
        private bool isSave = true;
        private int paramModuleSelectRow;
        private Dictionary<string, int> AxisNames;

        /// <summary>
        /// 保存数据，Item1：序号，Item2：配方Key值，Item3：配方Value值，Item4：配方描述
        /// </summary>
        public List<Tuple<string, string, string, string>> KeyValues = new List<Tuple<string, string, string, string>>();

        private void InitView()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { InitView(); }));
                return;
            }
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[ColIndex.Index].Width = 20;
            dataGridView1.Columns[ColKey.Index].Width = 120;
            dataGridView1.Columns[ColValue.Index].Width = 60;
            dataGridView1.Columns[ColDescription.Index].Width = 150;
            dataGridView1.Columns[ColControl.Index].Width = 70;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[ColIndex.Index].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridParamModule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridParamModule.Columns[ColModuleName.Index].Width = 70;
            dataGridParamModule.Columns[ColModuleIndex.Index].Width = 50;
            dataGridParamModule.Columns[ColModuleIndex.Index].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// 添加数据到DataGridView
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public void AddData(string moduleName)
        {
            KeyValues.Clear();
            dataGridView1.RowCount = 0;
            InitView();
            if (InvokeRequired)
            {
                Invoke(new Action(() => { AddData(moduleName); }));
                return;
            }
            var module =  Config.GetParamsModule(moduleName);
            int index = 0;
            foreach (var pValue in module.KeyValues.Values)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Height = 30;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[ColIndex.Index].Value = ++index;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[ColKey.Index].Value = pValue.Key;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[ColValue.Index].Value = pValue.Value;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[ColDescription.Index].Value = pValue.Remark;
                KeyValues.Add(new Tuple<string, string, string, string>(index.ToString(), pValue.Key, pValue.Value.ToString(), pValue.Remark));
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearView()
        {
            KeyValues.Clear();
            if (InvokeRequired)
            {
                Invoke(new Action(() => { dataGridView1.RowCount = 0; dataGridParamModule.RowCount = 0; }));
                return;
            }
            dataGridView1.RowCount = 0;
            dataGridParamModule.RowCount = 0;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var count = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColIndex.Index].Value.ToString();
            var key = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColKey.Index].Value.ToString();
            var value = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColValue.Index].Value?.ToString();
            var description = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColDescription.Index].Value?.ToString();
            for (int i = 0; i < KeyValues.Count; i++)
            {
                if (KeyValues[i].Item2 == key)
                {
                    KeyValues.RemoveAt(i);
                    KeyValues.Insert(i, new Tuple<string, string, string, string>(count, key, value, description));
                    var result = InputValueCheck(key);
                    if (result == -1)   //输入的是无效值
                    {
                        AlcSystem.Instance.Error($"{dataGridParamModule.Rows[paramModuleSelectRow].Cells[ColModuleName.Index].Value} 模块中输入的 {key} 值无效，请重新输入", 0, AlcErrorLevel.WARN, PlcDriver.Name);
                        return;
                    }
                    else if (result == -2)  //输入的值超出范围
                    {
                   //   AlcSystem.Instance.Error($"{dataGridParamModule.Rows、//[aamModuleSelectRow].Cells[ColModuleName.Index].Value} 模块、输//的 {key} 值 {value} 超出范围，请重新输入", 2, 、/AcrrorLevel.WARN,/ PlcDriver.Name);
                   //   return;
                    }
                    //根据输入内容修改配方值
                    ModifyRecipe();
                    break;
                }
            }
        }

        #region Event
        private void cbxRecipeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisPlayRecipe(_filePath + cbxRecipeName.Text);
        }

        private void cbxRecipeName_TextChanged(object sender, EventArgs e)
        {
            if (!cbxRecipeName.Items.Contains(cbxRecipeName.Text))
            {
                btnSave.Text = "new";
                isSave = false;
            }
            else
            {
                btnSave.Text = "Save";
                isSave = true;
            }
        }

        /// <summary>
        /// 保存修改后的配方（含下发）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Config == null)
                return;
            if (isSave)
            {
                if (!File.Exists(_filePath + cbxRecipeName.Text))
                    return;
                //保存到项目专门的配方文件夹下
                Config.Save(_filePath + cbxRecipeName.Text);
                var path = _uploadData.FilePath;
                //保存到项目启动后加载的配方目录去
                Config.Save(path);
                AlcSystem.Instance.ShowMsgBox($"Recipe file saved successfully!", $"{ _plcDriver.Name}" + " Information");

                PlcDriver.CfgParamsConfig = Config;
                var ret = PlcDriver.DownloadToPlc();
                //写断电保持数据
                var ret2 = PlcDriver.GetSysInfoCtrl.WritePersistentData();
                if (ret == 0 && ret2 == 0)
                    AlcSystem.Instance.ShowMsgBox("Data download successfully!", "Information");
                else
                    AlcSystem.Instance.Error($"Data download failed!", ret, AlcErrorLevel.ERROR1, PlcDriver.Name);
                RecipeSave?.Invoke(cbxRecipeName.Text);
            }
            else //新建xml
            {
                if (Config == null)
                    Config = new ParamsConfig();
                if (!cbxRecipeName.Text.Contains(".xml"))
                {
                    AlcSystem.Instance.ShowMsgBox("Invalid file format!", PlcDriver.Name, icon: AlcMsgBoxIcon.Error);
                    return;
                }

                var displayMember = cbxRecipeName.Text;
                Config.Save(_filePath + displayMember);
                var path = _uploadData.FilePath;
                Config.Save(path);
                AlcSystem.Instance.ShowMsgBox($"Create recipe file successfully!", $"{ _plcDriver.Name}" + " Information");

                PlcDriver.CfgParamsConfig = Config;
                var ret = PlcDriver.DownloadToPlc();
                //写断电保持数据
                var ret2 = PlcDriver.GetSysInfoCtrl.WritePersistentData();
                if (ret == 0 && ret2 == 0)
                    AlcSystem.Instance.ShowMsgBox("Data download successfully!", "Information");
                else
                    AlcSystem.Instance.Error($"Data download failed!", ret, AlcErrorLevel.ERROR1, PlcDriver.Name);

                cbxRecipeName.DataSource = GetFileNameFromFolder(_filePath);
                cbxRecipeName.SelectedIndex = cbxRecipeName.FindString(displayMember);
                RecipeSave?.Invoke(_filePath + displayMember);
            }
        }

        /// <summary>
        /// 展示配方到ListView上
        /// </summary>
        /// <param name="recipeName"></param>
        private void DisPlayRecipe(string recipeName)
        {
            ClearView();
            Config = ParamsConfig.Upload(recipeName);
            if (Config == null)
                return;
            if (!_isOverride)   //如果没有点击覆盖按钮
            {
                //_isOverride = true;
                var filePath = _uploadData.FilePath;
                _uploadData = Config;
                _uploadData.FilePath = filePath;
                _uploadData.Save();
            }

            AddParamModule(Config);
            AddData(dataGridParamModule.Rows[0].Cells[ColModuleName.Index].Value.ToString());
        }

        /// <summary>
        /// 删除配方（源文件删除）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!File.Exists(_filePath + cbxRecipeName.Text))
            {
                AlcSystem.Instance.ShowMsgBox("Recipe file does not exist!", $"{ _plcDriver.Name}" + "Error", icon: AlcMsgBoxIcon.Error);
                return;
            }
            if (AlcSystem.Instance.ShowMsgBox($"Are you sure to delete {cbxRecipeName.Text} recipe?", $"{ _plcDriver.Name}" + " Question", AlcMsgBoxButtons.YesNo, icon: AlcMsgBoxIcon.Question) == AlcMsgBoxResult.Yes)
            {
                File.Delete(_filePath + cbxRecipeName.Text);
                cbxRecipeName.DataSource = GetFileNameFromFolder(_filePath);
            }
        }
        #endregion

        private void AddParamModule(ParamsConfig config)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { AddParamModule(config); }));
                return;
            }
            int index = 0;
            foreach (var moduleName in config.ParamsModules.Keys)
            {
                dataGridParamModule.Rows.Add();
                dataGridParamModule.Rows[dataGridParamModule.RowCount - 1].Height = 30;
                dataGridParamModule.Rows[dataGridParamModule.RowCount - 1].Cells[ColModuleIndex.Index].Value = ++index;
                dataGridParamModule.Rows[dataGridParamModule.RowCount - 1].Cells[ColModuleName.Index].Value = moduleName;
            }
        }

        private void InitData()
        {
            UpLoadRecipe(); //从PLC底层加载配方
            if (!_isOverride)
            {
                var ret = PlcDriver.DownloadToPlc();
                //写断电保持数据
                var ret2 = PlcDriver.GetSysInfoCtrl.WritePersistentData();
                if (ret == 0 && ret2 == 0)
                    AlcSystem.Instance.ShowMsgBox("Data download successfully!", "Information");
                else
                    AlcSystem.Instance.Error($"Data download failed!", ret, AlcErrorLevel.ERROR1, PlcDriver.Name);
            }
            

            //读取PLC轴信息(轴数量)
            AxisCount = (ushort)PlcDriver?.ReadObject("GVL_MachineDevice.MAX_AXIS_NUM", typeof(ushort));
            //获取每根轴对应的编号
            AxisNames = GetAxisNameDic();
            
            cbxRecipeName.BeginInvoke(new Action(() =>
            {
                cbxRecipeName.DataSource = GetFileNameFromFolder(_filePath);
                if (File.Exists(DefaultRecipe))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => { cbxRecipeName.SelectedIndex = cbxRecipeName.FindString(Path.GetFileName(DefaultRecipe)); }));
                        return;
                    }
                    cbxRecipeName.SelectedIndex = cbxRecipeName.FindString(Path.GetFileName(DefaultRecipe));
                }
            }));
        }

        /// <summary>
        /// 修改配方
        /// </summary>
        private void ModifyRecipe()
        {
            var module = Config.GetParamsModule(dataGridParamModule.Rows[paramModuleSelectRow].Cells[ColModuleName.Index].Value.ToString());
            foreach (var dic in KeyValues)
                if (module.KeyValues.ContainsKey(dic.Item2))
                    module.KeyValues[dic.Item2].Value = dic.Item3;
        }

        /// <summary>
        /// 获取指定路径下的所有文件名
        /// </summary>
        /// <param name="filePath">文件路径名</param>
        /// <returns>返回一个文件列表</returns>
        private List<string> GetFileNameFromFolder(string filePath)
        {
            var fName = new List<string>();
            var files = Directory.GetFiles(filePath);
            if (files.Length != 0 || files != null)
                foreach (var file in files)
                    fName.Add(Path.GetFileName(file));
            return fName;
        }

        /// <summary>
        /// 从PLC底层上载配方
        /// </summary>
        private void UpLoadRecipe()
        {
            if (_plcDriver == null)
                return;

            var ret = _plcDriver.UploadFromPlc(out ParamsConfig uploadData);
            var uploadPath = uploadData.FilePath;
            if (ret == -999)
            {
                AlcSystem.Instance.Error("Data upload failed!", ret, AlcErrorLevel.WARN, _plcDriver.Name);
                return;
            }
            else if (ret < 0)
            {
                var text = "The underlying data is inconsistent with the upper computer data. Do you want to cover the upper computer data?";
                if (AlcSystem.Instance.ShowMsgBox(text, $"{ _plcDriver.Name}", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2) == AlcMsgBoxResult.Yes)
                {
                    _isOverride = true;
                    Config = uploadData;
                    uploadData.Save();
                    Config.FilePath = _filePath + Path.GetFileName(DefaultRecipe);
                    Config.Save();
                }
                else
                {
                    _isOverride = false;
                }
            }
            _uploadData = uploadData;
            _uploadData.FilePath = uploadPath;
        }

        private void dataGridParamModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColModuleName.Index && e.RowIndex >= 0)
            {
                var row = e.RowIndex;
                paramModuleSelectRow = row;
                AddData(dataGridParamModule.Rows[row].Cells[ColModuleName.Index].Value.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColControl.Index && e.RowIndex >= 0)
            {
                var data = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColKey.Index].Value.ToString();
                foreach (var name in AxisNames)
                {
                    if ((data.Contains("Z") || data.Contains("z")) && name.Key == "S")
                    {
                        var pos = _plcDriver?.GetSingleAxisCtrl(name.Value)?.Info.ActPos.ToString("N4");
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColValue.Index].Value = pos;
                    }
                    else if ((data.Contains("Y") || data.Contains("y")) && name.Key == "L")
                    {
                        var pos = _plcDriver?.GetSingleAxisCtrl(name.Value)?.Info.ActPos.ToString("N4");
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColValue.Index].Value = pos;
                    }
                    else if (data.Contains(name.Key))
                    {
                        var pos = _plcDriver?.GetSingleAxisCtrl(name.Value)?.Info.ActPos.ToString("N4");
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColValue.Index].Value = pos;
                    }
                }
                
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColValue.Index && e.RowIndex >= 0)
            {
                var index = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColIndex.Index].Value?.ToString();
                var key = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColKey.Index].Value?.ToString();
                var value = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColValue.Index].Value?.ToString();
                var description = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[ColDescription.Index].Value?.ToString();
                for (int i = 0; i < KeyValues.Count; i++)
                {
                    if (KeyValues[i].Item2 == key)
                    {
                        KeyValues.RemoveAt(i);
                        KeyValues.Insert(i, new Tuple<string, string, string, string>(index, key, value, description));
                        ModifyRecipe();
                        break;
                    }
                } 
            }
                
        }

        private Dictionary<string, int> GetAxisNameDic()
        {
            var axisNames = new Dictionary<string, int>();
            for (int i = 1; i <= AxisCount; i++)
            {
                axisNames.Add(_plcDriver?.GetSingleAxisCtrl(i).Info.Name, i);
            }
            return axisNames;
        }

        //对某个配方值进行修改时，判断其输入值是否在范围内
        private int InputValueCheck(string key)
        {
            foreach (var dic in KeyValues)
                if (dic.Item2 == key)
                {
                    if (dic.Item2.Contains("X"))
                    {
                        if (!Double.TryParse(dic.Item3, out double value))
                        {
                            return -1;
                        }
                        if (value < -20 || value > 700)
                        {
                            return -2;
                        }
                    }
                    else if (dic.Item2.Contains("Y"))
                    {
                        if (!Double.TryParse(dic.Item3, out double value))
                        {
                            return -1;
                        }
                        if (value < -20 || value > 600)
                        {
                            return -2;
                        }
                    }
                    else if (dic.Item2.Contains("Z1"))
                    {

                    }
                    else if (dic.Item2.Contains("R1"))
                    {

                    }
                    else if (dic.Item2.Contains("Z2"))
                    {

                    }
                    else if (dic.Item2.Contains("R2"))
                    {

                    }
                }
            return 0;
        }
    }
}
