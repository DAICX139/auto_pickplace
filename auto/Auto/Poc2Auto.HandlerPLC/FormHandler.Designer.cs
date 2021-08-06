namespace Poc2Auto.HandlerPLC
{
    partial class FormHandler
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
            this.ucModeUI1 = new DragonFlex.GUI.Factory.UCModeUI();
            this.SuspendLayout();
            // 
            // ucModeUI1
            // 
            this.ucModeUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeUI1.Location = new System.Drawing.Point(0, 0);
            this.ucModeUI1.Margin = new System.Windows.Forms.Padding(2);
            this.ucModeUI1.Name = "ucModeUI1";
            this.ucModeUI1.Size = new System.Drawing.Size(800, 450);
            this.ucModeUI1.TabIndex = 0;
            // 
            // FormHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ucModeUI1);
            this.Name = "FormHandler";
            this.Text = "FormHandler";
            this.ResumeLayout(false);

        }

        #endregion

        private DragonFlex.GUI.Factory.UCModeUI ucModeUI1;
    }
}