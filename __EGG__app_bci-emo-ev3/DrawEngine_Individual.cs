using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Point = SharpDX.Point;

namespace __EGG__app_bci_emo_ev3
{
    /*
     * jedinec dle elektrody
     */
    class DrawEngine_Individual 
    {
        public int X;
        public int Y;
        private int Xdefault;
        private int Ydefault;
        private int Xmax;
        private int Ymax;
        public float size;
        private float sizeDefault;
        public int old;
        public float value; // EEG mV
        public enum type { };
        private enum direction { UP, RIGHT, DOWN, LEFT };
        private List<Point> historyPosition = new List<Point>(); // zasobnik hodnot
        public List<float> historyValue = new List<float>(); // zasobnik hodnot
        private int historyMax = 500;

        public DrawEngine_Individual(int x = 0, int y = 0, float s = 1, float val = 0, int xmax = 800, int ymax = 600)
        {
            setPosition(x, y);
            setPositionLimit(xmax, ymax);
            size = s;
            value = val;
            // vychozi stavy
            Xdefault = X;
            Ydefault = Y;
            sizeDefault = size;
        }
        private void reset()
        {
            X = Xdefault;
            Y = Ydefault;
            size = sizeDefault;
        }
        public void setPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void setPositionLimit(int x, int y)
        {
            Xmax = x;
            Ymax = y;
        }
        private void XYlimit()
        {
            if (X < 0)
                X = 0;
            if (Y < 0)
                Y = 0;
            if (X > Xmax)
                X = Xmax;
            if (X > Xmax)
                X = Xmax;
        }
        private void setHistoryPosition(Point val)
        {
            historyPosition.Add(val);
            if (historyPosition.Count >= historyMax)
                historyPosition.RemoveAt(0);
        }
        private void setHistoryValue(float val)
        {
            historyValue.Add(val);
            if (historyValue.Count >= historyMax)
                historyValue.RemoveAt(0);
        }
        public void grow(float val)
        {
            float diff = value - val;
            float delta = Math.Abs(diff);
            value = val;
            setHistoryPosition(new Point(X, Y));
            setHistoryValue(val);

            float av = historyValue.Average();
            X = Xdefault + (int)(av*100);
            float angle = 360 * av;
            float r = sizeDefault+size;
            size += 1;
            X = Xdefault + (int)(Math.Cos(angle) * r);
            Y = Ydefault + (int)(Math.Sin(angle)*r);
            XYlimit();

            /*
            value = val;
            size += delta * 1.0002f;
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
             * */
        }
        private void move(direction where)
        {
            int delda = 1;
            switch (where)
            {
                case direction.UP:
                    Y -= delda;
                    break;
                case direction.RIGHT:
                    X += delda;
                    break;
                case direction.DOWN:
                    Y += delda;
                    break;
                case direction.LEFT:
                    X -= delda;
                    break;
            }
        }
    }
}
