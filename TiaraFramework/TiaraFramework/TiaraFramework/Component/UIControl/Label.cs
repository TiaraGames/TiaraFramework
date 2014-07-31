using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TiaraFramework.Component
{
    public class Label : ASprite
    {
        public string Text;
        public SpriteFont SpriteFont;
        public float Width { get { return SpriteFont.MeasureString(Text).X; } }
        public float Height { get { return SpriteFont.MeasureString(Text).Y; } }

        public Vector2 LT { get { return new Vector2(0, 0); } }
        public Vector2 CT { get { return new Vector2(Width / 2, 0); } }
        public Vector2 RT { get { return new Vector2(Width, 0); } }
        public Vector2 LC { get { return new Vector2(0, Height / 2); } }
        public Vector2 CC { get { return new Vector2(Width / 2, Height / 2); } }
        public Vector2 RC { get { return new Vector2(Width, Height / 2); } }
        public Vector2 LB { get { return new Vector2(0, Height); } }
        public Vector2 CB { get { return new Vector2(Width / 2, Height); } }
        public Vector2 RB { get { return new Vector2(Width, Height); } }

        public Label(string text, Vector2 position, string spriteFontPath, Color color, float depth, Game game)
            : base(game)
        {
            this.Text = text;
            this.SpriteFont = game.Content.Load<SpriteFont>(spriteFontPath);
            this.Position = position;
            this.Color = color;
            this.Depth = depth;
        }

        public Label(string text, Vector2 position, string spriteFontPath, Color color, float rotation, Vector2 origin,
            Vector2 scale, SpriteEffects spriteEffect, float depth, Game game)
            : this(text, position, spriteFontPath, color, depth, game)
        {
            this.Rotation = rotation;
            this.Origin = origin;
            this.Scale = scale;
            this.SpriteEffect = spriteEffect;
            this.Depth = depth;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public ASprite CopyBase()
        {
            ASprite sp = (ASprite)this.MemberwiseClone();
            if (sp.Slaves != null)
            {
                sp.Slaves = new List<ASprite>();
                foreach (Sprite slv in this.Slaves)
                    sp.Slaves.Add(slv.Copy());
            }
            if (this.ActStoreList != null)
            {
                sp.ActStoreList = new List<Act>();
                foreach (Act act in this.ActStoreList)
                    sp.ActStoreList.Add(act.Copy());
                sp.ActPlayingList = new List<Act>();
                foreach (Act act in this.ActPlayingList)
                    sp.ActPlayingList.Add(act.Copy());
            }
            return sp;
        }

        public Label Copy()
        {
            return (Label)this.CopyBase();
        }
    }
}