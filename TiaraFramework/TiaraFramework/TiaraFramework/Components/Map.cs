using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TiaraFramework.Component
{

    [Serializable]
    public class Map
    {
        public List<Grid> Mesh { get; set; }
        public int RowNum { get; set; }
        public int ColNum { get; set; }
        public List<byte[]> Textures { get; set; }
    }

    [Serializable]
    public class Grid
    {
        public Vector2 Position { get; set; }
        public int Layer { get; set; }
        public int TextureIndex { get; set; }
        public Rectangle DrawRect { get; set; }
        public bool isCollision { get; set; }
    }

    public static class MapOperate
    {
        public static Map Trim(Map map)
        {
            float minX = map.Mesh[0].Position.X;
            float minY = map.Mesh[0].Position.Y;
            foreach (Grid grid in map.Mesh)
            {
                if (grid.Position.X < minX)
                    minX = grid.Position.X;
                if (grid.Position.Y < minY)
                    minY = grid.Position.Y;
            }
            foreach (Grid grid in map.Mesh)
                grid.Position -= new Vector2(minX, minY);
            return map;
        }
    }

    public static class MapAccess
    {
        public static Map Load(string path, out Point size)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Map));
                Map map = (Map)xs.Deserialize(File.Open(path, FileMode.Open));
                map = MapOperate.Trim(map);
                size = new Point(map.ColNum, map.RowNum);
                return map;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                size = Point.Zero;
                return null;
            }
        }

        public static List<Sprite> Load(string path, Game game, out Point size)
        {
            Map map = Load(path, out size);
            if (map == null)
                return null;
            // hold map source textures
            Texture2D[] texts = new Texture2D[map.Textures.Count];
            for (int i = 0; i < texts.Length; i++)
                texts[i] = Texture2D.FromStream(game.GraphicsDevice, Change.BytesToStream(map.Textures[i]));
            // build map
            List<Sprite> liMap = new List<Sprite>();
            foreach (Grid grid in map.Mesh)
            {
                Sprite spGrid = Sprite.OneFrameSprite(
                    grid.Position,
                    texts[grid.TextureIndex],
                    0.1f + grid.Layer * 0.1f, // 0.1 2 4 5 for map
                    game);
                spGrid.DrawRect = grid.DrawRect;
                spGrid.ColliRect = new Rectangle(0, 0, grid.DrawRect.Width, grid.DrawRect.Height);
                if (!grid.isCollision)
                    spGrid.isColli = false;
                liMap.Add(spGrid);
            }

            return liMap;
        }
    }
}
