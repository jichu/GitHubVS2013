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
using System.IO;

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
        public int tick = 1000;
        private Boolean gyroWait = true;
        public string log = "";
        private EdkDll.EE_EEG_ContactQuality_t[] contactQualityEmotiv = new EdkDll.EE_EEG_ContactQuality_t[18];
        private Dictionary<string, string> contactQualityDictionary = new Dictionary<string, string>();
        private Dictionary<EdkDll.EE_DataChannel_t, double[]> dataEEG = new Dictionary<EdkDll.EE_DataChannel_t, double[]>();
        private Dictionary<EdkDll.EE_DataChannel_t, double> firstValues = new Dictionary<EdkDll.EE_DataChannel_t, double>();
        private Boolean firstRun = true;

        public Brain()
        {
            stopWatch = new Stopwatch();
            engine = EmoEngine.Instance;
            engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(EmoEngineConnected);
            engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(EmoEngineDisconnected);
            engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(EmoStateUpdated);
            engine.UserAdded += new EmoEngine.UserAddedEventHandler(UserAdded);
           
            for (int i = 0; i < 18; i++)
            {
                contactQualityDictionary.Add(channelNames[i], "0");
            }
            try
            {
                Connect();
                
                connected = true;
            }
            catch (EmoEngineException ex)
            {
                connected = false;
                //Console.WriteLine("errrrrr {0}", ex.Message);
                Console.WriteLine("{0}", ex.ErrorCode);
                log += "Fail in EmoEngine. Message: " + ex.Message;
                log += "\nError stack: " + ex.StackTrace;

            }
            catch (Exception e)
            {
                Console.WriteLine("xxxxx {0}", e.ToString());
            }

        }
        private void EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;         
            //if (!es.GetWirelessSignalStatus().Equals(EdkDll.EE_SignalStrength_t.NO_SIGNAL))
            //{
                contactQualityEmotiv = es.GetContactQualityFromAllChannels();
                isBlink = es.ExpressivIsEyesOpen();
                for (int i = 0; i < 18; i++) { 
                    contactQualityDictionary[channelNames[i]]=contactQualityEmotiv[i].ToString();
                }

               Console.WriteLine(es.GetHeadsetOn());
            //}
            /*else
            {
                connected = false;
                //Console.WriteLine(es.GetWirelessSignalStatus());

            }*/
        }
        public void Run()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(tick);
                try
                {
                    if (!connected)
                    {
                        Connect();

                    }
                    connected = true;
                    
                    engine.ProcessEvents(100);
                    /*if (gyroWait)
                    {
                        System.Threading.Thread.Sleep(550);
                        gyroWait = false;
                    }*/
                    Console.WriteLine("{0}, {1}",connected, userID);
                    if (!userID.Equals(-1))
                    {
                        
                        dataEEG = engine.GetData((uint)userID);
                        
                        if (dataEEG == null) {
                            return;
                        }
                        if (firstRun){

                            foreach (KeyValuePair<EdkDll.EE_DataChannel_t, double[]> item in dataEEG){
                             firstValues.Add(item.Key, item.Value[0]);
                            }
                            firstRun = false;
                        }
                       /*int _bufferSize = dataEEG[EdkDll.EE_DataChannel_t.TIMESTAMP].Length;
                        string filename = @"C:\Programy\data.txt";
                        // Write the data to a file
                        TextWriter file = new StreamWriter(filename, true);

                        for (int i = 0; i < _bufferSize; i++)
                        {
                            // now write the data
                            foreach (EdkDll.EE_DataChannel_t channel in dataEEG.Keys)
                                file.Write(dataEEG[channel][i] + ",");
                            file.WriteLine("");

                        }
                        file.Close();*/
                    }


                    //gyro();
                    //dataEEG = engine.GetData((uint)userID);

                }
                catch (EmoEngineException ex)
                {

                    //Data are not available
                    if (ex.ErrorCode.Equals(1024)) {
                        Console.WriteLine("EmoComposer only. No EEG data access.");
                    }

                    //Cannot connect to device
                    if (ex.ErrorCode.Equals(1282)) { 
                      //TODO
                    }
                    

                    Console.WriteLine("{0}, {1}",ex.ErrorCode, ex.Message);
                    log += "Fail in EmoEngine. Message: " + ex.Message;
                    log += "\nError stack: " + ex.StackTrace;
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

        private void Connect() {
                //engine.Connect();
                engine.RemoteConnect("127.0.0.1", 1726);
            
        }

        private void EmoEngineConnected(object sender, EmoEngineEventArgs e)
        {
            //Console.WriteLine("connected {0}",e);
            //connected = true;
        }

        private void EmoEngineDisconnected(object sender, EmoEngineEventArgs e)
        {
            //Console.WriteLine("disconnected");
            //connected = false;
        }

        void UserAdded(object sender, EmoEngineEventArgs e)
        {
            // record the user 
            userID = (int)e.userId;

            // enable data aquisition for this user.
            engine.DataAcquisitionEnable((uint)userID, true);

            // ask for up to 1 second of buffered data
            engine.EE_DataSetBufferSizeInSec(1); 

            Console.WriteLine("User Added Event has occured. User ID: {0}, connected: {1} ", userID,connected);

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

        public Dictionary<EdkDll.EE_DataChannel_t, double[]> getEEGData() {

            if (dataEEG != null)
            {
                int TC = 256;
                int rows;
                double back;
                Dictionary<EdkDll.EE_DataChannel_t, double[]> ACdataEEG = new Dictionary<EdkDll.EE_DataChannel_t, double[]>();

                dataEEG.Remove(EdkDll.EE_DataChannel_t.COUNTER);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.ES_TIMESTAMP);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.FUNC_ID);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.FUNC_VALUE);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.GYROX);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.GYROY);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.INTERPOLATED);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.MARKER);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.RAW_CQ);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.SYNC_SIGNAL);
                dataEEG.Remove(EdkDll.EE_DataChannel_t.TIMESTAMP);


                foreach (KeyValuePair<EdkDll.EE_DataChannel_t, double[]> item in dataEEG)
                {

                    back = firstValues[item.Key];
                    rows = item.Value.Length;
                    double[] editedData = new double[rows];
                    editedData[0] = item.Value[0] - ((back * (TC - 1) + item.Value[0]) / TC);
                    for (int i = 1; i < rows; i++)
                    {
                        back = (back * (TC - 1) + item.Value[i]) / TC;
                        editedData[i] = item.Value[i] - back;
                    }
                    ACdataEEG.Add(item.Key, editedData);
                }

                /*int _bufferSize = ACdataEEG[EdkDll.EE_DataChannel_t.AF3].Length;
                string filename = @"C:\Programy\data.csv";
                // Write the data to a file
                TextWriter file = new StreamWriter(filename, true);

                for (int i = 0; i < _bufferSize; i++)
                {
                    // now write the data
                    foreach (EdkDll.EE_DataChannel_t channel in ACdataEEG.Keys)
                        file.Write(ACdataEEG[channel][i] + ";");
                    file.WriteLine("");

                }
                file.Close();*/

                return (ACdataEEG);
            }
            else {
                return null;
            }
        }

        public int GetUserID() {
            return (userID);
        }
        
    };
}
