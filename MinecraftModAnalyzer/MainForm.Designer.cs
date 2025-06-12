namespace MinecraftModAnalyzer
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ListBox lstJarFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblModCount;
        private System.Windows.Forms.Label lblJarCount;
        private System.Windows.Forms.ListBox lstAuthors;
        private System.Windows.Forms.ListBox lstDependencies;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMCVersions;
        private System.Windows.Forms.Label label4;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.lstJarFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblModCount = new System.Windows.Forms.Label();
            this.lblJarCount = new System.Windows.Forms.Label();
            this.lstAuthors = new System.Windows.Forms.ListBox();
            this.lstDependencies = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMCVersions = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView).BeginInit();
            this.SuspendLayout();

            // dataGridView
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(860, 250);
            this.dataGridView.TabIndex = 0;

            // lstJarFiles
            this.lstJarFiles.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.lstJarFiles.FormattingEnabled = true;
            this.lstJarFiles.Location = new System.Drawing.Point(12, 328);
            this.lstJarFiles.Name = "lstJarFiles";
            this.lstJarFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstJarFiles.Size = new System.Drawing.Size(860, 95);
            this.lstJarFiles.TabIndex = 1;

            // label1
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "待分析的JAR:";

            // btnAnalyze
            this.btnAnalyze.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnAnalyze.Location = new System.Drawing.Point(12, 429);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(100, 30);
            this.btnAnalyze.TabIndex = 3;
            this.btnAnalyze.Text = "开始分析";
            this.btnAnalyze.UseVisualStyleBackColor = true;

            // btnClear
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.Location = new System.Drawing.Point(118, 429);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清除所有";
            this.btnClear.UseVisualStyleBackColor = true;

            // btnExport
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnExport.Location = new System.Drawing.Point(772, 429);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 30);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "导出CSV";
            this.btnExport.UseVisualStyleBackColor = true;

            // progressBar
            this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.progressBar.Location = new System.Drawing.Point(12, 465);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(860, 23);
            this.progressBar.TabIndex = 6;

            // lblModCount
            this.lblModCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.lblModCount.AutoSize = true;
            this.lblModCount.Location = new System.Drawing.Point(12, 270);
            this.lblModCount.Name = "lblModCount";
            this.lblModCount.Size = new System.Drawing.Size(91, 13);
            this.lblModCount.TabIndex = 7;
            this.lblModCount.Text = "找到MOD数量: 0";

            // lblJarCount
            this.lblJarCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.lblJarCount.AutoSize = true;
            this.lblJarCount.Location = new System.Drawing.Point(130, 270);
            this.lblJarCount.Name = "lblJarCount";
            this.lblJarCount.Size = new System.Drawing.Size(91, 13);
            this.lblJarCount.TabIndex = 8;
            this.lblJarCount.Text = "处理JAR数量: 0";

            // lstAuthors
            this.lstAuthors.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lstAuthors.FormattingEnabled = true;
            this.lstAuthors.Location = new System.Drawing.Point(878, 32);
            this.lstAuthors.Name = "lstAuthors";
            this.lstAuthors.Size = new System.Drawing.Size(200, 108);
            this.lstAuthors.TabIndex = 9;

            // lstDependencies
            this.lstDependencies.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lstDependencies.FormattingEnabled = true;
            this.lstDependencies.Location = new System.Drawing.Point(878, 167);
            this.lstDependencies.Name = "lstDependencies";
            this.lstDependencies.Size = new System.Drawing.Size(200, 108);
            this.lstDependencies.TabIndex = 10;

            // label2
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(875, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "作者统计";

            // label3
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(875, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "依赖统计";

            // lblMCVersions
            this.lblMCVersions.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.lblMCVersions.AutoSize = true;
            this.lblMCVersions.Location = new System.Drawing.Point(260, 270);
            this.lblMCVersions.Name = "lblMCVersions";
            this.lblMCVersions.Size = new System.Drawing.Size(55, 13);
            this.lblMCVersions.TabIndex = 13;
            this.lblMCVersions.Text = "MC版本: ";

            // label4
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(295, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "提示: 可直接拖放JAR文件到窗口或使用下方列表管理文件";

            // MainForm
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 500);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblMCVersions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstDependencies);
            this.Controls.Add(this.lstAuthors);
            this.Controls.Add(this.lblJarCount);
            this.Controls.Add(this.lblModCount);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstJarFiles);
            this.Controls.Add(this.dataGridView);
            this.Name = "MainForm";
            this.Text = "JAR Mod 分析工具";
            ((System.ComponentModel.ISupportInitialize)this.dataGridView).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}