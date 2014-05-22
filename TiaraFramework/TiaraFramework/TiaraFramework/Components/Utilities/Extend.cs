using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace TiaraFramework.Component.Extend
{
    public static class EXT
    {
        #region Point
        public static Point Plus(this Point point, Point addPoint)
        {
            return new Point(point.X + addPoint.X, point.Y + addPoint.Y);
        }

        public static Point Minus(this Point point, Point subPoint)
        {
            return new Point(point.X - subPoint.X, point.Y - subPoint.Y);
        }

        public static Point ToPoint(this Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }
        #endregion

        #region Rectangle
        public static Rectangle Expand(this Rectangle rect, Point point)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width + point.X, rect.Height + point.Y);
        }

        public static Rectangle Shrink(this Rectangle rect, Point point)
        {
            return new Rectangle(rect.X, rect.Y,
                rect.Width - point.X < 0 ? 0 : rect.Width - point.X,
                rect.Height - point.Y < 0 ? 0 : rect.Height - point.Y);
        }

        public static Rectangle Zoom(this Rectangle rect, int value)
        {
            rect.X -= value;
            rect.Y -= value;
            rect.Width = rect.Width + 2 * value > 0 ? rect.Width + 2 * value : 0;
            rect.Height = rect.Height + 2 * value > 0 ? rect.Height + 2 * value : 0;
            return rect;
        }
        #endregion

        #region Vector2
        public static Vector2 ToVector2(this Point pt)
        {
            return new Vector2(pt.X, pt.Y);
        }

        public static Vector2 Rotate(this Vector2 vec, float omega)
        {
            Matrix mtx = Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), omega);
            vec = Vector2.Transform(vec, mtx);
            return vec;
        }

        public static float GetAngle(this Vector2 v, Vector2 vec)
        {
            return (float)(Math.Atan2(v.Y - vec.Y, v.X - vec.X));
        }

        public static float GetAngle(this Vector2 v)
        {
            return (float)(Math.Atan2(v.Y, v.X));
        }
        #endregion

        #region Color
        public static Color Plus(this Color c, Color color)
        {
            return new Color(
                c.R + color.R > 255 ? 255 : c.R + color.R,
                c.G + color.G > 255 ? 255 : c.G + color.G,
                c.B + color.B > 255 ? 255 : c.B + color.B,
                c.A + color.A > 255 ? 255 : c.A + color.A);
        }

        public static Color Plus(this Color c, int A, int R = 0, int G = 0, int B = 0)
        {
            return new Color(
                c.R + R > 255 ? 255 : c.R + R,
                c.G + G > 255 ? 255 : c.G + G,
                c.B + B > 255 ? 255 : c.B + B,
                c.A + A > 255 ? 255 : c.A + A);
        }

        public static Color Minus(this Color c, Color color)
        {
            return new Color(
                c.R - color.R < 0 ? 0 : c.R - color.R,
                c.G - color.G < 0 ? 0 : c.G - color.G,
                c.B - color.B < 0 ? 0 : c.B - color.B,
                c.A - color.A < 0 ? 0 : c.A - color.A);
        }

        public static Color Minus(this Color c, int A, int R = 0, int G = 0, int B = 0)
        {
            return new Color(
                c.R - R < 0 ? 0 : c.R - R,
                c.G - G < 0 ? 0 : c.G - G,
                c.B - B < 0 ? 0 : c.B - B,
                c.A - A < 0 ? 0 : c.A - A);
        }
        #endregion

        #region Texture2D
        public static Texture2D Blend(Texture2D ttBack, Texture2D ttFore, Vector2 Offset, Game game)
        {
            Texture2D ttMerged = new Texture2D(
                game.GraphicsDevice,
                ttBack.Width > ttFore.Width + Offset.X ? ttBack.Width : ttFore.Width + (int)Offset.X,
                ttBack.Width > ttFore.Width + Offset.X ? ttBack.Width : ttFore.Width + (int)Offset.X);
            return ttMerged;
        }
        #endregion
    }
}
