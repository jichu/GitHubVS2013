using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emotiv;

namespace __EGG__app_bci_emo_ev3
{
    public partial class Form1 : Form
    {
        private Brain B;
        private Computer C;
        private Interface I;
        private DrawEngine drawEngine;
        private Thread threadB;
        public enum qualitySignal { NONE, LOW, MEDIUM, HIGH };
        public string[] channelNames = new string[18] { "CMS", "DRL", "FP1", "AF3", "F7", "F3", "FC5", "T7", "P7", "O1", "O2", "P8", "T8", "FC6", "F4", "F8", "AF4", "FP2" }; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
            drawEngine = new DrawEngine();

            initBrain();
            //initComputer();
            initInterface();
            timerMain.Start();
            //drawEngine.setQualitySignalData(B.getContenctQuality());
        }

        private void initBrain()
        {
            B = new Brain();
            threadB = new Thread(new ThreadStart(B.Run));
            threadB.IsBackground = true;
            threadB.Start();
            labelLog.Text += B.log;
            if (B.connected)
            {
                labelEmoStatus.Text = "Připojeno";
            }
            else
            {
                labelEmoStatus.Text = "Odpojeno";
            }
            System.Threading.Thread.Sleep(B.tick + 10);
        }

        private void initComputer()
        {
            C = new Computer();
            if (C.connected)
            {
                Thread c = new Thread(new ThreadStart(C.Run));
                c.IsBackground = true;
                c.Start();
            }
            labelLog.Text += C.log;
        }

        private void initInterface()
        {
            I = new Interface(B);
            I.initDrawEngine();
            /* robot
            if (B.connected && C.connected)
            {
                I = new Interface(B, C);
                Thread i = new Thread(new ThreadStart(I.Run));
                i.IsBackground = true;
                i.Start();
            }
             * */
        }

        private void pictureBoxEmo_Click(object sender, EventArgs e)
        {
            labelEmoStatus.Text = "Připojuji...";
            initBrain();
        }

        private void pictureBoxMonobrick_Click(object sender, EventArgs e)
        {
            labelMonobrickStatus.Text = "Připojuji...";
            initComputer();
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            labelLog.Text = "";
            /*
            for (int i = 0; i < 18; i++)
            {
                string key = B.getChannelNames()[i];
                labelLog.Text += key + ": "+ B.getContactQualityDictionary()[key] + Environment.NewLine;
            }
            */
            labelLog.Text += B.GetUserID().ToString()+Environment.NewLine;
            foreach (KeyValuePair<EdkDll.EE_DataChannel_t,double[]> item in B.getEEGData())
            {
                    labelLog.Text += item.Key + " " + item.Value[1] + Environment.NewLine;
            }


        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = canvas.CreateGraphics();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadB.Abort();
            I.stop();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        static extern bool AllocConsole();


    }
}