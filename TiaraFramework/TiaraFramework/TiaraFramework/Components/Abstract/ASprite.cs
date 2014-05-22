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
    public abstract class ASprite
    {
        Game Game;
        protected SpriteManager _manager;
        public SpriteManager Manager
        {
            set
            {
                if (this.Slaves != null)
                    foreach (Sprite s in this.Slaves)
                        s.Manager = value;
                _manager = value;
                if (this is Button)
                    ((Button)this).spMoveOn.Manager = ((Button)this).spMoveOff.Manager = ((Button)this).spPressed.Manager = ((Button)this).spPressing.Manager = value;
            }
            get { return _manager; }
        }


        public ASprite Owner;
        public List<ASprite> Slaves;

        public Vector2 Position; // the position relative to it's manager
        public float Depth;
        public Vector2 Origin;
        public Color Color;
        public float Rotation;
        public Vector2 Scale;
        public SpriteEffects SpriteEffect;
        public int FPS { get; protected set; }

        public bool isPositive;
        public bool isAniStoped;
        public bool isActStoped;
        public bool isEnd;
        public bool isShown;
        public bool isStopAtLastFrame;
        public bool isColli;

        protected int tslf; // time since last frame
        protected int mspf; // milliscoends per frame
        public Point ColliOffset;
        public Rectangle ColliRect;
        public List<Act> ActList;
        public List<Act> ActPlayingList;
        public Dictionary<string, object> Tags;

        Vector2 _prePosition;
        float _preRotation;
        bool isFirstFrame = true;
        public Vector2 MovedVlaue { get; private set; }
        public float RotatedValue { get; private set; }

        public ASprite(Game game)
        {
            this.Game = game;
            Initialize();
        }

        public virtual void Initialize()
        {
            Scale = Vector2.One;
            Color = Color.White;
            Rotation = 0;
            SpriteEffect = SpriteEffects.None;
            FPS = 60;
            tslf = 0;
            Tags = new Dictionary<string, object>();

            isAniStoped = false;
            isActStoped = false;
            isEnd = false;
            isShown = true;
            isColli = true;

            Owner = null;
            _prePosition = Position;
            _preRotation = Rotation;

        }

        public virtual void PreUpdate(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void PostUpdate(GameTime gameTime)
        {
            if (isFirstFrame)
            {
                isFirstFrame = false;
                _prePosition = Position;
                _preRotation = Rotation;
            }
            else
            {
                MovedVlaue = Position - _prePosition;
                RotatedValue = Rotation - _preRotation;
                _prePosition = Position;
                _preRotation = Rotation;

                if(Slaves != null)
                    foreach (ASprite slv in Slaves)
                    {
                        slv.Position += MovedVlaue;
                        slv.Rotation += RotatedValue;
                    }
            }
        }

        public virtual void _allupdate(GameTime gameTime)
        {
            PreUpdate(gameTime);
            Update(gameTime);
            PostUpdate(gameTime);
        }

        public void AddSlave(ASprite slave)
        {
            if (Slaves == null)
                Slaves = new List<ASprite>();
            slave.Owner = this;
            slave.Manager = this.Manager;
            Slaves.Add(slave);
        }
    }
}
