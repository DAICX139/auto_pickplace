using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionFlows
{
    public partial class Form_offset : Form
    {
        public Form_offset()
        {
            InitializeComponent();
        }

        private void Form_offset_Load(object sender, EventArgs e)
        {
            //吸嘴1
            this.Nozzle1_Slot_CompensateX.Text = SystemPrara_New.Instance.Nozzle1_Slot_CompensateX.ToString("f3");
            this.Nozzle1_Slot_CompensateY.Text = SystemPrara_New.Instance.Nozzle1_Slot_CompensateY.ToString("f3");
            this.Nozzle1_Slot_CompensateR.Text = SystemPrara_New.Instance.Nozzle1_Slot_CompensateR.ToString("f3");

            this.Nozzle1_TrayDUT_CompensateX.Text = SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateX.ToString("f3");
            this.Nozzle1_TrayDUT_CompensateY.Text = SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateY.ToString("f3");
            this.Nozzle1_TrayDUT_CompensateR.Text = SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateR.ToString("f3");

            this.Nozzle1_Socket_Get_DUT_CompensateX.Text = SystemPrara_New.Instance.Nozzle1_Socket_Get_DUT_CompensateX.ToString("f3");
            this.Nozzle1_Socket_Get_DUT_CompensateY.Text = SystemPrara_New.Instance.Nozzle1_Socket_Get_DUT_CompensateY.ToString("f3");
            this.Nozzle1_Socket_Get_DUT_CompensateR.Text = SystemPrara_New.Instance.Nozzle1_Socket_Get_DUT_CompensateR.ToString("f3");

            this.Nozzle1_Socket_Put_DUT_CompensateX.Text = SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateX.ToString("f3");
            this.Nozzle1_Socket_Put_DUT_CompensateY.Text = SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateY.ToString("f3");
            this.Nozzle1_Socket_Put_DUT_CompensateR.Text = SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateR.ToString("f3");
            //吸嘴2

            this.Nozzle2_Slot_CompensateX.Text = SystemPrara_New.Instance.Nozzle2_Slot_CompensateX.ToString("f3");
            this.Nozzle2_Slot_CompensateY.Text = SystemPrara_New.Instance.Nozzle2_Slot_CompensateY.ToString("f3");
            this.Nozzle2_Slot_CompensateR.Text = SystemPrara_New.Instance.Nozzle2_Slot_CompensateR.ToString("f3");

            this.Nozzle2_TrayDUT_CompensateX.Text = SystemPrara_New.Instance.Nozzle2_TrayDUT_CompensateX.ToString("f3");
            this.Nozzle2_TrayDUT_CompensateY.Text = SystemPrara_New.Instance.Nozzle2_TrayDUT_CompensateY.ToString("f3");
            this.Nozzle2_TrayDUT_CompensateR.Text = SystemPrara_New.Instance.Nozzle2_TrayDUT_CompensateR.ToString("f3");

            this.Nozzle2_Socket_Get_DUT_CompensateX.Text = SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateX.ToString("f3");
            this.Nozzle2_Socket_Get_DUT_CompensateY.Text = SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateY.ToString("f3");
            this.Nozzle2_Socket_Get_DUT_CompensateR.Text = SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateR.ToString("f3");

            this.Nozzle2_Socket_Put_DUT_CompensateX.Text = SystemPrara_New.Instance.Nozzle2_Socket_Put_DUT_CompensateX.ToString("f3");
            this.Nozzle2_Socket_Put_DUT_CompensateY.Text = SystemPrara_New.Instance.Nozzle2_Socket_Put_DUT_CompensateY.ToString("f3");
            this.Nozzle2_Socket_Put_DUT_CompensateR.Text = SystemPrara_New.Instance.Nozzle2_Socket_Put_DUT_CompensateR.ToString("f3");
        }

        private void Nozzle1_Slot_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Slot_CompensateX = Convert.ToDouble(Nozzle1_Slot_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Slot_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Slot_CompensateY = Convert.ToDouble(Nozzle1_Slot_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Slot_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Slot_CompensateR = Convert.ToDouble(Nozzle1_Slot_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_TrayDUT_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateX = Convert.ToDouble(Nozzle1_TrayDUT_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_TrayDUT_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateY = Convert.ToDouble(Nozzle1_TrayDUT_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_TrayDUT_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateR = Convert.ToDouble(Nozzle1_TrayDUT_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Socket_Get_DUT_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Socket_Get_DUT_CompensateX = Convert.ToDouble(Nozzle1_Socket_Get_DUT_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Socket_Get_DUT_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Socket_Get_DUT_CompensateY = Convert.ToDouble(Nozzle1_Socket_Get_DUT_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Socket_Get_DUT_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Socket_Get_DUT_CompensateR = Convert.ToDouble(Nozzle1_Socket_Get_DUT_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Socket_Put_DUT_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateX = Convert.ToDouble(Nozzle1_Socket_Put_DUT_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Socket_Put_DUT_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateY = Convert.ToDouble(Nozzle1_Socket_Put_DUT_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle1_Socket_Put_DUT_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateR = Convert.ToDouble(Nozzle1_Socket_Put_DUT_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Slot_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Slot_CompensateX = Convert.ToDouble(Nozzle2_Slot_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Slot_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Slot_CompensateY = Convert.ToDouble(Nozzle2_Slot_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Slot_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Slot_CompensateR = Convert.ToDouble(Nozzle2_Slot_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_TrayDUT_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_TrayDUT_CompensateX = Convert.ToDouble(Nozzle2_TrayDUT_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_TrayDUT_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_TrayDUT_CompensateY = Convert.ToDouble(Nozzle2_TrayDUT_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_TrayDUT_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_TrayDUT_CompensateR = Convert.ToDouble(Nozzle2_TrayDUT_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Socket_Get_DUT_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateX = Convert.ToDouble(Nozzle2_Socket_Get_DUT_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Socket_Get_DUT_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateY = Convert.ToDouble(Nozzle2_Socket_Get_DUT_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Socket_Get_DUT_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateR = Convert.ToDouble(Nozzle2_Socket_Get_DUT_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Socket_Put_DUT_CompensateX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Socket_Put_DUT_CompensateX = Convert.ToDouble(Nozzle2_Socket_Put_DUT_CompensateX.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Socket_Put_DUT_CompensateY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Socket_Put_DUT_CompensateY = Convert.ToDouble(Nozzle2_Socket_Put_DUT_CompensateY.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Nozzle2_Socket_Put_DUT_CompensateR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SystemPrara_New.Instance.Nozzle2_Socket_Put_DUT_CompensateR = Convert.ToDouble(Nozzle2_Socket_Put_DUT_CompensateR.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Form_offset_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlHelper.Instance().SerializeToXml(Utility.Config + "Compensate.xml", SystemPrara_New.Instance);
        }
    }
}