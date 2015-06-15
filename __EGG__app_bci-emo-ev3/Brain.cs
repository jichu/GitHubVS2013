using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Emotiv;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace __EGG__app_bci_emo_ev3
{    
    public class Brain : Form1
    {
        EmoEngine engine; // Access to the EDK is viaa the EmoEngine 
        public Boolean isBlink;
        public Boolean isLeft;
        public Boolean isRight;
        public Boolean isCalm;
        private Stopwatch stopWatch;
        public double angle;
        int oldX = 0;
        int userID = -1;
        public Boolean connected = false;
        public Boolean stopped = false;
        public int tick = 100;
        private Boolean gyroWait = true;
        public string log = "";
        private EdkDll.EE_EEG_ContactQuality_t[] contactQualityEmotiv = new EdkDll.EE_EEG_ContactQuality_t[18];
        private Dictionary<string, string> contactQualityDictionary = new Dictionary<string, string>();

        public Brain()
        {
            stopWatch = new Stopwatch();
            engine = EmoEngine.Instance;
            engine.EmoEngineConnected += (EmoEngineConnected);
            engine.EmoEngineDisconnected += (EmoEngineDisconnected);
            engine.EmoStateUpdated += (EmoStateUpdated);
            engine.UserAdded += (UserAdded);
            try
            {
                //engine.Connect();
                engine.RemoteConnect("127.0.0.1", 1726);
                for (int i = 0; i < 18; i++)
                {
                    contactQualityDictionary.Add(channelNames[i], "0");
                }
            }
            catch (EmoEngineException ex)
            {
                Console.WriteLine("errrrrr");
                log += "Fail in EmoEngine. Message: "+ex.Message;
                log += "\nError stack: "+ex.StackTrace;
                connected = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("xxxxx {0}", e.ToString());
            }
            //engine.Disconnect();
        }
        private void EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;
            if (es.GetHeadsetOn() == 1)
            {
                connected = true;
                contactQualityEmotiv = es.GetContactQualityFromAllChannels();
                isBlink = es.ExpressivIsEyesOpen();
                for (int i = 0; i < 18; i++) { 
                    contactQualityDictionary[channelNames[i]]=contactQualityEmotiv[i].ToString();
                } 
                
            }
            else
            {
                connected = false;
            }
        }
        public void Run()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(tick);
                try
                {
                    engine.ProcessEvents(100);
                    if (gyroWait)
                    {
                        System.Threading.Thread.Sleep(550);
                        gyroWait = false;
                    }
                    gyro();
                    

                }
                catch (EmoEngineException ex)
                {
                    Console.WriteLine("error run");
                    log += "Fail in EmoEngine. Message: " + ex.Message;
                    log += "\nError stack: " + ex.StackTrace;
                    connected = false;
                }
            }
        }

        private void gyro()
        {
            int x = 0, y = 0, t = 50;
            engine.HeadsetGetGyroDelta((uint)userID, out x, out y);
            double T = stopWatch.ElapsedMilliseconds/1000.0;
            angle = (double)(x) * T;
            Console.WriteLine("Time s: {0}\n", (double)T);
            Console.WriteLine("a {0}\n", angle);
            if (oldX != x)
            {
                stopWatch.Start();
                if (x < (sbyte)-t)
                {
                    isLeft = true;
                    isRight = false;
                    isCalm = false;
                }
                if (x > t)
                {
                    isLeft = false;
                    isRight = true;
                    isCalm = false;
                }
            }
            else
            {
                stopWatch.Stop();
            }
            if (x > (sbyte)-t && x < t || (!isLeft && !isRight))
            {
                stopWatch.Reset();
                isCalm = true;
                isLeft = false;
                isRight = false;
            }
            x = oldX;
            //Console.Clear();
            /*
            if (isLeft)
            {
                Console.WriteLine("leva");
            }
            if (isRight)
            {
                Console.WriteLine("prava");
            }
            if (isCalm)
            {
                Console.WriteLine("klid");
            }
            if (isBlink)
            {
                Console.WriteLine("");
            }
            if (!isBlink)
            {
                Console.WriteLine("mrk");
            }
             * */
        }

        private static void EmoEngineConnected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("connected {0}",e);
        }

        private static void EmoEngineDisconnected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("disconnected");
        }

        void UserAdded(object sender, EmoEngineEventArgs e)
        {
            // record the user 
            userID = (int)e.userId;

            // enable data aquisition for this user.
            engine.DataAcquisitionEnable((uint)userID, true);            

            Console.WriteLine("User Added Event has occured {0}", userID);

        }
//Načítání kvality signálu z jednotlivých elektrod
        public EdkDll.EE_EEG_ContactQuality_t[] getContactQuality()
        {

            return (contactQualityEmotiv);

        }

        public string[] getChannelNames() {
            return (channelNames);
        }

        public Dictionary<string, string> getContactQualityDictionary() {
            return (contactQualityDictionary);
        }

    };
}
