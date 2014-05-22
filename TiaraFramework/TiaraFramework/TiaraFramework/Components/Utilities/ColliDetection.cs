using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiaraFramework.Component
{
    public static class ColliDetection
    {
        public static bool isPointInRound(Point p, Round r)
        {
            if (Math.Sqrt((p.X - r.X) * (p.X - r.X) + (p.Y - r.Y) * (p.Y - r.Y)) < r.R)
                return true;
            else
                return false;
        }
        public static bool isPointInRect(Point p, Rectangle r)
        {
            if (p.X > r.X && p.X <= r.X + r.Width && p.Y > r.Y && p.Y <= r.Y + r.Height)
                return true;
            else
                return false;
        }
        public static bool isRectInRect(Rectangle i, Rectangle o)
        {
            if (i.X >= o.X && i.Y >= o.Y && i.X - o.X + i.Width <= o.Width && i.Y - o.Y + i.Height <= o.Height)
                return true;
            else
                return false;
        }
        public static bool isRoundInRound(Round ri, Round ro)
        {
            if ((ro.X - ri.X) * (ro.X - ri.X) + (ro.Y - ri.Y) * (ro.Y - ri.Y) <= (ro.R - ri.R) * (ro.R - ri.R))
                return true;
            else
                return false;
        }

        public static bool RectAndRound(Rectangle re, Round ro)
        {
            return isPointInRect(new Point(ro.X, ro.Y), new Rectangle(re.X - ro.R, re.Y - ro.R, re.Width + ro.R, re.Height + ro.R));
        }
        public static bool RoundAndRound(Round r1, Round r2)
        {
            if ((r1.X - r2.X) * (r1.X - r2.X) + (r1.Y - r2.Y) * (r1.Y - r2.Y) < (r1.R + r2.R) * (r1.R + r2.R))
                return true;
            else
                return false;
        }
        public static bool RectAndRect(Rectangle r1, Rectangle r2)
        {
            return r1.Intersects(r2);
        }
    }


    public class Round
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int R { get; set; }
        public Round() { }
        public Round(int X, int Y, int R)
        {
            this.X = X;
            this.Y = Y;
            this.R = R;
        }

        public Round(Vector2 pos, int R)
        {
            this.X = (int)pos.X;
            this.Y = (int)pos.Y;
            this.R = R;
        }
    }

    public class RoundF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public RoundF() { }
        public RoundF(float X, float Y, float R)
        {
            this.X = X;
            this.Y = Y;
            this.R = R;
        }
    }
}
