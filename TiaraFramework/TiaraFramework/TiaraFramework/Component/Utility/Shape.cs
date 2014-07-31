using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TiaraFramework.Component
{
    public static class Pixel
    {
        public static Texture2D P1;
        public static Texture2D P2;
        public static Texture2D P3;
        public static Texture2D P5;

        public static void Init(Game game)
        {
            P1 = new Texture2D(game.GraphicsDevice, 1, 1);
            P1.SetData<Color>(new Color[] { Color.White });

            P2 = new Texture2D(game.GraphicsDevice, 2, 2);
            P2.SetData<Color>(new Color[] { Color.White, Color.White, Color.White, Color.White, });

            P3 = new Texture2D(game.GraphicsDevice, 3, 3);
            Color[] c3 = new Color[9];
            for (int i = 0; i < 9; i++)
                c3[i] = Color.White;
            P3.SetData<Color>(c3); 
            
            P5 = new Texture2D(game.GraphicsDevice, 5, 5);
            Color[] c5 = new Color[25];
            for (int i = 0; i < 25; i++)
                c5[i] = Color.White;
            P5.SetData<Color>(c5);
        }
    }

    public class Shape
    {
        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 返回一个实心圆Texture2D
        /// </summary>
        public static Texture2D GetRound(int R, Color color, Game game)
        {
            //Texture2D ttRound = new Texture2D(game.GraphicsDevice, R * 2 + 1, R * 2 + 1);
            Texture2D ttRound = new Texture2D(game.GraphicsDevice, R * 2, R * 2);
            Byte4[] btRound = new Byte4[ttRound.Width * ttRound.Width];
            for (int i = 0; i < btRound.Length; i++)
            {
                Vector2 vtO = new Vector2(R, R);
                Point ptPos = new Point(i / ttRound.Width, i % ttRound.Width);
                // ptPos.X = Row number, ptPos.Y = Col number

                if (Math.Pow((ptPos.X - vtO.X), 2) + Math.Pow((ptPos.Y - vtO.Y), 2) <= R * R)
                {
                    uint packed = (uint)(
                        (color.A << 24) +
                        (color.B << 16) +
                        (color.G << 8) +
                        color.R
                        );
                    btRound[i].PackedValue = packed;
                }
                else
                {
                    btRound[i].PackedValue = 0;
                }
            }
            ttRound.SetData<Byte4>(btRound);

            return ttRound;
        }
        /// <summary>
        /// 返回一个实心圆Sprite，旋转中心在圆心。
        /// </summary>
        public static Sprite GetRound(Vector2 O, int R, Color color, float Depth, Game game)
        {
            Sprite spRound = Sprite.OneFrameSprite(
                O,
                GetRound(R, color, game),
                new Vector2(R + 1, R + 1),
                0,
                Color.White,
                Depth,
                game);

            return spRound;
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 返回一个实心矩形Texture2D
        /// </summary>
        public static Texture2D GetRectangle(int width, int height, Color color, Game game)
        {
            Texture2D ttRect = new Texture2D(game.GraphicsDevice, width, height);
            Byte4[] btRect = new Byte4[width * height];
            uint packed = (uint)((color.A << 24) + (color.B << 16) + (color.G << 8) + color.R);

            for (int i = 0; i < btRect.Length; i++)
                btRect[i].PackedValue = packed;
            ttRect.SetData<Byte4>(btRect);

            return ttRect;
        }
        /// <summary>
        /// 返回一个实心矩形Sprite
        /// </summary>
        public static Sprite GetRectangle(Rectangle rect, Color color, float depth, Game game)
        {
            return GetRectangle(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, color, depth, game);
        }
        /// <summary>
        /// 返回一个实心矩形Sprite
        /// </summary>
        public static Sprite GetRectangle(Vector2 position, int width, int height, Color color, float depth, Game game)
        {
            return Sprite.OneFrameSprite(position, GetRectangle(width, height, color, game), depth, game);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 返回一个空心矩形Texture2D
        /// </summary>
        public static Texture2D GetHollowRectangle(int width, int height, int border, Color color, Game game)
        {
            Texture2D ttRect = new Texture2D(game.GraphicsDevice, width, height);
            Byte4[,] btRect = new Byte4[width, height];
            uint packed = (uint)((color.A << 24) + (color.B << 16) + (color.G << 8) + color.R);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                    btRect[i, j].PackedValue = packed;
                if (j == border - 1)
                    j = height - border - 1;
            }
            for (int j = 0; j < width; j++)
            {
                for (int i = border; i < height - border; i++)
                    btRect[j, i].PackedValue = packed;
                if (j == border - 1)
                    j = width - border - 1;
            }
            ttRect.SetData<Byte4>(Change.Array2ToArray<Byte4>(btRect, width));

            return ttRect;
        }
        /// <summary>
        /// 返回一个空心矩形Sprite
        /// </summary>
        public static Sprite GetHollowRectangle(Vector2 position, int width, int height, int border, Color color, float depth, Game game)
        {
            return Sprite.OneFrameSprite(position, GetHollowRectangle(width, height, border, color, game), depth, game);
        }
        /// <summary>
        /// 返回一个空心矩形Sprite
        /// </summary>
        public static Sprite GetHollowRectangle(Rectangle rect, Color color, int border, float depth, Game game)
        {
            return GetHollowRectangle(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, border, color, depth, game);
        }



        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 返回一个菱形Texture2D
        /// </summary>
        public static Texture2D GetDiamond(int width, int height, Color color, Game game)
        {

            Texture2D ttDmd = new Texture2D(game.GraphicsDevice, width, height);
            Byte4[] btDmd = new Byte4[width * height];
            Vector2 vtO = new Vector2(width / 2f, height / 2f);
            // Point ptPos = new Point(i / ttRound.Width, i % ttRound.Width);
            // ptPos.X = Row number, ptPos.Y = Col number
            for (int x = 0; x <= width / 2; x++)
            {
                for (int y = 0; y <= height / 2; y++)
                {
                    if (width / 2f * y + height / 2f * x >= width * height / 4f)
                    {
                        uint packed = (uint)(
                                (color.A << 24) +
                                (color.B << 16) +
                                (color.G << 8) +
                                color.R
                                );
                        btDmd[Vector2ToIndex(new Vector2(x, y), width)].PackedValue = packed;
                        if (x != 0)
                            btDmd[Vector2ToIndex(new Vector2(width - x, y), width)].PackedValue = packed;
                        if (y != 0)
                            btDmd[Vector2ToIndex(new Vector2(x, height - y), width)].PackedValue = packed;
                        if (x != 0 && y != 0)
                            btDmd[Vector2ToIndex(new Vector2(width - x, height - y), width)].PackedValue = packed;
                    }
                }
            }
            ttDmd.SetData<Byte4>(btDmd);

            return ttDmd;
        }
        /// <summary>
        /// 返回一个菱形Sprite，旋转中心在中心
        /// </summary>
        public static Sprite GetDiamond(Vector2 position, int width, int height, Color color, float depth, Game game)
        {
            Sprite spDmd = Sprite.OneFrameSprite(position, GetDiamond(width, height, color, game), depth, game);
            spDmd.Origin = new Vector2(width / 2, height / 2);
            return spDmd;
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 返回1像素宽的直线Texture2D
        /// </summary>
        public static Texture2D GetLine(Vector2 vecLine, Color color, Game game)
        {
            Texture2D ttLine = new Texture2D(game.GraphicsDevice, (int)vecLine.X, (int)vecLine.Y);
            Byte4[] btLine = new Byte4[(int)vecLine.X * (int)vecLine.Y];
            for (int x = 0; x < vecLine.X; x++)
            {
                uint packed = (uint)(
                        (color.A << 24) +
                        (color.B << 16) +
                        (color.G << 8) +
                        color.R
                        );
                btLine[Vector2ToIndex(new Vector2(x, vecLine.Y * x / vecLine.X), (int)vecLine.X)].PackedValue = packed;
            }
            ttLine.SetData<Byte4>(btLine);

            return ttLine;
        }
        /// <summary>
        /// 返回1像素宽的直线Sprite, 旋转中心在左上角
        /// </summary>
        public static Sprite GetLine(Vector2 position, Vector2 vecLine, Color color, float depth, Game game)
        {
            return Sprite.OneFrameSprite(position, GetLine(vecLine, color, game), depth, game);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 字节位置转换为二维坐标，Y轴正方向朝下
        /// </summary>
        private static Vector2 IndexToVector2(int i, int width)
        {
            return new Vector2(i % width, i / width);
        }

        /// <summary>
        /// 二维坐标转换为字节位置，Y轴正方向朝下
        /// </summary>
        private static int Vector2ToIndex(Vector2 pos, int width)
        {
            return (int)pos.Y * width + (int)pos.X;
        }
    }
}
