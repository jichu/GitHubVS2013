using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
/*
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
 * */
using SharpDX.DirectWrite;
using SharpDX.Direct2D1;
using System.Windows.Forms;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Factory = SharpDX.Direct2D1.Factory;
using Point = SharpDX.Point;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using SharpDX.WIC;
using SharpDX.IO;

namespace __EGG__app_bci_emo_ev3
{
    class DrawEngine : Form1
    {
        private Thread renderThread;
        private Factory factory;
        private ImagingFactory imagingFactory;
        NativeFileStream fileStream;
        private WindowRenderTarget renderTarget;
        RenderLoop.RenderCallback callback;
        private RenderForm rf;
        private int sleep = 1000; // idle time
        private float[] data = new float[14];
        private Individual[] ind = new Individual[14];
        private Point[] point = new Point[14];
        private Dictionary<string, string> qualitySignalData = new Dictionary<string, string>();
        public bool connect = false;

        public DrawEngine()
        {
            try
            {
                factory = new Factory(SharpDX.Direct2D1.FactoryType.MultiThreaded);
                rf = new RenderForm("__proto__BCI");
                rf.Width = 1024;
                rf.Height = 768;
                RenderTargetProperties winProp = new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));

                //set hwnd target properties (permit to attach Direct2D to window)
                HwndRenderTargetProperties hwnd = new HwndRenderTargetProperties()
                {
                    Hwnd = rf.Handle,
                    PixelSize = new Size2(rf.Width, rf.Height),//canvas.ClientSize.Width, canvas.ClientSize.Height),
                    PresentOptions = PresentOptions.RetainContents
                };

                renderTarget = new WindowRenderTarget(factory, winProp, hwnd);
                renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error initializing Managed DirectX", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void init()
        {
            renderThread = new Thread(new ThreadStart(simulate));
            renderThread.IsBackground = true;
            renderThread.Start();
            rf.Show();
            initPoints();
            int offset = 250;
            int zoom = 40;
            int size = 50;
            for (int i = 0; i < 14; i++)
            {
                int x = point[i].X * zoom + offset;
                int y = point[i].Y * zoom + offset;
                ind[i] = new Individual(x, y, size, data[i]);
            }
            callback = new RenderLoop.RenderCallback(render);
            RenderLoop.Run(rf, callback);
        }
        public void stop()
        {
            rf.Close();
            //renderThread.Abort();
        }
        private void simulate()
        {
            while (renderThread.IsAlive)
            {
                Random noise = new Random();
                String tmp = "";
                for (int i = 0; i < 14; i++)
                {
                    data[i] = noise.NextFloat(0, 1);
                    tmp += data[i].ToString() + ", ";
                }
                Console.WriteLine(tmp);
                Thread.Sleep(sleep);
                Application.DoEvents();
            }
        }
        public void initPoints()
        {
            // I. kvadrant
            point[0].X = 1; point[0].Y = -3; // F4
            point[1].X = 2; point[1].Y = -4; // AF4
            point[2].X = 4; point[2].Y = -3; // F8
            point[3].X = 3; point[3].Y = -2; // FC6
            // IV. kvadrant
            point[4].X = 5; point[4].Y = 0; // T8
            point[5].X = 3; point[5].Y = 3; // P8
            point[6].X = 1; point[6].Y = 4; // O8
            // III. kvadrant
            point[7].X = -1; point[7].Y = 4; // O1
            point[8].X = -3; point[8].Y = 3; // P7
            point[9].X = -5; point[9].Y = 0; // T7
            // II. kvadrant
            point[10].X = -3; point[10].Y = -2; // FC5
            point[11].X = -4; point[11].Y = -3; // F7
            point[12].X = -2; point[12].Y = -4; // AF3
            point[13].X = -1; point[13].Y = -3; // F3
        }
        // hlavnĂ­ render smyÄŤka
        bool firstRender = true;
        private void render()
        {
            renderTarget.BeginDraw();
            if (firstRender)
            {
                renderTarget.Clear(Color4.White);
                //dPolygon();
                firstRender = false;
                
            }
            drawQualitySignal(15, rf.Width - 90, 80);
            drawConectivity();
            draw();
            renderTarget.Flush();
            renderTarget.EndDraw();
            Thread.Sleep(sleep);
        }

        /*
         * kvalita signĂˇlu
         */
        public void setQualitySignalData(Dictionary<string, string> quality)
        {
            qualitySignalData = quality;
        }
        // bod/kvaita signĂˇlu
        private void dPointSenzor(float x, float y, float radius, string quality)
        {
            Color4 color = new Color4(0, 0, 0, 1);
            switch (quality)
            {
                case "EEG_CQ_NO_SIGNAL":
                    color = new Color4(0, 0, 0, 1);
                    break;
                case "EEG_CQ_VERY_BAD":
                    color = new Color4(1, 0, 0, 1);
                    break;
                case "EEG_CQ_POOR":
                    color = new Color4(1, 0.5f, 0, 1);
                    break;
                case "EEG_CQ_FAIR":
                    color = new Color4(1, 1, 0, 1);
                    break;
                case "EEG_CQ_GOOD":
                    color = new Color4(0, 1, 0, 1);
                    break;
            }
            SolidColorBrush brush = new SolidColorBrush(renderTarget, color);
            renderTarget.FillEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
        }
        public void drawQualitySignal(int zoom, int offsetX, int offsetY)
        {
            for (int i = 0; i < 14; i++)
            {
                int x = point[i].X * zoom + offsetX;
                int y = point[i].Y * zoom + offsetY;
                dPointSenzor(x, y, 10, qualitySignalData[channelNames[i]]);
            }
        }


        /*
         * vykreslovĂˇnĂ­
         */
        // kruh/bod/objekt
        private void dPoint(float x, float y, float radius, float value)
        {
            float r = value / 2;
            float g = value / 2;
            float b = value;
            float a = 0.1f;
            //SolidColorBrush brush = new SolidColorBrush(renderTarget, new Color4(r,g,b,a));
            GradientStop[] gradientStop = new GradientStop[2];
            gradientStop[0] = new GradientStop() { Color = new Color4(r, g, b, a), Position = 0.0f };
            gradientStop[1] = new GradientStop() { Color = new Color4(r, g, b, 0), Position = 1.0f };
            RadialGradientBrush fill = radialGradient(x, y, radius, gradientStop);
            //renderTarget.DrawEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
            renderTarget.FillEllipse(new Ellipse(new Vector2(x, y), radius, radius), fill);
        }
        private RadialGradientBrush radialGradient(float x, float y, float radius, GradientStop[] gradientStop)
        {
            GradientStopCollection gradientStopCollection = new GradientStopCollection(renderTarget, gradientStop, ExtendMode.Clamp);
            RadialGradientBrushProperties radialGradientBrush = new RadialGradientBrushProperties()
            {
                RadiusX = radius,
                RadiusY = radius,
                Center = new Vector2(x, y),
                GradientOriginOffset = new Vector2(0, 0)
            };
            return new RadialGradientBrush(renderTarget, radialGradientBrush, gradientStopCollection);
        }
        private void draw()
        {
            for (int i = 0; i < 14; i++)
            {
                //ind[i].grow(data[i]);
                dPoint(ind[i].posX, ind[i].posY, ind[i].size, ind[i].value);
                ind[i].old++;
            }
        }


        /*
         * stav konektivity
         */
        // bod/signĂˇl on/off
        private void dPointConnect(float x, float y, float radius, bool status)
        {
            Color4 color = new Color4(1, 0, 0, 1);
            if (status)
            {
                color = new Color4(0, 1, 0, 1);
            }
            SolidColorBrush brush = new SolidColorBrush(renderTarget, color);
            renderTarget.FillEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
            //LoadFromFile(renderTarget, "c:\\workspace\\csharp\\__EGG__app_bci-emo-ev3\\__EGG__app_bci-emo-ev3\\bin\\Debug\\epoc.png");
        }
        private void drawConectivity()
        {
            int size = 20;
            int padding = 10;
            dPointConnect(rf.Width - size - padding, rf.Height - size - padding, size, connect);
        }

       /* public static Bitmap LoadFromFile(RenderTarget renderTarget, string file)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;
                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }*/

        /// <summary>
        /// ///////////////////////////////
        /// </summary>
        private void dPolygon()
        {
            Vector2[] vector = new Vector2[14];
            GeometrySink geometrySink;
            PathGeometry pathGeometry;
            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();
            geometrySink.BeginFigure(new Vector2(ind[0].posX, ind[0].posY), new FigureBegin());
            /*
            for (int i = 1; i < 14; i++)
                geometrySink.AddLine(new Vector2(ind[i].posX, ind[i].posY));
            geometrySink.AddLine(new Vector2(ind[0].posX, ind[0].posY));
             * */
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = new Vector2(ind[0].posX, ind[0].posY),
                Point2 = new Vector2(ind[1].posX, ind[1].posY),
                Point3 = new Vector2(ind[2].posX, ind[2].posY)
            });
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = new Vector2(ind[2].posX, ind[2].posY),
                Point2 = new Vector2(ind[3].posX, ind[3].posY),
                Point3 = new Vector2(ind[4].posX, ind[4].posY)
            });
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = new Vector2(ind[4].posX, ind[4].posY),
                Point2 = new Vector2(ind[5].posX, ind[5].posY),
                Point3 = new Vector2(ind[6].posX, ind[6].posY)
            });
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();
            SolidColorBrush pen = new SolidColorBrush(renderTarget, new Color4(0, 0, 1, 0.1f));
            GradientStop[] gradientStop = new GradientStop[2];
            float value = 1;
            float r = value / 2;
            float g = value / 2;
            float b = value;
            float a = 0.2f;
            gradientStop[0] = new GradientStop() { Color = new Color4(r, g, b, a), Position = 0.0f };
            gradientStop[1] = new GradientStop() { Color = new Color4(r, g, b, 0), Position = 1.0f };
            RadialGradientBrush fill = radialGradient(250, 250, 500, gradientStop);
            renderTarget.DrawGeometry(pathGeometry, pen);
            renderTarget.FillGeometry(pathGeometry, fill);
            pathGeometry.Dispose();
            geometrySink.Dispose();
        }

    };



    /*
     * jedinec dle elektrody
     */
    class Individual
    {
        public int posX;
        public int posY;
        public float size;
        public int old;
        public float value; // EEG mV
        public enum type { };
        private enum direction { UP, RIGHT, DOWN, LEFT };
        private float historyDiff = 0;
        public Individual(int x = 0, int y = 0, float s = 1, float val = 0)
        {
            setPosition(x, y);
            size = s;
            old = 1;
            value = val;
        }
        public void setPosition(int x, int y)
        {
            posX = x;
            posY = y;
        }
        public void grow(float val)
        {
            float delta = Math.Abs(value - val);
            float diff = value - val;
            float av = (value + val) / (float)old;
            value = val;
            size += delta * 2;
            if (historyDiff < 0)
            {
                if (diff < 0)
                    move(direction.UP);
                if (diff > 0)
                    move(direction.DOWN);
            }
            else
            {
                if (diff < 0)
                    move(direction.LEFT);
                if (diff > 0)
                    move(direction.RIGHT);
            }
            historyDiff = diff;
        }
        private void move(direction where)
        {
            int delda = 1;
            switch (where)
            {
                case direction.UP:
                    posY -= delda;
                    break;
                case direction.RIGHT:
                    posX += delda;
                    break;
                case direction.DOWN:
                    posY += delda;
                    break;
                case direction.LEFT:
                    posX -= delda;
                    break;
            }
        }
    }
}
