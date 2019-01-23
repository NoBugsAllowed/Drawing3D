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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbShadingPhong = new System.Windows.Forms.RadioButton();
            this.rbShadingGouraud = new System.Windows.Forms.RadioButton();
            this.rbShadingFlat = new System.Windows.Forms.RadioButton();
            this.gbCamera = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.gbShading = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.gbFov = new System.Windows.Forms.GroupBox();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbCamera.SuspendLayout();
            this.gbShading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.gbFov.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(784, 562);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gbFov, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbCamera, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbShading, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(130, 562);
            this.tableLayoutPanel1.TabIndex = 0;
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
            // gbCamera
            // 
            this.gbCamera.Controls.Add(this.radioButton3);
            this.gbCamera.Controls.Add(this.radioButton2);
            this.gbCamera.Controls.Add(this.radioButton1);
            this.gbCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCamera.Location = new System.Drawing.Point(3, 3);
            this.gbCamera.Name = "gbCamera";
            this.gbCamera.Size = new System.Drawing.Size(124, 94);
            this.gbCamera.TabIndex = 6;
            this.gbCamera.TabStop = false;
            this.gbCamera.Text = "Camera";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 68);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(101, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Moving with ball";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 44);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(97, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Looking on ball";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(52, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Static";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // gbShading
            // 
            this.gbShading.Controls.Add(this.rbShadingPhong);
            this.gbShading.Controls.Add(this.rbShadingFlat);
            this.gbShading.Controls.Add(this.rbShadingGouraud);
            this.gbShading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbShading.Location = new System.Drawing.Point(3, 103);
            this.gbShading.Name = "gbShading";
            this.gbShading.Size = new System.Drawing.Size(124, 94);
            this.gbShading.TabIndex = 12;
            this.gbShading.TabStop = false;
            this.gbShading.Text = "Shading";
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
            // gbFov
            // 
            this.gbFov.Controls.Add(this.numericUpDown);
            this.gbFov.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFov.Location = new System.Drawing.Point(3, 203);
            this.gbFov.Name = "gbFov";
            this.gbFov.Size = new System.Drawing.Size(124, 56);
            this.gbFov.TabIndex = 13;
            this.gbFov.TabStop = false;
            this.gbFov.Text = "Fov";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Dock = System.Windows.Forms.DockStyle.Top;
            this.numericUpDown.Location = new System.Drawing.Point(3, 16);
            this.numericUpDown.Maximum = new decimal(new int[] {
            180,
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
            this.numericUpDown.TabIndex = 2;
            this.numericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbCamera.ResumeLayout(false);
            this.gbCamera.PerformLayout();
            this.gbShading.ResumeLayout(false);
            this.gbShading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.gbFov.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbShadingPhong;
        private System.Windows.Forms.RadioButton rbShadingGouraud;
        private System.Windows.Forms.RadioButton rbShadingFlat;
        private System.Windows.Forms.GroupBox gbCamera;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gbShading;
        private System.Windows.Forms.GroupBox gbFov;
        private System.Windows.Forms.NumericUpDown numericUpDown;
    }
}

