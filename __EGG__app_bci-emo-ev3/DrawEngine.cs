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
using FactoryDXGI = SharpDX.DXGI.Factory;
using Point = SharpDX.Point;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Device = SharpDX.Direct2D1.Device;
using DeviceDXGI = SharpDX.DXGI.Device;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using Texture2D = SharpDX.Toolkit.Graphics.Texture2D;
using SharpDX.WIC;
using SharpDX.IO;

namespace __EGG__app_bci_emo_ev3
{
    public class DrawEngine
    {
        private const int NoE = 14; // pocet zobrazenych snimacu
        private const int sleep = 0; // idle time
        private string[] channelNames = new string[14] { "F4", "AF4", "F8", "FC6", "T8", "P8", "O2", "O1", "P7", "T7", "FC5", "F7", "AF3", "F3" };
        private enum qualityNames { EEG_CQ_NO_SIGNAL, EEG_CQ_VERY_BAD, EEG_CQ_POOR, EEG_CQ_FAIR, EEG_CQ_GOOD };

        private Thread renderThread;
        private Factory factory;
        //        private ImagingFactory imagingFactory;
        //      private NativeFileStream fileStream;
        private WindowRenderTarget renderTarget;
        //private RenderTarget target;
        private Bitmap bitmap;
        RenderLoop.RenderCallback callback;
        private float[] data = new float[NoE];
        private DrawEngine_Individual[] ind = new DrawEngine_Individual[NoE];
        private Point[] point = new Point[NoE];
        private Dictionary<string, string> qualitySignalData = new Dictionary<string, string>();
        public bool connect = false;

        // form vars
        private Form1 form;
        private int canvasWidth;
        private int canvasHeight;

        public DrawEngine(Form1 f)
        {
            try
            {
                form = f;
                canvasWidth = form.canvas.ClientSize.Width;
                canvasHeight = form.canvas.ClientSize.Height;
                factory = new Factory(SharpDX.Direct2D1.FactoryType.MultiThreaded);
                RenderTargetProperties winProp = new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));

                //set hwnd target properties (permit to attach Direct2D to window)
                HwndRenderTargetProperties hwnd = new HwndRenderTargetProperties()
                {
                    Hwnd = form.canvas.Handle,
                    PixelSize = new Size2(canvasWidth, canvasHeight),
                    PresentOptions = PresentOptions.Immediately
                };

                renderTarget = new WindowRenderTarget(factory, winProp, hwnd);
                renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
                
                BitmapProperties props = new BitmapProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));
                bitmap = new Bitmap(renderTarget, new Size2(500, 300), props);
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

            initPoints();
            int offsetX = canvasWidth/2;
            int offsetY = canvasHeight/2;
            int zoom = 55;
            int size = 10;
            for (int i = 0; i < NoE; i++)
            {
                int x = point[i].X * zoom + offsetX;
                int y = point[i].Y * zoom + offsetY;
                ind[i] = new DrawEngine_Individual(x, y, size, data[i], canvasWidth, canvasHeight);
            }
            callback = new RenderLoop.RenderCallback(render);
            RenderLoop.Run(form.canvas, callback);
        }
        public void stop()
        {
            renderTarget.Dispose();
            renderTarget = null;
        }
        private void simulate()
        {
            while (renderThread.IsAlive)
            {
                Random noise = new Random();
                String tmp = "";
                for (int i = 0; i < NoE; i++)
                {
                    data[i] = noise.NextFloat(0, 1);
                    tmp += data[i].ToString() + ", ";
                }
                Thread.Sleep(550);
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

        // hlavni­ render smycka
        bool firstRender = true;
        private void render()
        {
            renderTarget.BeginDraw();
            if (firstRender)
            {
                renderTarget.Clear(Color4.White);
                firstRender = false;

            }
                        //renderTarget.Clear(Color4.White);
            drawQualitySignal(15, canvasWidth - 90, 80);
            drawConectivity();
            dPolygon();
            draw();
            renderTarget.Flush();
            renderTarget.EndDraw();
            //form.labelLog.Text = ind[0].historyValue.Average().ToString();
            Application.DoEvents();
            Thread.Sleep(sleep+100);
            form.canvas.Update();
        }

        /*
         * kvalita signalu
         */
        public void setQualitySignalData(Dictionary<string, string> quality)
        {
            qualitySignalData = quality;
        }
        // bod/kvaita signalu
        private void dPointSenzor(float x, float y, float radius, string quality)
        {
            Color4 color = new Color4(0, 0, 0, 1);
            switch ((qualityNames)Enum.Parse(typeof(qualityNames), quality))
            {
                case qualityNames.EEG_CQ_NO_SIGNAL:
                    color = new Color4(0, 0, 0, 1);
                    break;
                case qualityNames.EEG_CQ_VERY_BAD:
                    color = new Color4(1, 0, 0, 1);
                    break;
                case qualityNames.EEG_CQ_POOR:
                    color = new Color4(1, 0.5f, 0, 1);
                    break;
                case qualityNames.EEG_CQ_FAIR:
                    color = new Color4(1, 1, 0, 1);
                    break;
                case qualityNames.EEG_CQ_GOOD:
                    color = new Color4(0, 1, 0, 1);
                    break;
            }
            SolidColorBrush brush = new SolidColorBrush(renderTarget, color);
            renderTarget.FillEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
            brush.Dispose();
        }
        public void drawQualitySignal(int zoom, int offsetX, int offsetY)
        {
            for (int i = 0; i < NoE; i++)
            {
                int x = point[i].X * zoom + offsetX;
                int y = point[i].Y * zoom + offsetY;
                if (qualitySignalData.Count > 1)
                    dPointSenzor(x, y, 10, qualitySignalData[this.channelNames[i]]);
            }
        }


        /*
         * vykreslovani
         */
        // kruh/bod/objekt
        private void dPoint(float x, float y, float radius, float value)
        {
            float r = value;
            float g = value;
            float b = value;
            float a = 0.1f;
            SolidColorBrush brush = new SolidColorBrush(renderTarget, new Color4(r, g, b, a));
            /*
            GradientStop[] gradientStop = new GradientStop[2];
            gradientStop[0] = new GradientStop() { Color = new Color4(r, g, b, a), Position = 0.0f };
            gradientStop[1] = new GradientStop() { Color = new Color4(r, g, b, 0), Position = 1.0f };
            GradientStopCollection gradientStopCollection = new GradientStopCollection(renderTarget, gradientStop, ExtendMode.Clamp);
            RadialGradientBrushProperties radialGradientBrush = new RadialGradientBrushProperties()
            {
                RadiusX = radius,
                RadiusY = radius,
                Center = new Vector2(x, y),
                GradientOriginOffset = new Vector2(0, 0)
            };
            RadialGradientBrush brush = new RadialGradientBrush(renderTarget, radialGradientBrush, gradientStopCollection);
             * */
            //renderTarget.DrawEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
            renderTarget.FillEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
            brush.Dispose();
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
        private int maxOld = 10000;
        private void draw()
        {
            /*
            if (ind[0].old > maxOld)
            {
                ind[0].old = 0;
                //renderTarget.Clear(Color4.White);
            }*/
            for (int i = 0; i < NoE; i++)
            {
                ind[i].grow(data[i]);
                dPoint(ind[i].X, ind[i].Y, ind[i].size, ind[i].value);
                ind[i].old++;
            }
        }


        /*
         * stav konektivity
         */
        // bod/signal on/off
        private void dPointConnect(float x, float y, float radius, bool status)
        {
            Color4 color = new Color4(1, 0, 0, 1);
            if (status)
            {
                color = new Color4(0, 1, 0, 1);
            }
            SolidColorBrush brush = new SolidColorBrush(renderTarget, color);
            renderTarget.FillEllipse(new Ellipse(new Vector2(x, y), radius, radius), brush);
            brush.Dispose();
            //LoadFromFile(renderTarget, "c:\\workspace\\csharp\\__EGG__app_bci-emo-ev3\\__EGG__app_bci-emo-ev3\\bin\\Debug\\epoc.png");
        }
        private void drawConectivity()
        {
            int size = 20;
            int padding = 10;
            dPointConnect(form.canvas.ClientSize.Width - size - padding, form.canvas.ClientSize.Height - size - padding, size, connect);
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
            Vector2[] vector = new Vector2[NoE];
            GeometrySink geometrySink;
            PathGeometry pathGeometry;
            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();
            geometrySink.BeginFigure(new Vector2(ind[0].X, ind[0].Y), new FigureBegin());
            for (int i = 1; i < NoE; i++)
                geometrySink.AddLine(new Vector2(ind[i].X, ind[i].Y));
            geometrySink.AddLine(new Vector2(ind[0].X, ind[0].Y));
            /*
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = new Vector2(ind[0].X, ind[0].Y),
                Point2 = new Vector2(ind[1].X, ind[1].Y),
                Point3 = new Vector2(ind[2].X, ind[2].Y)
            });
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = new Vector2(ind[2].X, ind[2].Y),
                Point2 = new Vector2(ind[3].X, ind[3].Y),
                Point3 = new Vector2(ind[4].X, ind[4].Y)
            });
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = new Vector2(ind[4].X, ind[4].Y),
                Point2 = new Vector2(ind[5].X, ind[5].Y),
                Point3 = new Vector2(ind[6].X, ind[6].Y)
            });*/
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();
            SolidColorBrush pen = new SolidColorBrush(renderTarget, new Color4(0, 0, 1, 0.1f));
            float value = 0.3f;
            float r = value;
            float g = value;
            float b = 1;
            float a = 0.01f;
            /*
            GradientStop[] gradientStop = new GradientStop[2];
            gradientStop[0] = new GradientStop() { Color = new Color4(r, g, b, a), Position = 0.0f };
            gradientStop[1] = new GradientStop() { Color = new Color4(r, g, b, 0), Position = 1.0f };
            RadialGradientBrush fill = radialGradient(250, 250, 500, gradientStop);
             * */
            SolidColorBrush brush = new SolidColorBrush(renderTarget, new Color4(r, g, b, a));
            //renderTarget.DrawGeometry(pathGeometry, pen);
            renderTarget.FillGeometry(pathGeometry, brush);
            pathGeometry.Dispose();
            geometrySink.Dispose();
        }

    };

}