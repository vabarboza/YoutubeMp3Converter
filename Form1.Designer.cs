namespace YoutubeMp3Converter
{
    partial class mainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbDownload = new ProgressBar();
            labelStatus = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            tbLink = new TextBox();
            btnDownload = new Button();
            label2 = new Label();
            tbLocation = new TextBox();
            btnLocation = new Button();
            listBoxLinks = new ListBox();
            btnAddVideo = new Button();
            btnClean = new Button();
            cbSaveVideo = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pbDownload
            // 
            pbDownload.Location = new Point(12, 526);
            pbDownload.Name = "pbDownload";
            pbDownload.Size = new Size(540, 23);
            pbDownload.TabIndex = 0;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelStatus.Location = new Point(12, 508);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(109, 15);
            labelStatus.TabIndex = 1;
            labelStatus.Text = "Aguardando Video";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Image = Properties.Resources.image;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(564, 110);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 113);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 3;
            label1.Text = "Link do Video";
            // 
            // tbLink
            // 
            tbLink.Location = new Point(12, 131);
            tbLink.Name = "tbLink";
            tbLink.Size = new Size(540, 23);
            tbLink.TabIndex = 4;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(12, 369);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(125, 23);
            btnDownload.TabIndex = 5;
            btnDownload.Text = "Baixar e Converter";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 431);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 6;
            label2.Text = "Local de Saida";
            // 
            // tbLocation
            // 
            tbLocation.Location = new Point(12, 449);
            tbLocation.Name = "tbLocation";
            tbLocation.Size = new Size(459, 23);
            tbLocation.TabIndex = 7;
            // 
            // btnLocation
            // 
            btnLocation.Location = new Point(477, 448);
            btnLocation.Name = "btnLocation";
            btnLocation.Size = new Size(75, 24);
            btnLocation.TabIndex = 8;
            btnLocation.Text = "Selecionar";
            btnLocation.UseVisualStyleBackColor = true;
            btnLocation.Click += btnLocation_Click;
            // 
            // listBoxLinks
            // 
            listBoxLinks.FormattingEnabled = true;
            listBoxLinks.ItemHeight = 15;
            listBoxLinks.Location = new Point(12, 209);
            listBoxLinks.Name = "listBoxLinks";
            listBoxLinks.Size = new Size(540, 154);
            listBoxLinks.TabIndex = 9;
            // 
            // btnAddVideo
            // 
            btnAddVideo.Location = new Point(12, 160);
            btnAddVideo.Name = "btnAddVideo";
            btnAddVideo.Size = new Size(125, 23);
            btnAddVideo.TabIndex = 10;
            btnAddVideo.Text = "Adicionar Video";
            btnAddVideo.UseVisualStyleBackColor = true;
            btnAddVideo.Click += btnAddVideo_Click;
            // 
            // btnClean
            // 
            btnClean.Location = new Point(427, 369);
            btnClean.Name = "btnClean";
            btnClean.Size = new Size(125, 23);
            btnClean.TabIndex = 11;
            btnClean.Text = "Limpar Lista";
            btnClean.UseVisualStyleBackColor = true;
            btnClean.Click += btnClean_Click;
            // 
            // cbSaveVideo
            // 
            cbSaveVideo.AutoSize = true;
            cbSaveVideo.Location = new Point(143, 373);
            cbSaveVideo.Name = "cbSaveVideo";
            cbSaveVideo.Size = new Size(95, 19);
            cbSaveVideo.TabIndex = 12;
            cbSaveVideo.Text = "Salvar Videos";
            cbSaveVideo.UseVisualStyleBackColor = true;
            // 
            // mainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 561);
            Controls.Add(cbSaveVideo);
            Controls.Add(btnClean);
            Controls.Add(btnAddVideo);
            Controls.Add(listBoxLinks);
            Controls.Add(btnLocation);
            Controls.Add(tbLocation);
            Controls.Add(label2);
            Controls.Add(btnDownload);
            Controls.Add(tbLink);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(labelStatus);
            Controls.Add(pbDownload);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "mainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Youtube Audio Downloader";
            Load += mainForm_Load;
            FormClosing += mainForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar pbDownload;
        private Label labelStatus;
        private PictureBox pictureBox1;
        private Label label1;
        private TextBox tbLink;
        private Button btnDownload;
        private Label label2;
        private TextBox tbLocation;
        private Button btnLocation;
        private ListBox listBoxLinks;
        private Button btnAddVideo;
        private Button btnClean;
        private CheckBox cbSaveVideo;
    }
}
