namespace Ajv.Controls
{
    public partial class ValueSetter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SettingLabel = new System.Windows.Forms.Label();
            this.SettingDimension = new System.Windows.Forms.Label();
            this.SettingText = new System.Windows.Forms.TextBox();
            this.SettingScrollbar = new System.Windows.Forms.HScrollBar();
            this.SuspendLayout();
            // 
            // SettingLabel
            // 
            this.SettingLabel.AutoSize = true;
            this.SettingLabel.Location = new System.Drawing.Point(3, 5);
            this.SettingLabel.Name = "SettingLabel";
            this.SettingLabel.Size = new System.Drawing.Size(47, 13);
            this.SettingLabel.TabIndex = 5;
            this.SettingLabel.Text = "Rotation";
            // 
            // SettingDimension
            // 
            this.SettingDimension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingDimension.AutoSize = true;
            this.SettingDimension.Location = new System.Drawing.Point(188, 5);
            this.SettingDimension.Name = "SettingDimension";
            this.SettingDimension.Size = new System.Drawing.Size(11, 13);
            this.SettingDimension.TabIndex = 6;
            this.SettingDimension.Text = "°";
            // 
            // SettingText
            // 
            this.SettingText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingText.Location = new System.Drawing.Point(145, 1);
            this.SettingText.Name = "SettingText";
            this.SettingText.Size = new System.Drawing.Size(39, 20);
            this.SettingText.TabIndex = 4;
            this.SettingText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SettingText_KeyPress);
            // 
            // SettingScrollbar
            // 
            this.SettingScrollbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingScrollbar.LargeChange = 100;
            this.SettingScrollbar.Location = new System.Drawing.Point(62, 3);
            this.SettingScrollbar.Maximum = 100000;
            this.SettingScrollbar.Name = "SettingScrollbar";
            this.SettingScrollbar.Size = new System.Drawing.Size(80, 17);
            this.SettingScrollbar.SmallChange = 20;
            this.SettingScrollbar.TabIndex = 3;
            this.SettingScrollbar.ValueChanged += new System.EventHandler(this.SettingScrollbar_ValueChanged);
            // 
            // ValueSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingLabel);
            this.Controls.Add(this.SettingDimension);
            this.Controls.Add(this.SettingText);
            this.Controls.Add(this.SettingScrollbar);
            this.Name = "ValueSetter";
            this.Size = new System.Drawing.Size(200, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SettingLabel;
        private System.Windows.Forms.Label SettingDimension;
        private System.Windows.Forms.TextBox SettingText;
        private System.Windows.Forms.HScrollBar SettingScrollbar;
    }
}
