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
    public partial class FrmPlc : Form
    {
        private int axisID = -1;//axis id
        private double p2pPos = 0;
        private double p2pVel = PlcPara.Instance.P2PVel;
        private double jogVel = PlcPara.Instance.JogVel;

        public FrmPlc()
        {
            InitializeComponent();
        }

        private void FrmPlc_Load(object sender, EventArgs e)
        {
            IniTextBox();
            IniTreeView(tvwAxisList);
        }

        private void FrmPlc_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && Plc.PlcDriver != null && Plc.PlcDriver.IsInitOk)
            {
                this.tabControl1.Enabled = true;
                timer1.Enabled = false;
            }
            //else
            //{
            //    this.tabControl1.Enabled = false;
            //    timer1.Enabled = true;
            //}
        }

        private void tvwAxisList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var temp = e.Node.Tag;
            if (temp != null)
            {
                axisID = (int)temp;
                lblAxisID.Text = "AxisID: " + e.Node.Text;
            }
        }

        private void rdoAbs_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAbs.Checked)
            {
                btnGo.Text = "AbsGo";
                lblP2PPos.Text = "Pos";
            }
            else
            {
                btnGo.Text = "RelGo";
                lblP2PPos.Text = "Dis";
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (rdoAbs.Checked)
                Plc.AxisAbsGo(axisID, p2pPos, p2pVel);
            else
                Plc.AxisRelGo(axisID, p2pPos, p2pVel);
        }

        private void btnJogNeg_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo(axisID, -jogVel, true);
        }

        private void btnJogNeg_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo(axisID, -jogVel, false);
        }

        private void btnJogPos_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo(axisID, jogVel, true);
        }

        private void btnJogPos_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo(axisID, jogVel, false);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Plc.AxisGoHome(axisID);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Plc.AxisStop(axisID);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Plc.AxisReset(axisID);
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            Plc.AxisEnable(axisID, true);
        }

        private void txtP2PPos_Leave(object sender, EventArgs e)
        {
            var temp = (TextBox)sender;
            if (Regex.IsMatch(temp.Text, @"^[+-]?\d*[.]?\d*$"))
                p2pPos = Convert.ToDouble(temp.Text);
            else
            {
                temp.Text = "0";
                p2pPos = 0;
            }
        }

        private void txtP2PVel_Leave(object sender, EventArgs e)
        {
            var temp = (TextBox)sender;
            if (Regex.IsMatch(temp.Text, @"^[+-]?\d*[.]?\d*$"))
                p2pVel = Convert.ToDouble(temp.Text);
            else
            {
                temp.Text = "0";
                p2pVel = 0;
            }
        }

        private void txtJogVel_Leave(object sender, EventArgs e)
        {
            var temp = (TextBox)sender;
            if (Regex.IsMatch(temp.Text, @"^[+-]?\d*[.]?\d*$"))
                jogVel = Convert.ToDouble(temp.Text);
            else
            {
                temp.Text = "0";
                jogVel = 0;
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            pic.BackColor = Color.Yellow;
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            pic.BackColor = Color.DeepSkyBlue;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox pic = (PictureBox)sender;
                var index = Convert.ToInt32(pic.Name.Substring(3));

                int id = index / 3;
                int sts = index % 3;

                if (id < 8)
                {
                    if (sts == 0) Plc.CylinderToBase(id + 1);
                    else if (sts == 1) Plc.CylinderToWork(id + 1);
                    else Plc.CylinderToNone(id + 1);
                    return;
                }

                id = (index - 40) / 2;
                sts = (index - 40) % 2;
                if (id < 3)
                {
                    if (sts == 0) Plc.SetIO(id + 1, true);
                    else Plc.SetIO(id + 1, false);
                }
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                AxisStatus axisStatus = Plc.GetAxisStatus(axisID);
                lblActPos.Text = "ActPos(mm): " + axisStatus.ActPos.ToString("0.0000");
                lblErrorID.Text = "ErrorID: " + axisStatus.ErrorID.ToString();

                if (axisStatus.Enable) picEnable.BackColor = Color.Lime; else picEnable.BackColor = Color.Red;
                if (axisStatus.Homed) picHomed.BackColor = Color.Lime; else picHomed.BackColor = Color.Red;
                if (axisStatus.Busy) picBusy.BackColor = Color.Lime; else picBusy.BackColor = Color.WhiteSmoke;
                if (axisStatus.Done) picDone.BackColor = Color.Lime; else picDone.BackColor = Color.WhiteSmoke;
                if (axisStatus.Error) picError.BackColor = Color.Red; else picError.BackColor = Color.WhiteSmoke;

                for (int i = 0; i < 8; i++)
                {
                    if (Plc.GetCylinder(i + 1).Info.IsBase)
                    {
                        ((PictureBox)Controls.Find("pic" + (i * 3).ToString(), true)[0]).BackColor = Color.Lime;
                        ((PictureBox)Controls.Find("pic" + (i * 3 + 1).ToString(), true)[0]).BackColor = Color.Red;
                        ((PictureBox)Controls.Find("pic" + (i * 3 + 2).ToString(), true)[0]).BackColor = Color.Red;
                    }
                    else if (Plc.GetCylinder(i + 1).Info.IsWork)
                    {
                        ((PictureBox)Controls.Find("pic" + (i * 3).ToString(), true)[0]).BackColor = Color.Red;
                        ((PictureBox)Controls.Find("pic" + (i * 3 + 1).ToString(), true)[0]).BackColor = Color.Lime;
                        ((PictureBox)Controls.Find("pic" + (i * 3 + 2).ToString(), true)[0]).BackColor = Color.Red;
                    }
                    else
                    {
                        ((PictureBox)Controls.Find("pic" + (i * 3).ToString(), true)[0]).BackColor = Color.Red;
                        ((PictureBox)Controls.Find("pic" + (i * 3 + 1).ToString(), true)[0]).BackColor = Color.Red;
                        ((PictureBox)Controls.Find("pic" + (i * 3 + 2).ToString(), true)[0]).BackColor = Color.Lime;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (Plc.GetIO(i + 1))
                    {
                        ((PictureBox)Controls.Find("pic" + (40 + i * 2).ToString(), true)[0]).BackColor = Color.Lime;
                        ((PictureBox)Controls.Find("pic" + (40 + i * 2 + 1).ToString(), true)[0]).BackColor = Color.Red;
                    }
                    else
                    {
                        ((PictureBox)Controls.Find("pic" + (40 + i * 2).ToString(), true)[0]).BackColor = Color.Red;
                        ((PictureBox)Controls.Find("pic" + (40 + i * 2 + 1).ToString(), true)[0]).BackColor = Color.Lime;
                    }
                }
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void IniTextBox()
        {
            txtP2PPos.Text = p2pPos.ToString("0.0000");
            txtP2PVel.Text = p2pVel.ToString("0.0000");
            txtJogVel.Text = jogVel.ToString("0.0000");
        }

        private void IniTreeView(TreeView tvw)
        {
            try
            {
                tvw.Nodes.Clear();
                var parentNode = tvw.Nodes.Add("Axis List");
                parentNode.ImageIndex = 0;
                parentNode.SelectedImageIndex = 0;
                var names = Enum.GetNames(typeof(EnumAxis));
                for (int i = 0; i < names.GetLength(0); i++)
                {
                    parentNode.Nodes.Add(i.ToString(), names[i], 1, 1);
                    var selectNode = parentNode.Nodes[i];
                    selectNode.ImageIndex = 1;
                    selectNode.SelectedImageIndex = 1;
                    selectNode.Tag = (EnumAxis)Enum.Parse(typeof(EnumAxis), names[i]);
                    selectNode.ToolTipText = "";
                }
                tvw.ExpandAll();
                tvw.SelectedNode = parentNode.FirstNode;
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}