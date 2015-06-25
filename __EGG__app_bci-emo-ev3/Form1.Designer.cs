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
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.souborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upravitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zobrazeníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nastaveníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emoComposerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canvas = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMonobrick)).BeginInit();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Font = new System.Drawing.Font("Lucida Console", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLog.Location = new System.Drawing.Point(13, 30);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(47, 10);
            this.labelLog.TabIndex = 0;
            this.labelLog.Text = "Console";
            // 
            // pictureBoxEmo
            // 
            this.pictureBoxEmo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxEmo.Image")));
            this.pictureBoxEmo.Location = new System.Drawing.Point(908, 59);
            this.pictureBoxEmo.Name = "pictureBoxEmo";
            this.pictureBoxEmo.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxEmo.TabIndex = 1;
            this.pictureBoxEmo.TabStop = false;
            this.pictureBoxEmo.Click += new System.EventHandler(this.pictureBoxEmo_Click);
            // 
            // pictureBoxMonobrick
            // 
            this.pictureBoxMonobrick.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMonobrick.Image")));
            this.pictureBoxMonobrick.Location = new System.Drawing.Point(908, 137);
            this.pictureBoxMonobrick.Name = "pictureBoxMonobrick";
            this.pictureBoxMonobrick.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxMonobrick.TabIndex = 2;
            this.pictureBoxMonobrick.TabStop = false;
            this.pictureBoxMonobrick.Click += new System.EventHandler(this.pictureBoxMonobrick_Click);
            // 
            // labelEmoStatus
            // 
            this.labelEmoStatus.AutoSize = true;
            this.labelEmoStatus.Location = new System.Drawing.Point(905, 43);
            this.labelEmoStatus.Name = "labelEmoStatus";
            this.labelEmoStatus.Size = new System.Drawing.Size(53, 13);
            this.labelEmoStatus.TabIndex = 3;
            this.labelEmoStatus.Text = "Odpojeno";
            // 
            // labelMonobrickStatus
            // 
            this.labelMonobrickStatus.AutoSize = true;
            this.labelMonobrickStatus.Location = new System.Drawing.Point(905, 121);
            this.labelMonobrickStatus.Name = "labelMonobrickStatus";
            this.labelMonobrickStatus.Size = new System.Drawing.Size(53, 13);
            this.labelMonobrickStatus.TabIndex = 4;
            this.labelMonobrickStatus.Text = "Odpojeno";
            // 
            // timerMain
            // 
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.souborToolStripMenuItem,
            this.upravitToolStripMenuItem,
            this.zobrazeníToolStripMenuItem,
            this.nastaveníToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1008, 24);
            this.menuMain.TabIndex = 6;
            this.menuMain.Text = "menuStrip1";
            // 
            // souborToolStripMenuItem
            // 
            this.souborToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konecToolStripMenuItem});
            this.souborToolStripMenuItem.Name = "souborToolStripMenuItem";
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.souborToolStripMenuItem.Text = "Soubor";
            // 
            // upravitToolStripMenuItem
            // 
            this.upravitToolStripMenuItem.Name = "upravitToolStripMenuItem";
            this.upravitToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.upravitToolStripMenuItem.Text = "Upravit";
            // 
            // zobrazeníToolStripMenuItem
            // 
            this.zobrazeníToolStripMenuItem.Name = "zobrazeníToolStripMenuItem";
            this.zobrazeníToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.zobrazeníToolStripMenuItem.Text = "Zobrazení";
            // 
            // nastaveníToolStripMenuItem
            // 
            this.nastaveníToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emoComposerToolStripMenuItem});
            this.nastaveníToolStripMenuItem.Name = "nastaveníToolStripMenuItem";
            this.nastaveníToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.nastaveníToolStripMenuItem.Text = "Nástroje";
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.konecToolStripMenuItem.Text = "Konec";
            this.konecToolStripMenuItem.Click += new System.EventHandler(this.konecToolStripMenuItem_Click);
            // 
            // emoComposerToolStripMenuItem
            // 
            this.emoComposerToolStripMenuItem.CheckOnClick = true;
            this.emoComposerToolStripMenuItem.Name = "emoComposerToolStripMenuItem";
            this.emoComposerToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.emoComposerToolStripMenuItem.Text = "EmoComposer";
            this.emoComposerToolStripMenuItem.CheckedChanged += new System.EventHandler(this.emoComposerToolStripMenuItem_CheckedChanged);
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.Black;
            this.canvas.Location = new System.Drawing.Point(15, 43);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(884, 674);
            this.canvas.TabIndex = 7;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.labelMonobrickStatus);
            this.Controls.Add(this.labelEmoStatus);
            this.Controls.Add(this.pictureBoxMonobrick);
            this.Controls.Add(this.pictureBoxEmo);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.menuMain);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuMain;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "BCI-gyroroboblink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMonobrick)).EndInit();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
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
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem souborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upravitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zobrazeníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nastaveníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emoComposerToolStripMenuItem;
        public System.Windows.Forms.Panel canvas;
    }
}


