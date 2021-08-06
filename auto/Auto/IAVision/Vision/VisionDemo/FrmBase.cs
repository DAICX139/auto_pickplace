using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisionDemo
{
    public partial class FrmBase : Form
    {
        protected Timer timer = new Timer();
        public FrmBase()
        {
            InitializeComponent();
            IniForm();
            timer.Tick += new EventHandler(timer_Tick);
        }

        public virtual void btnExecute_Click(object sender, EventArgs e)
        {
        }

        public virtual void btnOk_Click(object sender, EventArgs e)
        {
            Close(); ;
        }

        public virtual void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public virtual void timer_Tick(object sender, EventArgs e)
        {
        }

        public virtual void IniForm()
        {
            ShowIcon = false;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
