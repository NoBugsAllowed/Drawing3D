namespace Drawing3D
{
    partial class MainForm
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gbLights = new System.Windows.Forms.GroupBox();
            this.cbPointLight = new System.Windows.Forms.CheckBox();
            this.cbReflector = new System.Windows.Forms.CheckBox();
            this.cbDirectional = new System.Windows.Forms.CheckBox();
            this.gbCamera = new System.Windows.Forms.GroupBox();
            this.rbStaticBackCamera = new System.Windows.Forms.RadioButton();
            this.rbMovingCamera = new System.Windows.Forms.RadioButton();
            this.rbFocusedCamera = new System.Windows.Forms.RadioButton();
            this.rbStaticFrontCamera = new System.Windows.Forms.RadioButton();
            this.gbShading = new System.Windows.Forms.GroupBox();
            this.rbShadingPhong = new System.Windows.Forms.RadioButton();
            this.rbShadingFlat = new System.Windows.Forms.RadioButton();
            this.rbShadingGouraud = new System.Windows.Forms.RadioButton();
            this.gbFov = new System.Windows.Forms.GroupBox();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.gbAnimation = new System.Windows.Forms.GroupBox();
            this.btnAnimation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.gbLights.SuspendLayout();
            this.gbCamera.SuspendLayout();
            this.gbShading.SuspendLayout();
            this.gbFov.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.gbAnimation.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer.Size = new System.Drawing.Size(784, 562);
            this.splitContainer.SplitterDistance = 130;
            this.splitContainer.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.gbLights, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.gbCamera, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.gbShading, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.gbFov, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.gbAnimation, 0, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(130, 562);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // gbLights
            // 
            this.gbLights.Controls.Add(this.cbPointLight);
            this.gbLights.Controls.Add(this.cbReflector);
            this.gbLights.Controls.Add(this.cbDirectional);
            this.gbLights.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbLights.Location = new System.Drawing.Point(3, 222);
            this.gbLights.Name = "gbLights";
            this.gbLights.Size = new System.Drawing.Size(124, 86);
            this.gbLights.TabIndex = 14;
            this.gbLights.TabStop = false;
            this.gbLights.Text = "Lights";
            // 
            // cbPointLight
            // 
            this.cbPointLight.AutoSize = true;
            this.cbPointLight.Location = new System.Drawing.Point(7, 67);
            this.cbPointLight.Name = "cbPointLight";
            this.cbPointLight.Size = new System.Drawing.Size(72, 17);
            this.cbPointLight.TabIndex = 2;
            this.cbPointLight.Text = "Point light";
            this.cbPointLight.UseVisualStyleBackColor = true;
            this.cbPointLight.CheckedChanged += new System.EventHandler(this.cbPointLight_CheckedChanged);
            // 
            // cbReflector
            // 
            this.cbReflector.AutoSize = true;
            this.cbReflector.Location = new System.Drawing.Point(7, 44);
            this.cbReflector.Name = "cbReflector";
            this.cbReflector.Size = new System.Drawing.Size(69, 17);
            this.cbReflector.TabIndex = 1;
            this.cbReflector.Text = "Reflector";
            this.cbReflector.UseVisualStyleBackColor = true;
            this.cbReflector.CheckedChanged += new System.EventHandler(this.cbReflector_CheckedChanged);
            // 
            // cbDirectional
            // 
            this.cbDirectional.AutoSize = true;
            this.cbDirectional.Checked = true;
            this.cbDirectional.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDirectional.Location = new System.Drawing.Point(7, 20);
            this.cbDirectional.Name = "cbDirectional";
            this.cbDirectional.Size = new System.Drawing.Size(98, 17);
            this.cbDirectional.TabIndex = 0;
            this.cbDirectional.Text = "Directional light";
            this.cbDirectional.UseVisualStyleBackColor = true;
            this.cbDirectional.CheckedChanged += new System.EventHandler(this.cbDirectional_CheckedChanged);
            // 
            // gbCamera
            // 
            this.gbCamera.Controls.Add(this.rbStaticBackCamera);
            this.gbCamera.Controls.Add(this.rbMovingCamera);
            this.gbCamera.Controls.Add(this.rbFocusedCamera);
            this.gbCamera.Controls.Add(this.rbStaticFrontCamera);
            this.gbCamera.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbCamera.Location = new System.Drawing.Point(3, 3);
            this.gbCamera.Name = "gbCamera";
            this.gbCamera.Size = new System.Drawing.Size(124, 113);
            this.gbCamera.TabIndex = 6;
            this.gbCamera.TabStop = false;
            this.gbCamera.Text = "Camera";
            // 
            // rbStaticBackCamera
            // 
            this.rbStaticBackCamera.AutoSize = true;
            this.rbStaticBackCamera.Location = new System.Drawing.Point(7, 43);
            this.rbStaticBackCamera.Name = "rbStaticBackCamera";
            this.rbStaticBackCamera.Size = new System.Drawing.Size(85, 17);
            this.rbStaticBackCamera.TabIndex = 3;
            this.rbStaticBackCamera.TabStop = true;
            this.rbStaticBackCamera.Text = "Static (back)";
            this.rbStaticBackCamera.UseVisualStyleBackColor = true;
            this.rbStaticBackCamera.CheckedChanged += new System.EventHandler(this.rbStaticBackCamera_CheckedChanged);
            // 
            // rbMovingCamera
            // 
            this.rbMovingCamera.AutoSize = true;
            this.rbMovingCamera.Location = new System.Drawing.Point(7, 90);
            this.rbMovingCamera.Name = "rbMovingCamera";
            this.rbMovingCamera.Size = new System.Drawing.Size(101, 17);
            this.rbMovingCamera.TabIndex = 2;
            this.rbMovingCamera.Text = "Moving with ball";
            this.rbMovingCamera.UseVisualStyleBackColor = true;
            this.rbMovingCamera.CheckedChanged += new System.EventHandler(this.rbMovingCamera_CheckedChanged);
            // 
            // rbFocusedCamera
            // 
            this.rbFocusedCamera.AutoSize = true;
            this.rbFocusedCamera.Location = new System.Drawing.Point(7, 66);
            this.rbFocusedCamera.Name = "rbFocusedCamera";
            this.rbFocusedCamera.Size = new System.Drawing.Size(97, 17);
            this.rbFocusedCamera.TabIndex = 1;
            this.rbFocusedCamera.Text = "Looking on ball";
            this.rbFocusedCamera.UseVisualStyleBackColor = true;
            this.rbFocusedCamera.CheckedChanged += new System.EventHandler(this.rbFocusedCamera_CheckedChanged);
            // 
            // rbStaticFrontCamera
            // 
            this.rbStaticFrontCamera.AutoSize = true;
            this.rbStaticFrontCamera.Checked = true;
            this.rbStaticFrontCamera.Location = new System.Drawing.Point(7, 20);
            this.rbStaticFrontCamera.Name = "rbStaticFrontCamera";
            this.rbStaticFrontCamera.Size = new System.Drawing.Size(82, 17);
            this.rbStaticFrontCamera.TabIndex = 0;
            this.rbStaticFrontCamera.TabStop = true;
            this.rbStaticFrontCamera.Text = "Static (front)";
            this.rbStaticFrontCamera.UseVisualStyleBackColor = true;
            this.rbStaticFrontCamera.CheckedChanged += new System.EventHandler(this.rbStaticFrontCamera_CheckedChanged);
            // 
            // gbShading
            // 
            this.gbShading.Controls.Add(this.rbShadingPhong);
            this.gbShading.Controls.Add(this.rbShadingFlat);
            this.gbShading.Controls.Add(this.rbShadingGouraud);
            this.gbShading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbShading.Location = new System.Drawing.Point(3, 122);
            this.gbShading.Name = "gbShading";
            this.gbShading.Size = new System.Drawing.Size(124, 94);
            this.gbShading.TabIndex = 12;
            this.gbShading.TabStop = false;
            this.gbShading.Text = "Shading";
            // 
            // rbShadingPhong
            // 
            this.rbShadingPhong.AutoSize = true;
            this.rbShadingPhong.Location = new System.Drawing.Point(7, 65);
            this.rbShadingPhong.Name = "rbShadingPhong";
            this.rbShadingPhong.Size = new System.Drawing.Size(56, 17);
            this.rbShadingPhong.TabIndex = 2;
            this.rbShadingPhong.Text = "Phong";
            this.rbShadingPhong.UseVisualStyleBackColor = true;
            this.rbShadingPhong.CheckedChanged += new System.EventHandler(this.rbShadingPhong_CheckedChanged);
            // 
            // rbShadingFlat
            // 
            this.rbShadingFlat.AutoSize = true;
            this.rbShadingFlat.Checked = true;
            this.rbShadingFlat.Location = new System.Drawing.Point(7, 19);
            this.rbShadingFlat.Name = "rbShadingFlat";
            this.rbShadingFlat.Size = new System.Drawing.Size(42, 17);
            this.rbShadingFlat.TabIndex = 0;
            this.rbShadingFlat.TabStop = true;
            this.rbShadingFlat.Text = "Flat";
            this.rbShadingFlat.UseVisualStyleBackColor = true;
            this.rbShadingFlat.CheckedChanged += new System.EventHandler(this.rbShadingFlat_CheckedChanged);
            // 
            // rbShadingGouraud
            // 
            this.rbShadingGouraud.AutoSize = true;
            this.rbShadingGouraud.Location = new System.Drawing.Point(7, 42);
            this.rbShadingGouraud.Name = "rbShadingGouraud";
            this.rbShadingGouraud.Size = new System.Drawing.Size(66, 17);
            this.rbShadingGouraud.TabIndex = 1;
            this.rbShadingGouraud.Text = "Gouraud";
            this.rbShadingGouraud.UseVisualStyleBackColor = true;
            this.rbShadingGouraud.CheckedChanged += new System.EventHandler(this.rbShadingGouraud_CheckedChanged);
            // 
            // gbFov
            // 
            this.gbFov.Controls.Add(this.numericUpDown);
            this.gbFov.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFov.Location = new System.Drawing.Point(3, 314);
            this.gbFov.Name = "gbFov";
            this.gbFov.Size = new System.Drawing.Size(124, 41);
            this.gbFov.TabIndex = 15;
            this.gbFov.TabStop = false;
            this.gbFov.Text = "Fov";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Dock = System.Windows.Forms.DockStyle.Top;
            this.numericUpDown.Location = new System.Drawing.Point(3, 16);
            this.numericUpDown.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDown.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(118, 20);
            this.numericUpDown.TabIndex = 3;
            this.numericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(650, 562);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // gbAnimation
            // 
            this.gbAnimation.Controls.Add(this.btnAnimation);
            this.gbAnimation.Location = new System.Drawing.Point(3, 361);
            this.gbAnimation.Name = "gbAnimation";
            this.gbAnimation.Size = new System.Drawing.Size(124, 198);
            this.gbAnimation.TabIndex = 16;
            this.gbAnimation.TabStop = false;
            this.gbAnimation.Text = "Animation";
            // 
            // btnAnimation
            // 
            this.btnAnimation.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAnimation.Location = new System.Drawing.Point(3, 16);
            this.btnAnimation.Name = "btnAnimation";
            this.btnAnimation.Size = new System.Drawing.Size(118, 23);
            this.btnAnimation.TabIndex = 0;
            this.btnAnimation.Text = "Start/Stop";
            this.btnAnimation.UseVisualStyleBackColor = true;
            this.btnAnimation.Click += new System.EventHandler(this.btnAnimation_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Bowling";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.gbLights.ResumeLayout(false);
            this.gbLights.PerformLayout();
            this.gbCamera.ResumeLayout(false);
            this.gbCamera.PerformLayout();
            this.gbShading.ResumeLayout(false);
            this.gbShading.PerformLayout();
            this.gbFov.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.gbAnimation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.RadioButton rbShadingPhong;
        private System.Windows.Forms.RadioButton rbShadingGouraud;
        private System.Windows.Forms.RadioButton rbShadingFlat;
        private System.Windows.Forms.GroupBox gbCamera;
        private System.Windows.Forms.RadioButton rbMovingCamera;
        private System.Windows.Forms.RadioButton rbFocusedCamera;
        private System.Windows.Forms.RadioButton rbStaticFrontCamera;
        private System.Windows.Forms.GroupBox gbShading;
        private System.Windows.Forms.GroupBox gbLights;
        private System.Windows.Forms.GroupBox gbFov;
        private System.Windows.Forms.NumericUpDown numericUpDown;
        private System.Windows.Forms.CheckBox cbDirectional;
        private System.Windows.Forms.CheckBox cbReflector;
        private System.Windows.Forms.CheckBox cbPointLight;
        private System.Windows.Forms.RadioButton rbStaticBackCamera;
        private System.Windows.Forms.GroupBox gbAnimation;
        private System.Windows.Forms.Button btnAnimation;
    }
}

