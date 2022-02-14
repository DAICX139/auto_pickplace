
namespace Poc2Auto.GUI
{
    partial class FMDutStatList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxLotID = new System.Windows.Forms.ComboBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ColDutSN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColLIV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColNFBP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColKYRL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColBMPF = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColBin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOutPut = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lot ID:";
            // 
            // comboBoxLotID
            // 
            this.comboBoxLotID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLotID.FormattingEnabled = true;
            this.comboBoxLotID.Location = new System.Drawing.Point(63, 10);
            this.comboBoxLotID.Name = "comboBoxLotID";
            this.comboBoxLotID.Size = new System.Drawing.Size(168, 28);
            this.comboBoxLotID.TabIndex = 1;
            this.comboBoxLotID.SelectedIndexChanged += new System.EventHandler(this.comboBoxLotID_SelectedIndexChanged);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColDutSN,
            this.ColLIV,
            this.ColNFBP,
            this.ColKYRL,
            this.ColBMPF,
            this.ColBin});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 44);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(653, 304);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ColDutSN
            // 
            this.ColDutSN.Text = "SN";
            this.ColDutSN.Width = 150;
            // 
            // ColLIV
            // 
            this.ColLIV.Text = "LIV";
            this.ColLIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColLIV.Width = 100;
            // 
            // ColNFBP
            // 
            this.ColNFBP.Text = "NFBP";
            this.ColNFBP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColNFBP.Width = 100;
            // 
            // ColKYRL
            // 
            this.ColKYRL.Text = "KYRL";
            this.ColKYRL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColKYRL.Width = 100;
            // 
            // ColBMPF
            // 
            this.ColBMPF.Text = "BMPF";
            this.ColBMPF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColBMPF.Width = 100;
            // 
            // ColBin
            // 
            this.ColBin.Text = "Bin";
            this.ColBin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColBin.Width = 100;
            // 
            // btnOutPut
            // 
            this.btnOutPut.Location = new System.Drawing.Point(237, 9);
            this.btnOutPut.Name = "btnOutPut";
            this.btnOutPut.Size = new System.Drawing.Size(75, 30);
            this.btnOutPut.TabIndex = 3;
            this.btnOutPut.Text = "导出";
            this.btnOutPut.UseVisualStyleBackColor = true;
            this.btnOutPut.Click += new System.EventHandler(this.btnOutPut_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "CSV文件(*.csv)|*.csv";
            // 
            // FMDutStatList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(652, 353);
            this.Controls.Add(this.btnOutPut);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.comboBoxLotID);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(670, 400);
            this.MinimumSize = new System.Drawing.Size(670, 400);
            this.Name = "FMDutStatList";
            this.Text = "DUT数据查看";
            this.Shown += new System.EventHandler(this.FMDutStatList_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxLotID;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ColLIV;
        private System.Windows.Forms.ColumnHeader ColNFBP;
        private System.Windows.Forms.ColumnHeader ColKYRL;
        private System.Windows.Forms.ColumnHeader ColBMPF;
        private System.Windows.Forms.ColumnHeader ColBin;
        private System.Windows.Forms.ColumnHeader ColDutSN;
        private System.Windows.Forms.Button btnOutPut;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}