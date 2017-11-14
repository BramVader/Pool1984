namespace Pool1984
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.RightPanel = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ExportersCombo = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.SaveRenderingButton = new System.Windows.Forms.Button();
            this.RenderButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.timeSetter = new Pool1984.ValueSetter();
            this.ViewLightsCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewColorRefsCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewNumbersFlatCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewGridCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewNumbers3DCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewShadowOutlinesCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewBoxesCheckbox = new System.Windows.Forms.CheckBox();
            this.ViewRenderingCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewBallsCheckBox = new System.Windows.Forms.CheckBox();
            this.ViewBallOutlineFlatCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ViewEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.ViewRotation2Setter = new Pool1984.ValueSetter();
            this.ViewRotation1Setter = new Pool1984.ValueSetter();
            this.ViewDistanceSetter = new Pool1984.ValueSetter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CamRotationSetter = new Pool1984.ValueSetter();
            this.OffsetYSetter = new Pool1984.ValueSetter();
            this.OffsetXSetter = new Pool1984.ValueSetter();
            this.CamDistSetter = new Pool1984.ValueSetter();
            this.label1 = new System.Windows.Forms.Label();
            this.ApertureVLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ApertureHLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CubeMapContextActiveCubeMap = new System.Windows.Forms.ToolStripComboBox();
            this.cubemapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CubemapsRecalcCoarseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CubemapsRecalcFineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CubemapsCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spheremapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SpheremapsRecalcCoarseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SpheremapsCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.RenderBox = new Pool1984.ZoomablePictureBox();
            this.CubeMapBox = new Pool1984.ZoomablePictureBox();
            this.rerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveRenderingDialog = new System.Windows.Forms.SaveFileDialog();
            this.ExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.RightPanel.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RenderBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CubeMapBox)).BeginInit();
            this.SuspendLayout();
            // 
            // RightPanel
            // 
            this.RightPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RightPanel.Controls.Add(this.groupBox5);
            this.RightPanel.Controls.Add(this.groupBox4);
            this.RightPanel.Controls.Add(this.groupBox3);
            this.RightPanel.Controls.Add(this.groupBox2);
            this.RightPanel.Controls.Add(this.groupBox1);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(822, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(277, 823);
            this.RightPanel.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ExportButton);
            this.groupBox5.Controls.Add(this.ExportersCombo);
            this.groupBox5.Location = new System.Drawing.Point(5, 567);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(259, 60);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Export";
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(156, 19);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(75, 23);
            this.ExportButton.TabIndex = 403;
            this.ExportButton.Text = "Export...";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ExportersCombo
            // 
            this.ExportersCombo.FormattingEnabled = true;
            this.ExportersCombo.Location = new System.Drawing.Point(10, 19);
            this.ExportersCombo.Name = "ExportersCombo";
            this.ExportersCombo.Size = new System.Drawing.Size(138, 21);
            this.ExportersCombo.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.SaveRenderingButton);
            this.groupBox4.Controls.Add(this.RenderButton);
            this.groupBox4.Location = new System.Drawing.Point(5, 507);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(259, 54);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rendering";
            // 
            // SaveRenderingButton
            // 
            this.SaveRenderingButton.Location = new System.Drawing.Point(156, 19);
            this.SaveRenderingButton.Name = "SaveRenderingButton";
            this.SaveRenderingButton.Size = new System.Drawing.Size(75, 23);
            this.SaveRenderingButton.TabIndex = 402;
            this.SaveRenderingButton.Text = "Save...";
            this.SaveRenderingButton.UseVisualStyleBackColor = true;
            this.SaveRenderingButton.Click += new System.EventHandler(this.SaveRenderingButton_Click);
            // 
            // RenderButton
            // 
            this.RenderButton.Location = new System.Drawing.Point(10, 19);
            this.RenderButton.Name = "RenderButton";
            this.RenderButton.Size = new System.Drawing.Size(138, 23);
            this.RenderButton.TabIndex = 401;
            this.RenderButton.Text = "Render start/stop";
            this.RenderButton.UseVisualStyleBackColor = true;
            this.RenderButton.Click += new System.EventHandler(this.RenderButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.timeSetter);
            this.groupBox3.Controls.Add(this.ViewLightsCheckBox);
            this.groupBox3.Controls.Add(this.ViewColorRefsCheckBox);
            this.groupBox3.Controls.Add(this.ViewNumbersFlatCheckBox);
            this.groupBox3.Controls.Add(this.ViewGridCheckBox);
            this.groupBox3.Controls.Add(this.ViewNumbers3DCheckBox);
            this.groupBox3.Controls.Add(this.ViewShadowOutlinesCheckBox);
            this.groupBox3.Controls.Add(this.ViewBoxesCheckbox);
            this.groupBox3.Controls.Add(this.ViewRenderingCheckBox);
            this.groupBox3.Controls.Add(this.ViewBallsCheckBox);
            this.groupBox3.Controls.Add(this.ViewBallOutlineFlatCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(5, 332);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 169);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "View options";
            // 
            // timeSetter
            // 
            this.timeSetter.Dimension = "";
            this.timeSetter.Label = "Time";
            this.timeSetter.Location = new System.Drawing.Point(12, 134);
            this.timeSetter.Max = 1D;
            this.timeSetter.Min = 0D;
            this.timeSetter.Name = "timeSetter";
            this.timeSetter.Size = new System.Drawing.Size(225, 23);
            this.timeSetter.TabIndex = 340;
            this.timeSetter.Value = 0D;
            this.timeSetter.ValueChanged += new System.EventHandler(this.TimeSetter_ValueChanged);
            // 
            // ViewLightsCheckBox
            // 
            this.ViewLightsCheckBox.AutoSize = true;
            this.ViewLightsCheckBox.Location = new System.Drawing.Point(121, 42);
            this.ViewLightsCheckBox.Name = "ViewLightsCheckBox";
            this.ViewLightsCheckBox.Size = new System.Drawing.Size(54, 17);
            this.ViewLightsCheckBox.TabIndex = 321;
            this.ViewLightsCheckBox.Text = "Lights";
            this.ViewLightsCheckBox.UseVisualStyleBackColor = true;
            this.ViewLightsCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewColorRefsCheckBox
            // 
            this.ViewColorRefsCheckBox.AutoSize = true;
            this.ViewColorRefsCheckBox.Location = new System.Drawing.Point(12, 42);
            this.ViewColorRefsCheckBox.Name = "ViewColorRefsCheckBox";
            this.ViewColorRefsCheckBox.Size = new System.Drawing.Size(70, 17);
            this.ViewColorRefsCheckBox.TabIndex = 302;
            this.ViewColorRefsCheckBox.Text = "Color refs";
            this.ViewColorRefsCheckBox.UseVisualStyleBackColor = true;
            this.ViewColorRefsCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewNumbersFlatCheckBox
            // 
            this.ViewNumbersFlatCheckBox.AutoSize = true;
            this.ViewNumbersFlatCheckBox.Checked = true;
            this.ViewNumbersFlatCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewNumbersFlatCheckBox.Location = new System.Drawing.Point(12, 65);
            this.ViewNumbersFlatCheckBox.Name = "ViewNumbersFlatCheckBox";
            this.ViewNumbersFlatCheckBox.Size = new System.Drawing.Size(91, 17);
            this.ViewNumbersFlatCheckBox.TabIndex = 303;
            this.ViewNumbersFlatCheckBox.Text = "Numbers (flat)";
            this.ViewNumbersFlatCheckBox.UseVisualStyleBackColor = true;
            this.ViewNumbersFlatCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewGridCheckBox
            // 
            this.ViewGridCheckBox.AutoSize = true;
            this.ViewGridCheckBox.Checked = true;
            this.ViewGridCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewGridCheckBox.Location = new System.Drawing.Point(12, 111);
            this.ViewGridCheckBox.Name = "ViewGridCheckBox";
            this.ViewGridCheckBox.Size = new System.Drawing.Size(88, 17);
            this.ViewGridCheckBox.TabIndex = 305;
            this.ViewGridCheckBox.Text = "Grid && Border";
            this.ViewGridCheckBox.UseVisualStyleBackColor = true;
            this.ViewGridCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewNumbers3DCheckBox
            // 
            this.ViewNumbers3DCheckBox.AutoSize = true;
            this.ViewNumbers3DCheckBox.Checked = true;
            this.ViewNumbers3DCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewNumbers3DCheckBox.Location = new System.Drawing.Point(12, 88);
            this.ViewNumbers3DCheckBox.Name = "ViewNumbers3DCheckBox";
            this.ViewNumbers3DCheckBox.Size = new System.Drawing.Size(91, 17);
            this.ViewNumbers3DCheckBox.TabIndex = 304;
            this.ViewNumbers3DCheckBox.Text = "Numbers (3D)";
            this.ViewNumbers3DCheckBox.UseVisualStyleBackColor = true;
            this.ViewNumbers3DCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewShadowOutlinesCheckBox
            // 
            this.ViewShadowOutlinesCheckBox.AutoSize = true;
            this.ViewShadowOutlinesCheckBox.Location = new System.Drawing.Point(121, 65);
            this.ViewShadowOutlinesCheckBox.Name = "ViewShadowOutlinesCheckBox";
            this.ViewShadowOutlinesCheckBox.Size = new System.Drawing.Size(104, 17);
            this.ViewShadowOutlinesCheckBox.TabIndex = 322;
            this.ViewShadowOutlinesCheckBox.Text = "Shadow outlines";
            this.ViewShadowOutlinesCheckBox.UseVisualStyleBackColor = true;
            this.ViewShadowOutlinesCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewBoxesCheckbox
            // 
            this.ViewBoxesCheckbox.AutoSize = true;
            this.ViewBoxesCheckbox.Location = new System.Drawing.Point(121, 111);
            this.ViewBoxesCheckbox.Name = "ViewBoxesCheckbox";
            this.ViewBoxesCheckbox.Size = new System.Drawing.Size(55, 17);
            this.ViewBoxesCheckbox.TabIndex = 324;
            this.ViewBoxesCheckbox.Text = "Boxes";
            this.ViewBoxesCheckbox.UseVisualStyleBackColor = true;
            this.ViewBoxesCheckbox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewRenderingCheckBox
            // 
            this.ViewRenderingCheckBox.AutoSize = true;
            this.ViewRenderingCheckBox.Location = new System.Drawing.Point(121, 88);
            this.ViewRenderingCheckBox.Name = "ViewRenderingCheckBox";
            this.ViewRenderingCheckBox.Size = new System.Drawing.Size(75, 17);
            this.ViewRenderingCheckBox.TabIndex = 323;
            this.ViewRenderingCheckBox.Text = "Rendering";
            this.ViewRenderingCheckBox.UseVisualStyleBackColor = true;
            this.ViewRenderingCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewBallsCheckBox
            // 
            this.ViewBallsCheckBox.AutoSize = true;
            this.ViewBallsCheckBox.Checked = true;
            this.ViewBallsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewBallsCheckBox.Location = new System.Drawing.Point(121, 19);
            this.ViewBallsCheckBox.Name = "ViewBallsCheckBox";
            this.ViewBallsCheckBox.Size = new System.Drawing.Size(71, 17);
            this.ViewBallsCheckBox.TabIndex = 320;
            this.ViewBallsCheckBox.Text = "Balls (3D)";
            this.ViewBallsCheckBox.UseVisualStyleBackColor = true;
            this.ViewBallsCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // ViewBallOutlineFlatCheckBox
            // 
            this.ViewBallOutlineFlatCheckBox.AutoSize = true;
            this.ViewBallOutlineFlatCheckBox.Checked = true;
            this.ViewBallOutlineFlatCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewBallOutlineFlatCheckBox.Location = new System.Drawing.Point(12, 19);
            this.ViewBallOutlineFlatCheckBox.Name = "ViewBallOutlineFlatCheckBox";
            this.ViewBallOutlineFlatCheckBox.Size = new System.Drawing.Size(100, 17);
            this.ViewBallOutlineFlatCheckBox.TabIndex = 301;
            this.ViewBallOutlineFlatCheckBox.Text = "Ball outline (flat)";
            this.ViewBallOutlineFlatCheckBox.UseVisualStyleBackColor = true;
            this.ViewBallOutlineFlatCheckBox.CheckedChanged += new System.EventHandler(this.ViewOptionsCheckBox_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ViewEnabledCheckbox);
            this.groupBox2.Controls.Add(this.ViewRotation2Setter);
            this.groupBox2.Controls.Add(this.ViewRotation1Setter);
            this.groupBox2.Controls.Add(this.ViewDistanceSetter);
            this.groupBox2.Location = new System.Drawing.Point(3, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 143);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3D View";
            // 
            // ViewEnabledCheckbox
            // 
            this.ViewEnabledCheckbox.AutoSize = true;
            this.ViewEnabledCheckbox.Location = new System.Drawing.Point(14, 23);
            this.ViewEnabledCheckbox.Name = "ViewEnabledCheckbox";
            this.ViewEnabledCheckbox.Size = new System.Drawing.Size(59, 17);
            this.ViewEnabledCheckbox.TabIndex = 201;
            this.ViewEnabledCheckbox.Text = "Enable";
            this.ViewEnabledCheckbox.UseVisualStyleBackColor = true;
            this.ViewEnabledCheckbox.CheckedChanged += new System.EventHandler(this.ViewEnabledCheckbox_CheckedChanged);
            // 
            // ViewRotation2Setter
            // 
            this.ViewRotation2Setter.Dimension = "°";
            this.ViewRotation2Setter.Label = "Rotation 2";
            this.ViewRotation2Setter.Location = new System.Drawing.Point(8, 105);
            this.ViewRotation2Setter.Max = 360D;
            this.ViewRotation2Setter.Min = 0D;
            this.ViewRotation2Setter.Name = "ViewRotation2Setter";
            this.ViewRotation2Setter.Size = new System.Drawing.Size(225, 23);
            this.ViewRotation2Setter.TabIndex = 204;
            this.ViewRotation2Setter.Value = 4.9968D;
            this.ViewRotation2Setter.ValueChanged += new System.EventHandler(this.ViewSetter_ValueChanged);
            // 
            // ViewRotation1Setter
            // 
            this.ViewRotation1Setter.Dimension = "°";
            this.ViewRotation1Setter.Label = "Rotation 1";
            this.ViewRotation1Setter.Location = new System.Drawing.Point(8, 76);
            this.ViewRotation1Setter.Max = -0.1D;
            this.ViewRotation1Setter.Min = -90D;
            this.ViewRotation1Setter.Name = "ViewRotation1Setter";
            this.ViewRotation1Setter.Size = new System.Drawing.Size(225, 23);
            this.ViewRotation1Setter.TabIndex = 203;
            this.ViewRotation1Setter.Value = -30.00074D;
            this.ViewRotation1Setter.ValueChanged += new System.EventHandler(this.ViewSetter_ValueChanged);
            // 
            // ViewDistanceSetter
            // 
            this.ViewDistanceSetter.Dimension = "";
            this.ViewDistanceSetter.Label = "Distance";
            this.ViewDistanceSetter.Location = new System.Drawing.Point(8, 47);
            this.ViewDistanceSetter.Max = 2000D;
            this.ViewDistanceSetter.Min = 0.1D;
            this.ViewDistanceSetter.Name = "ViewDistanceSetter";
            this.ViewDistanceSetter.Size = new System.Drawing.Size(225, 23);
            this.ViewDistanceSetter.TabIndex = 202;
            this.ViewDistanceSetter.Value = 34.978256D;
            this.ViewDistanceSetter.ValueChanged += new System.EventHandler(this.ViewSetter_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CamRotationSetter);
            this.groupBox1.Controls.Add(this.OffsetYSetter);
            this.groupBox1.Controls.Add(this.OffsetXSetter);
            this.groupBox1.Controls.Add(this.CamDistSetter);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ApertureVLabel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ApertureHLabel);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera";
            // 
            // CamRotationSetter
            // 
            this.CamRotationSetter.Dimension = "°";
            this.CamRotationSetter.Label = "Rotation";
            this.CamRotationSetter.Location = new System.Drawing.Point(8, 106);
            this.CamRotationSetter.Max = 360D;
            this.CamRotationSetter.Min = 0D;
            this.CamRotationSetter.Name = "CamRotationSetter";
            this.CamRotationSetter.Size = new System.Drawing.Size(247, 23);
            this.CamRotationSetter.TabIndex = 103;
            this.CamRotationSetter.Value = 34.9992D;
            this.CamRotationSetter.ValueChanged += new System.EventHandler(this.CamRotationSetter_ValueChanged);
            // 
            // OffsetYSetter
            // 
            this.OffsetYSetter.Dimension = "";
            this.OffsetYSetter.Label = "Offset Y";
            this.OffsetYSetter.Location = new System.Drawing.Point(8, 77);
            this.OffsetYSetter.Max = 2D;
            this.OffsetYSetter.Min = -2D;
            this.OffsetYSetter.Name = "OffsetYSetter";
            this.OffsetYSetter.Size = new System.Drawing.Size(247, 23);
            this.OffsetYSetter.TabIndex = 102;
            this.OffsetYSetter.Value = -0.00019999999999997797D;
            this.OffsetYSetter.ValueChanged += new System.EventHandler(this.CamRotationSetter_ValueChanged);
            // 
            // OffsetXSetter
            // 
            this.OffsetXSetter.Dimension = "";
            this.OffsetXSetter.Label = "Offset X";
            this.OffsetXSetter.Location = new System.Drawing.Point(8, 48);
            this.OffsetXSetter.Max = 2D;
            this.OffsetXSetter.Min = -2D;
            this.OffsetXSetter.Name = "OffsetXSetter";
            this.OffsetXSetter.Size = new System.Drawing.Size(247, 23);
            this.OffsetXSetter.TabIndex = 101;
            this.OffsetXSetter.Value = -0.0042400000000000215D;
            this.OffsetXSetter.ValueChanged += new System.EventHandler(this.CamRotationSetter_ValueChanged);
            // 
            // CamDistSetter
            // 
            this.CamDistSetter.Dimension = "";
            this.CamDistSetter.Label = "Distance";
            this.CamDistSetter.Location = new System.Drawing.Point(8, 19);
            this.CamDistSetter.Max = 100D;
            this.CamDistSetter.Min = 5D;
            this.CamDistSetter.Name = "CamDistSetter";
            this.CamDistSetter.Size = new System.Drawing.Size(247, 23);
            this.CamDistSetter.TabIndex = 100;
            this.CamDistSetter.Value = 5D;
            this.CamDistSetter.ValueChanged += new System.EventHandler(this.CamDistSetter_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "ApertureH =";
            // 
            // ApertureVLabel
            // 
            this.ApertureVLabel.AutoSize = true;
            this.ApertureVLabel.Location = new System.Drawing.Point(189, 140);
            this.ApertureVLabel.Name = "ApertureVLabel";
            this.ApertureVLabel.Size = new System.Drawing.Size(26, 13);
            this.ApertureVLabel.TabIndex = 6;
            this.ApertureVLabel.Text = "0,0°";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "ApertureV =";
            // 
            // ApertureHLabel
            // 
            this.ApertureHLabel.AutoSize = true;
            this.ApertureHLabel.Location = new System.Drawing.Point(78, 140);
            this.ApertureHLabel.Name = "ApertureHLabel";
            this.ApertureHLabel.Size = new System.Drawing.Size(26, 13);
            this.ApertureHLabel.TabIndex = 6;
            this.ApertureHLabel.Text = "0,0°";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CubeMapContextActiveCubeMap,
            this.cubemapsToolStripMenuItem,
            this.spheremapsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 97);
            // 
            // CubeMapContextActiveCubeMap
            // 
            this.CubeMapContextActiveCubeMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CubeMapContextActiveCubeMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CubeMapContextActiveCubeMap.Items.AddRange(new object[] {
            "Ball 1",
            "Ball 9c",
            "Ball 8a",
            "Ball 4a"});
            this.CubeMapContextActiveCubeMap.Name = "CubeMapContextActiveCubeMap";
            this.CubeMapContextActiveCubeMap.Size = new System.Drawing.Size(121, 23);
            this.CubeMapContextActiveCubeMap.SelectedIndexChanged += new System.EventHandler(this.CubeMapContextActiveCubeMap_SelectedIndexChanged);
            // 
            // cubemapsToolStripMenuItem
            // 
            this.cubemapsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CubemapsRecalcCoarseMenuItem,
            this.CubemapsRecalcFineMenuItem,
            this.CubemapsCopyMenuItem});
            this.cubemapsToolStripMenuItem.Name = "cubemapsToolStripMenuItem";
            this.cubemapsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.cubemapsToolStripMenuItem.Text = "Cubemaps";
            // 
            // CubemapsRecalcCoarseMenuItem
            // 
            this.CubemapsRecalcCoarseMenuItem.Name = "CubemapsRecalcCoarseMenuItem";
            this.CubemapsRecalcCoarseMenuItem.Size = new System.Drawing.Size(147, 22);
            this.CubemapsRecalcCoarseMenuItem.Text = "Recalc Coarse";
            this.CubemapsRecalcCoarseMenuItem.Click += new System.EventHandler(this.CubemapsRecalcCoarseMenuItem_Click);
            // 
            // CubemapsRecalcFineMenuItem
            // 
            this.CubemapsRecalcFineMenuItem.Name = "CubemapsRecalcFineMenuItem";
            this.CubemapsRecalcFineMenuItem.Size = new System.Drawing.Size(147, 22);
            this.CubemapsRecalcFineMenuItem.Text = "Recalc Fine";
            this.CubemapsRecalcFineMenuItem.Click += new System.EventHandler(this.CubemapsRecalcFineMenuItem_Click);
            // 
            // CubemapsCopyMenuItem
            // 
            this.CubemapsCopyMenuItem.Name = "CubemapsCopyMenuItem";
            this.CubemapsCopyMenuItem.Size = new System.Drawing.Size(147, 22);
            this.CubemapsCopyMenuItem.Text = "Copy";
            this.CubemapsCopyMenuItem.Click += new System.EventHandler(this.CubemapsCopyMenuItem_Click);
            // 
            // spheremapsToolStripMenuItem
            // 
            this.spheremapsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SpheremapsRecalcCoarseMenuItem,
            this.SpheremapsCopyMenuItem});
            this.spheremapsToolStripMenuItem.Name = "spheremapsToolStripMenuItem";
            this.spheremapsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.spheremapsToolStripMenuItem.Text = "Spheremaps";
            // 
            // SpheremapsRecalcCoarseMenuItem
            // 
            this.SpheremapsRecalcCoarseMenuItem.Name = "SpheremapsRecalcCoarseMenuItem";
            this.SpheremapsRecalcCoarseMenuItem.Size = new System.Drawing.Size(134, 22);
            this.SpheremapsRecalcCoarseMenuItem.Text = "Recalculate";
            this.SpheremapsRecalcCoarseMenuItem.Click += new System.EventHandler(this.SpheremapsRecalculateMenuItem_Click);
            // 
            // SpheremapsCopyMenuItem
            // 
            this.SpheremapsCopyMenuItem.Name = "SpheremapsCopyMenuItem";
            this.SpheremapsCopyMenuItem.Size = new System.Drawing.Size(134, 22);
            this.SpheremapsCopyMenuItem.Text = "Copy";
            this.SpheremapsCopyMenuItem.Click += new System.EventHandler(this.SpheremapsCopyMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(819, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 823);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.splitter2);
            this.LeftPanel.Controls.Add(this.RenderBox);
            this.LeftPanel.Controls.Add(this.CubeMapBox);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(819, 823);
            this.LeftPanel.TabIndex = 3;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 505);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(819, 3);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // RenderBox
            // 
            this.RenderBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.RenderBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderBox.ErrorImage = global::Pool1984.Properties.Resources.Close_up_corrected;
            this.RenderBox.Image = global::Pool1984.Properties.Resources.Original;
            this.RenderBox.Location = new System.Drawing.Point(0, 0);
            this.RenderBox.Name = "RenderBox";
            this.RenderBox.Offset = ((System.Drawing.PointF)(resources.GetObject("RenderBox.Offset")));
            this.RenderBox.Size = new System.Drawing.Size(819, 508);
            this.RenderBox.TabIndex = 0;
            this.RenderBox.TabStop = false;
            this.RenderBox.Zoom = 1F;
            this.RenderBox.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderBox_Paint);
            this.RenderBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RenderBox_MouseClick);
            this.RenderBox.MouseHover += new System.EventHandler(this.RenderBox_MouseHover);
            // 
            // CubeMapBox
            // 
            this.CubeMapBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CubeMapBox.ContextMenuStrip = this.contextMenuStrip1;
            this.CubeMapBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CubeMapBox.ErrorImage = global::Pool1984.Properties.Resources.Cubemap;
            this.CubeMapBox.Image = global::Pool1984.Properties.Resources.Ball_texture_2;
            this.CubeMapBox.Location = new System.Drawing.Point(0, 508);
            this.CubeMapBox.Name = "CubeMapBox";
            this.CubeMapBox.Offset = ((System.Drawing.PointF)(resources.GetObject("CubeMapBox.Offset")));
            this.CubeMapBox.Size = new System.Drawing.Size(819, 315);
            this.CubeMapBox.TabIndex = 2;
            this.CubeMapBox.TabStop = false;
            this.CubeMapBox.Zoom = 0.5F;
            this.CubeMapBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CubeMapBox_Paint);
            this.CubeMapBox.MouseHover += new System.EventHandler(this.CubeMapBox_MouseHover);
            // 
            // rerToolStripMenuItem
            // 
            this.rerToolStripMenuItem.Name = "rerToolStripMenuItem";
            this.rerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rerToolStripMenuItem.Text = "rer";
            // 
            // SaveRenderingDialog
            // 
            this.SaveRenderingDialog.DefaultExt = "png";
            this.SaveRenderingDialog.Filter = "PNG-files|*.png";
            // 
            // ExportDialog
            // 
            this.ExportDialog.DefaultExt = "png";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 823);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.RightPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.RightPanel.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RenderBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CubeMapBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZoomablePictureBox RenderBox;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ViewEnabledCheckbox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel LeftPanel;
        private ZoomablePictureBox CubeMapBox;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripComboBox CubeMapContextActiveCubeMap;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button RenderButton;
        private System.Windows.Forms.CheckBox ViewBallOutlineFlatCheckBox;
        private System.Windows.Forms.CheckBox ViewRenderingCheckBox;
        private System.Windows.Forms.ToolStripMenuItem cubemapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spheremapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CubemapsRecalcCoarseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CubemapsRecalcFineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CubemapsCopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SpheremapsRecalcCoarseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SpheremapsCopyMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ApertureVLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ApertureHLabel;
        private ValueSetter OffsetYSetter;
        private ValueSetter OffsetXSetter;
        private ValueSetter CamDistSetter;
        private ValueSetter ViewRotation2Setter;
        private ValueSetter ViewRotation1Setter;
        private ValueSetter ViewDistanceSetter;
        private ValueSetter CamRotationSetter;
        private System.Windows.Forms.CheckBox ViewColorRefsCheckBox;
        private ValueSetter timeSetter;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox ViewLightsCheckBox;
        private System.Windows.Forms.CheckBox ViewNumbersFlatCheckBox;
        private System.Windows.Forms.CheckBox ViewNumbers3DCheckBox;
        private System.Windows.Forms.CheckBox ViewShadowOutlinesCheckBox;
        private System.Windows.Forms.CheckBox ViewBallsCheckBox;
        private System.Windows.Forms.Button SaveRenderingButton;
        private System.Windows.Forms.SaveFileDialog SaveRenderingDialog;
        private System.Windows.Forms.CheckBox ViewGridCheckBox;
        private System.Windows.Forms.CheckBox ViewBoxesCheckbox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.ComboBox ExportersCombo;
        private System.Windows.Forms.SaveFileDialog ExportDialog;
    }
}

