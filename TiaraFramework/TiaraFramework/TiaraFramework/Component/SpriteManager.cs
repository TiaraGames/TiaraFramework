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
    public class SpriteManager : Microsoft.Xna.Framework.GameComponent
    {
        internal List<ASprite> spriteList;
        List<ASprite> spritesNeedRemove;
        bool _isShown = true;
        bool _isEnabled = true;

        public Vector2 MgrPosition;
        public Stage StageBase;
        public bool Shown
        {
            set
            {
                _isShown = value;
                foreach (ASprite s in spriteList)
                {
                    s.isShown = value;
                    if (s is Button)
                    {
                        ((Button)s).nowButton.isShown = value;
                        ((Button)s).isEnabled = value;
                    }
                }
            }
            get
            { 
                return _isShown; 
            }
        }
        public new bool Enabled
        {
            set
            {
                _isEnabled = value;
                foreach (ASprite btn in this.spriteList)
                    if(btn is Button)
                        ((Button)btn).isEnabled = value;
            }
            get
            {
                return _isEnabled;
            }
        }
        public ASprite Last
        { 
            get 
            {
                if (spriteList.Count >= 1)
                    return spriteList[spriteList.Count - 1];
                else
                    return null;
            } 
        }

        public SpriteManager(Game game, Stage stageBase)
            : base(game)
        {
            spriteList = new List<ASprite>();
            spritesNeedRemove = new List<ASprite>();
            this.StageBase = stageBase;
        }

        public ASprite this[int index]
        {
            get
            {
                return this.spriteList[index];
            }
            set
            {
                this.spriteList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                int i = 0;
                foreach (ASprite sp in spriteList)
                {
                    if (!sp.isEnd)
                        i++;
                }
                return i;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void RemoveAll()
        {
            foreach (ASprite s in spriteList)
                s.isEnd = true;
        }

        public void Add(ASprite sprite)
        {
            sprite.Manager = this;
            spriteList.Add(sprite);
            if (sprite.Slaves != null)
                foreach (ASprite slv in sprite.Slaves)
                    addSlave(slv);
        }
        private void addSlave(ASprite slave)
        {
            spriteList.Add(slave);
            if (slave.Slaves != null)
                foreach (ASprite slv in slave.Slaves)
                    addSlave(slv);
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= this.spriteList.Count)
                return false;
            this.spriteList[index].isEnd = true;
            return true;
        }

        public bool Remove(ASprite sprite)
        {
            if (spriteList.IndexOf(sprite) == -1)
                return false;
            for (int i = sprite.Slaves.Count - 1; i >= 0; i--)
                spriteList.Remove(sprite.Slaves[i]);
            spriteList.Remove(sprite);
            return true;
        }

        /// <summary>
        /// Hasn't been completed.
        /// </summary>
        public void Zoom(Vector2 Anchor, Vector2 Scale)
        {
            foreach (Sprite sp in spriteList)
            {
                sp.Scale *= Scale;
                sp.Position = Anchor + (sp.Position - Anchor) * Scale;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < spriteList.Count; i++)
            {
                spriteList[i].AllUpdate(gameTime);
                if (spriteList[i].isEnd)
                    spritesNeedRemove.Add(spriteList[i]);
                if (spriteList[i].Slaves != null)
                {
                    foreach (ASprite sp in spriteList[i].Slaves)
                    {
                        sp.Update(gameTime);
                        if (sp.isEnd)
                            spritesNeedRemove.Add(sp);
                    }
                    foreach (ASprite spr in spritesNeedRemove)
                        spriteList[i].Slaves.Remove(spr);
                }
            }
            foreach (ASprite s in spritesNeedRemove)
                this.spriteList.Remove(s);

            spritesNeedRemove.Clear();
        }




        // For foreach
        public Enumerator GetEnumerator()
        {
            return new Enumerator(spriteList);
        }

        public struct Enumerator
        {
            private int Position;
            private List<ASprite> spriteList;

            public Enumerator(List<ASprite> spriteList)
            {
                this.spriteList = spriteList;
                this.Position = -1;
            }

            public ASprite Current
            {
                get
                {
                    return spriteList[Position];
                }
            }

            public void Dispose()
            {
                this.Dispose();
            }

            public void Reset()
            {
                Position = -1;
            }

            public bool MoveNext()
            {
                if (Position < spriteList.Count - 1)
                {
                    Position++;
                    return true;
                }
                else
                    return false;
            }
        }
    }
}
