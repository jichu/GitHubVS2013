namespace __EGG__app_bci_emo_ev3
{
    partial class Form1
    {
        /// 

        /// Required designer variable.
        /// 

        private System.ComponentModel.IContainer components = null;

        /// 

        /// Clean up any resources being used.
        /// 

        /// true if managed resources should be disposed; otherwise, false.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// 

        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// 

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelLog = new System.Windows.Forms.Label();
            this.pictureBoxEmo = new System.Windows.Forms.PictureBox();
            this.pictureBoxMonobrick = new System.Windows.Forms.PictureBox();
            this.labelEmoStatus = new System.Windows.Forms.Label();
            this.labelMonobrickStatus = new System.Windows.Forms.Label();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMonobrick)).BeginInit();
            this.SuspendLayout();
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Font = new System.Drawing.Font("Lucida Console", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLog.Location = new System.Drawing.Point(13, 13);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(47, 10);
            this.labelLog.TabIndex = 0;
            this.labelLog.Text = "Console";
            // 
            // pictureBoxEmo
            // 
            this.pictureBoxEmo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxEmo.Image")));
            this.pictureBoxEmo.Location = new System.Drawing.Point(703, 42);
            this.pictureBoxEmo.Name = "pictureBoxEmo";
            this.pictureBoxEmo.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxEmo.TabIndex = 1;
            this.pictureBoxEmo.TabStop = false;
            this.pictureBoxEmo.Click += new System.EventHandler(this.pictureBoxEmo_Click);
            // 
            // pictureBoxMonobrick
            // 
            this.pictureBoxMonobrick.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMonobrick.Image")));
            this.pictureBoxMonobrick.Location = new System.Drawing.Point(703, 135);
            this.pictureBoxMonobrick.Name = "pictureBoxMonobrick";
            this.pictureBoxMonobrick.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxMonobrick.TabIndex = 2;
            this.pictureBoxMonobrick.TabStop = false;
            this.pictureBoxMonobrick.Click += new System.EventHandler(this.pictureBoxMonobrick_Click);
            // 
            // labelEmoStatus
            // 
            this.labelEmoStatus.AutoSize = true;
            this.labelEmoStatus.Location = new System.Drawing.Point(700, 26);
            this.labelEmoStatus.Name = "labelEmoStatus";
            this.labelEmoStatus.Size = new System.Drawing.Size(53, 13);
            this.labelEmoStatus.TabIndex = 3;
            this.labelEmoStatus.Text = "Odpojeno";
            // 
            // labelMonobrickStatus
            // 
            this.labelMonobrickStatus.AutoSize = true;
            this.labelMonobrickStatus.Location = new System.Drawing.Point(700, 119);
            this.labelMonobrickStatus.Name = "labelMonobrickStatus";
            this.labelMonobrickStatus.Size = new System.Drawing.Size(53, 13);
            this.labelMonobrickStatus.TabIndex = 4;
            this.labelMonobrickStatus.Text = "Odpojeno";
            // 
            // timerMain
            // 
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 245);
            this.Controls.Add(this.labelMonobrickStatus);
            this.Controls.Add(this.labelEmoStatus);
            this.Controls.Add(this.pictureBoxMonobrick);
            this.Controls.Add(this.pictureBoxEmo);
            this.Controls.Add(this.labelLog);
            this.Name = "Form1";
            this.Text = "BCI-gyroroboblink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMonobrick)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxEmo;
        private System.Windows.Forms.PictureBox pictureBoxMonobrick;
        public System.Windows.Forms.Label labelEmoStatus;
        public System.Windows.Forms.Label labelMonobrickStatus;
        public System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Timer timerMain;
    }
}


