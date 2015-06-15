using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace __EGG__app_bci_emo_ev3
{
    public class Interface
    {
        Brain B;
        Computer C;
        int counter = 0;
        Boolean wasCloseEye;
        Boolean moving = false;
        private DrawEngine drawEngine;
        public Interface(Brain brain, Computer comp)
        {
            B = brain;
            C = comp;
        }
        public Interface(Brain brain)
        {
            B = brain;
        }
        public void initDrawEngine()
        {
            drawEngine = new DrawEngine();
            Thread i = new Thread(new ThreadStart(Run));
            i.IsBackground = true;
            i.Start();
            drawEngine.init();
        }
        public void Run()
        {
            while (true)
            {
                Console.WriteLine(0);
                System.Threading.Thread.Sleep(20);
                if (B.connected)
                {
                    drawEngine.connect = true;
                }
                else {
                    drawEngine.connect = false;
                }
                // TO DO
                drawEngine.setQualitySignalData(B.getContactQualityDictionary());
                


                /*
                if (B.isCalm)
                {
                    C.motorOff();
                }
                if (B.isRight)
                {
                    C.moveRight();
                }
                if (B.isLeft)
                {
                    C.moveLeft();
                }

                if (!B.isBlink)
                {
                    wasCloseEye = true;
                }

                if (B.isBlink && wasCloseEye)
                {
                    wasCloseEye = false;
                    moving = !moving;
                    if (moving)
                    {
                        C.moveForward();
                        counter++;
                        Console.Clear();
                        Console.WriteLine(counter);
                    }
                    else
                    {
                        C.motorStop();
                        counter++;
                        Console.Clear();
                        Console.WriteLine(counter);
                    }
                }
                /*
                ConsoleKeyInfo cki;
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        C.moveForward();
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        C.moveBackward();
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        C.moveLeft();
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        C.moveRight();
                        break;
                    case ConsoleKey.Spacebar:
                        C.motorStop();
                        break;
                }*/
            }
        }
        public void stop()
        {
            drawEngine.stop();
        }

    };
}

