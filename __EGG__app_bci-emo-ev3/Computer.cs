using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoBrick.EV3;
using System.Runtime.InteropServices;
using System.Threading;

namespace __EGG__app_bci_emo_ev3
{
    public class Computer : Form1
    {
        Brick<TouchSensor, ColorSensor, Sensor, IRSensor> brick;
        static int sleep = 100;
        sbyte speed = 20;
        public bool active = false;
        public Boolean connected = false;
        public string log;
        public Computer()
        {
            log = "Brick message:/n";
            try
            {
                //var brick = new Brick<TouchSensor, ColorSensor, IRSensor, Sensor>("usb"); 
                //brick = new Brick<TouchSensor, ColorSensor, Sensor, IRSensor>("usb"); //Pro připojení přes USB
                brick = new Brick<TouchSensor, ColorSensor, Sensor, IRSensor>("com3"); //Pro připojení přes Bluetooth
                brick.Connection.Open();
            }
            catch (Exception e)
            {
                log = e.ToString();
                connected = false;
            }

        }
        private void WaitForMotor()
        {
            Thread.Sleep(sleep);
            while (brick.MotorB.IsRunning()) { Thread.Sleep(sleep / 2); }
        }
        public void motorOn()
        {
            if (!active)
            {
                speed = 100;
                active = true;
                Console.WriteLine("Motor rychlost " + speed);
                brick.MotorC.On(speed);
                System.Threading.Thread.Sleep(sleep);
            }
        }
        public void motorOff()
        {
            if (active)
            {
                active = false;
                speed = 0;
                Console.WriteLine("Motor rychlost " + speed);
                brick.MotorB.Off();
                System.Threading.Thread.Sleep(sleep);
                brick.MotorC.Off();
            }
        }
        public void moveLeft()
        {
            brick.MotorB.On((sbyte)-speed);
            System.Threading.Thread.Sleep(sleep);
            brick.MotorC.On(speed);
            System.Threading.Thread.Sleep(4 * sleep);
            brick.MotorC.Brake();
            System.Threading.Thread.Sleep(sleep);
            brick.MotorB.Brake();

        }
        public void moveRight()
        {
            brick.MotorB.On(speed);
            System.Threading.Thread.Sleep(sleep);
            brick.MotorC.On((sbyte)-speed);
            System.Threading.Thread.Sleep(4 * sleep);
            brick.MotorC.Brake();
            System.Threading.Thread.Sleep(sleep);
            brick.MotorB.Brake();
        }
        public void moveForward()
        {
            brick.MotorB.On(speed);
            System.Threading.Thread.Sleep(sleep);
            brick.MotorC.On(speed);
        }
        public void moveBackward()
        {
            speed = -20;
            brick.MotorB.On(speed);
            System.Threading.Thread.Sleep(sleep);
            brick.MotorC.On(speed);
        }
        public void motorStop()
        {
            brick.MotorB.Brake();
            System.Threading.Thread.Sleep(sleep);
            brick.MotorC.Brake();
        }

        public void Run()
        {
            while (true)
            {

                System.Threading.Thread.Sleep(20);
            }
        }
    };
}
