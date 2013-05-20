namespace ARSIE.SupervisedClassifier
{
    partial class FrmSubsetV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSubsetV2));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvFeatures = new System.Windows.Forms.DataGridView();
            this.ColSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gboxOptimize = new System.Windows.Forms.GroupBox();
            this.rbtnGainRatio = new System.Windows.Forms.RadioButton();
            this.btnBiIdentify = new System.Windows.Forms.Button();
            this.btnIdentify = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.dUDBestFields = new System.Windows.Forms.DomainUpDown();
            this.lblBest = new System.Windows.Forms.Label();
            this.rbtnGINI = new System.Windows.Forms.RadioButton();
            this.rbtnEntropy = new System.Windows.Forms.RadioButton();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFeatures)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gboxOptimize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvFeatures);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(534, 362);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvFeatures
            // 
            this.dgvFeatures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFeatures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSelect,
            this.ColName,
            this.DataType,
            this.ColType});
            this.dgvFeatures.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvFeatures.Location = new System.Drawing.Point(0, 0);
            this.dgvFeatures.Name = "dgvFeatures";
            this.dgvFeatures.Size = new System.Drawing.Size(530, 157);
            this.dgvFeatures.TabIndex = 4;
            // 
            // ColSelect
            // 
            this.ColSelect.HeaderText = "Include";
            this.ColSelect.Name = "ColSelect";
            // 
            // ColName
            // 
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // DataType
            // 
            this.DataType.HeaderText = "Data Type";
            this.DataType.Name = "DataType";
            // 
            // ColType
            // 
            this.ColType.HeaderText = "Feature Type";
            this.ColType.Items.AddRange(new object[] {
            "Continuous",
            "Discrete"});
            this.ColType.Name = "ColType";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gboxOptimize);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvResult);
            this.splitContainer2.Size = new System.Drawing.Size(534, 226);
            this.splitContainer2.SplitterDistance = 242;
            this.splitContainer2.TabIndex = 0;
            // 
            // gboxOptimize
            // 
            this.gboxOptimize.Controls.Add(this.rbtnGainRatio);
            this.gboxOptimize.Controls.Add(this.btnBiIdentify);
            this.gboxOptimize.Controls.Add(this.btnIdentify);
            this.gboxOptimize.Controls.Add(this.btnCalculate);
            this.gboxOptimize.Controls.Add(this.dUDBestFields);
            this.gboxOptimize.Controls.Add(this.lblBest);
            this.gboxOptimize.Controls.Add(this.rbtnGINI);
            this.gboxOptimize.Controls.Add(this.rbtnEntropy);
            this.gboxOptimize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gboxOptimize.Location = new System.Drawing.Point(0, 0);
            this.gboxOptimize.Name = "gboxOptimize";
            this.gboxOptimize.Size = new System.Drawing.Size(238, 222);
            this.gboxOptimize.TabIndex = 1;
            this.gboxOptimize.TabStop = false;
            this.gboxOptimize.Text = "OPTIMIZE";
            // 
            // rbtnGainRatio
            // 
            this.rbtnGainRatio.AutoSize = true;
            this.rbtnGainRatio.Location = new System.Drawing.Point(6, 53);
            this.rbtnGainRatio.Name = "rbtnGainRatio";
            this.rbtnGainRatio.Size = new System.Drawing.Size(75, 17);
            this.rbtnGainRatio.TabIndex = 37;
            this.rbtnGainRatio.Text = "Gain Ratio";
            this.rbtnGainRatio.UseVisualStyleBackColor = true;
            // 
            // btnBiIdentify
            // 
            this.btnBiIdentify.Location = new System.Drawing.Point(7, 130);
            this.btnBiIdentify.Name = "btnBiIdentify";
            this.btnBiIdentify.Size = new System.Drawing.Size(103, 33);
            this.btnBiIdentify.TabIndex = 34;
            this.btnBiIdentify.Text = "BI-IDENTIFY";
            this.btnBiIdentify.UseVisualStyleBackColor = true;
            this.btnBiIdentify.Click += new System.EventHandler(this.btnBiIdentify_Click);
            // 
            // btnIdentify
            // 
            this.btnIdentify.Location = new System.Drawing.Point(8, 176);
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(103, 33);
            this.btnIdentify.TabIndex = 33;
            this.btnIdentify.Text = "IDENTIFY";
            this.btnIdentify.UseVisualStyleBackColor = true;
            this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(139, 176);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(88, 33);
            this.btnCalculate.TabIndex = 31;
            this.btnCalculate.Text = "SET UP";
            this.btnCalculate.UseVisualStyleBackColor = true;
            // 
            // dUDBestFields
            // 
            this.dUDBestFields.Items.Add("1");
            this.dUDBestFields.Items.Add("2");
            this.dUDBestFields.Items.Add("3");
            this.dUDBestFields.Location = new System.Drawing.Point(144, 77);
            this.dUDBestFields.Name = "dUDBestFields";
            this.dUDBestFields.Size = new System.Drawing.Size(83, 20);
            this.dUDBestFields.TabIndex = 3;
            this.dUDBestFields.Text = "1";
            // 
            // lblBest
            // 
            this.lblBest.AutoSize = true;
            this.lblBest.Location = new System.Drawing.Point(98, 82);
            this.lblBest.Name = "lblBest";
            this.lblBest.Size = new System.Drawing.Size(38, 13);
            this.lblBest.TabIndex = 2;
            this.lblBest.Text = "BEST:";
            // 
            // rbtnGINI
            // 
            this.rbtnGINI.AutoSize = true;
            this.rbtnGINI.Location = new System.Drawing.Point(8, 82);
            this.rbtnGINI.Name = "rbtnGINI";
            this.rbtnGINI.Size = new System.Drawing.Size(47, 17);
            this.rbtnGINI.TabIndex = 1;
            this.rbtnGINI.Text = "GINI";
            this.rbtnGINI.UseVisualStyleBackColor = true;
            // 
            // rbtnEntropy
            // 
            this.rbtnEntropy.AutoSize = true;
            this.rbtnEntropy.Checked = true;
            this.rbtnEntropy.Location = new System.Drawing.Point(8, 26);
            this.rbtnEntropy.Name = "rbtnEntropy";
            this.rbtnEntropy.Size = new System.Drawing.Size(47, 17);
            this.rbtnEntropy.TabIndex = 0;
            this.rbtnEntropy.TabStop = true;
            this.rbtnEntropy.Text = "Gain";
            this.rbtnEntropy.UseVisualStyleBackColor = true;
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(0, 0);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.Size = new System.Drawing.Size(284, 222);
            this.dgvResult.TabIndex = 5;
            // 
            // FrmSubsetV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 362);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSubsetV2";
            this.Text = "Feature Subset";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFeatures)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.gboxOptimize.ResumeLayout(false);
            this.gboxOptimize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox gboxOptimize;
        private System.Windows.Forms.RadioButton rbtnGainRatio;
        private System.Windows.Forms.Button btnBiIdentify;
        private System.Windows.Forms.Button btnIdentify;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.DomainUpDown dUDBestFields;
        private System.Windows.Forms.Label lblBest;
        private System.Windows.Forms.RadioButton rbtnGINI;
        private System.Windows.Forms.RadioButton rbtnEntropy;
        private System.Windows.Forms.DataGridView dgvFeatures;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColType;
        private System.Windows.Forms.DataGridView dgvResult;
    }
}