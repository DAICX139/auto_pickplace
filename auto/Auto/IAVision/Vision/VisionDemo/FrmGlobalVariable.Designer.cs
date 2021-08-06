namespace VisionDemo
{
    partial class FrmGlobalVariable
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddBool = new System.Windows.Forms.Button();
            this.btnAddString = new System.Windows.Forms.Button();
            this.btnAddDouble = new System.Windows.Forms.Button();
            this.btnAddInt = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvGlobalVariable = new System.Windows.Forms.DataGridView();
            this.pnlFootBase.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlobalVariable)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFootBase
            // 
            this.pnlFootBase.Location = new System.Drawing.Point(0, 401);
            this.pnlFootBase.Size = new System.Drawing.Size(800, 48);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(472, 13);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(562, 13);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(653, 13);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAddBool);
            this.panel1.Controls.Add(this.btnAddString);
            this.panel1.Controls.Add(this.btnAddDouble);
            this.panel1.Controls.Add(this.btnAddInt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(600, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 401);
            this.panel1.TabIndex = 1;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(64, 325);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 93;
            this.btnDown.Text = "下移";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(64, 285);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 92;
            this.btnUp.Text = "上移";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(64, 211);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 91;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnAddBool
            // 
            this.btnAddBool.Location = new System.Drawing.Point(64, 139);
            this.btnAddBool.Name = "btnAddBool";
            this.btnAddBool.Size = new System.Drawing.Size(75, 23);
            this.btnAddBool.TabIndex = 90;
            this.btnAddBool.Text = "添加Bool";
            this.btnAddBool.UseVisualStyleBackColor = true;
            this.btnAddBool.Click += new System.EventHandler(this.btnAddBool_Click);
            // 
            // btnAddString
            // 
            this.btnAddString.Location = new System.Drawing.Point(64, 100);
            this.btnAddString.Name = "btnAddString";
            this.btnAddString.Size = new System.Drawing.Size(75, 23);
            this.btnAddString.TabIndex = 89;
            this.btnAddString.Text = "添加String";
            this.btnAddString.UseVisualStyleBackColor = true;
            this.btnAddString.Click += new System.EventHandler(this.btnAddString_Click);
            // 
            // btnAddDouble
            // 
            this.btnAddDouble.Location = new System.Drawing.Point(64, 60);
            this.btnAddDouble.Name = "btnAddDouble";
            this.btnAddDouble.Size = new System.Drawing.Size(75, 23);
            this.btnAddDouble.TabIndex = 88;
            this.btnAddDouble.Text = "添加Double";
            this.btnAddDouble.UseVisualStyleBackColor = true;
            this.btnAddDouble.Click += new System.EventHandler(this.btnAddDouble_Click);
            // 
            // btnAddInt
            // 
            this.btnAddInt.Location = new System.Drawing.Point(64, 21);
            this.btnAddInt.Name = "btnAddInt";
            this.btnAddInt.Size = new System.Drawing.Size(75, 23);
            this.btnAddInt.TabIndex = 87;
            this.btnAddInt.Text = "添加Int";
            this.btnAddInt.UseVisualStyleBackColor = true;
            this.btnAddInt.Click += new System.EventHandler(this.btnAddInt_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvGlobalVariable);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(600, 401);
            this.panel2.TabIndex = 2;
            // 
            // dgvGlobalVariable
            // 
            this.dgvGlobalVariable.AllowUserToAddRows = false;
            this.dgvGlobalVariable.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvGlobalVariable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGlobalVariable.ColumnHeadersHeight = 40;
            this.dgvGlobalVariable.Location = new System.Drawing.Point(78, 82);
            this.dgvGlobalVariable.Name = "dgvGlobalVariable";
            this.dgvGlobalVariable.RowHeadersVisible = false;
            this.dgvGlobalVariable.RowTemplate.Height = 23;
            this.dgvGlobalVariable.Size = new System.Drawing.Size(367, 266);
            this.dgvGlobalVariable.TabIndex = 0;
            // 
            // FrmGlobalVariable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmGlobalVariable";
            this.Text = "全局变量";
            this.Load += new System.EventHandler(this.FrmGlobalVariable_Load);
            this.Controls.SetChildIndex(this.pnlFootBase, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.pnlFootBase.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlobalVariable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvGlobalVariable;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAddBool;
        private System.Windows.Forms.Button btnAddString;
        private System.Windows.Forms.Button btnAddDouble;
        private System.Windows.Forms.Button btnAddInt;
    }
}