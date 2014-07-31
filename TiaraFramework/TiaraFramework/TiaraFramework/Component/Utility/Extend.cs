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

        public static Vector2 ToVector2(this Point pt)
        {
            return new Vector2(pt.X, pt.Y);
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

        public static Rectangle Move(this Rectangle rect, int X, int Y)
        {
            rect.X += X;
            rect.Y += Y;
            return rect;
        }

        public static Rectangle Move(this Rectangle rect, Vector2 vecMove)
        {
            rect.X += (int)vecMove.X;
            rect.Y += (int)vecMove.Y;
            return rect;
        }

        public static Rectangle Multiply(this Rectangle rect, float multiple, bool multiLocation)
        {
            rect.Width = (int)(rect.Width * multiple);
            rect.Height = (int)(rect.Height * multiple);
            if (multiLocation)
            {
                rect.X = (int)(rect.X * multiple);
                rect.Y = (int)(rect.Y * multiple);
            }
            return rect;
        }

        public static Vector2 GetSize(this Rectangle rect)
        {
            return new Vector2(rect.Width, rect.Height);
        }
        #endregion

        #region Vector2

        public static Vector2 Rotate(this Vector2 vec, float omega)
        {
            Matrix mtx = Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), omega);
            vec = Vector2.Transform(vec, mtx);
            return vec;
        }

        public static Point ToPoint(this Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }

        public static float GetAngle(this Vector2 v, Vector2 vec)
        {
            return (float)(Math.Atan2(v.Y - vec.Y, v.X - vec.X));
        }

        public static float GetAngle(this Vector2 v)
        {
            return (float)(Math.Atan2(v.Y, v.X) + Math.PI / 2);
        }

        public static Vector2 GetNormalize(this Vector2 v)
        {
            v.Normalize();
            return v;
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

        public static Color Multiply(this Color c, float factor)
        {
            return new Color(
                c.R * factor > 255 ? 255 : c.R * factor,
                c.G * factor > 255 ? 255 : c.G * factor,
                c.B * factor > 255 ? 255 : c.B * factor,
                c.A * factor > 255 ? 255 : c.A * factor);
        }

        public static Color Divide(this Color c, float divisor)
        {
            return new Color(
                c.R / divisor > 255 ? 255 : c.R / divisor,
                c.G / divisor > 255 ? 255 : c.G / divisor,
                c.B / divisor > 255 ? 255 : c.B / divisor,
                c.A / divisor > 255 ? 255 : c.A / divisor);
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
