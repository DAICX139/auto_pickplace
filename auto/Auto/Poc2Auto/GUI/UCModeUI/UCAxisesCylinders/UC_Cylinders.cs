using System.Collections.Generic;
using System.Windows.Forms;
using AlcUtility;

namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    public partial class UC_Cylinders : UserControl
    {
        public UC_Cylinders()
        {
            InitializeComponent();
            //init();
        }

        IPlcDriver _plcDriver;
        public IPlcDriver PlcDriver
        {
            get
            {
                return _plcDriver;
            }
            set
            {
                if (value == null)
                    return;
                _plcDriver = value;
                init();
                value.OnInitOk += BindData;
                if (value.IsInitOk)
                {
                    BindData();
                }
            }   
        }
        public int CYL_COUNT { get; set; }
        public bool IsInnerUpdatingOpen 
        {
            set
            {
                if (_plcDriver == null || cylinders.Count == 0 || cylinders == null)
                    return;
                for (int i = 0; i < CYL_COUNT; i++)
                    cylinders[i].EnableUpdate = value;
            }
        }
        List<UC_Cylinder_New> cylinders = new List<UC_Cylinder_New>();
        public bool AuthorityCtrl 
        {
            set
            {
                if (_plcDriver == null || cylinders.Count == 0 || cylinders == null)
                    return;
                for (int i = 0; i < CYL_COUNT; i++)
                    cylinders[i].AuthorityCtrl = value;
            }
        }
        private void init()
        {
            for (int i = 0; i < CYL_COUNT; i++)
            {
                UC_Cylinder_New cylinder = new UC_Cylinder_New();
                //cylinder.Dock = DockStyle.Fill;
                cylinder.Margin = new Padding(0,0,0,0);
                cylinders.Add(cylinder);
                tableLayoutPanel1.Controls.Add(cylinder);
            }
        }

        private void BindData()
        {
            for (int i = 0; i < CYL_COUNT; i++)
            {
                cylinders[i].Cylinder = PlcDriver.GetCylinderCtrl(i+1);
                
            }    
        }

        public bool EnableUI { set
            {
                foreach (var ui in cylinders)
                    ui.EnableUI = value;
            }
        }

    }
}
