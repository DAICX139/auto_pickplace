using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragonFlex.GUI.Factory
{
    public partial class UCIndex : UserControl
    {
        public UCIndex()
        {
            InitializeComponent();
        }

        string _name = "Name";
        [Browsable(true), Description("Name"), Category("自定义配置")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                lblName.Text = value;
            }
        }
    }
}
