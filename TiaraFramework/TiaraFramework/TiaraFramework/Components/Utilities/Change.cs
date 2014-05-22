using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace TiaraFramework.Component
{
    public static class Change
    {
        public static Rectangle RectZoom(Rectangle rect, int value)
        {
            rect.X -= value;
            rect.Y -= value;
            rect.Width += 2 * value;
            rect.Height += 2 * value;
            return rect;
        }

        public static Point Vector2ToPoint(Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }

        public static Vector2 PointToVector2(Point pt)
        {
            return new Vector2(pt.X, pt.Y);
        }

        public static Vector2 RotateVector2(Vector2 vec, float omega)
        {
            Matrix mtx = Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), omega);
            vec = Vector2.Transform(vec, mtx);
            return vec;
        }

        public static T[,] ArrayToArray2<T>(T[] Array, int width) where T : struct
        {
            T[,] array2 = new T[width, Array.Length / width];
            for (int i = 0; i < Array.Length; i++)
                array2[i / width, i % width] = Array[i];
            return array2;
        }
        
        public static T[] Array2ToArray<T>(T[,] Array2, int width) where T : struct
        {
            T[] array = new T[Array2.Length];
            for (int i = 0; i < width; i++)
                for (int j = 0; i < Array2.Length / width; i++)
                    array[i * width + j] = Array2[i, j];
            return array;
        }

        public static Stream BytesToStream(this byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}