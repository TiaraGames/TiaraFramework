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
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public struct Attach
    {
        public bool IsPosition;
        public bool IsRotation;
        public bool IsColor;
        public bool IsAlpha;

        public Attach(bool isAttach)
        {
            this.IsPosition = this.IsRotation = this.IsColor = this.IsAlpha = isAttach;
        }
        public Attach(bool position, bool rotation, bool color, bool alpha)
        {
            this.IsPosition = position;
            this.IsRotation = rotation;
            this.IsColor = color;
            this.IsAlpha = alpha;
        }
    }

    public abstract class ASprite
    {
        protected Game Game;
        protected SpriteManager _manager;
        public SpriteManager Manager
        {
            set
            {
                if (this.Slaves != null)
                    foreach (ASprite s in this.Slaves)
                        s.Manager = value;
                _manager = value;
                if (this is Button)
                    ((Button)this).spMoveOn.Manager = ((Button)this).spMoveOff.Manager = ((Button)this).spPressed.Manager = ((Button)this).spPressing.Manager = value;
            }
            get { return _manager; }
        }
        protected Dictionary<string, SpriteManager> SprMgrClct { get { return Manager.StageBase.SprMgrClct; } }

        public ASprite Owner;
        public List<ASprite> Slaves;
        public Attach AttachToOwner;

        public Vector2 Position; // the position relative to it's manager
        public float Depth;
        public Vector2 Origin;
        public Color Color;
        public float Rotation;
        public Vector2 Scale;
        public SpriteEffects SpriteEffect;

        public bool isActStoped;
        public bool isEnd;
        public bool isShown;
        public bool isColli;
        public Point ColliOffset;
        public Rectangle ColliRect;
        public List<Act> ActStoreList;
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

        public ASprite(ASprite sp)
        {
            Game = sp.Game;
            _manager = sp._manager;
            Owner = sp.Owner;

            Position = sp.Position;
            Depth = sp.Depth;
            Origin = sp.Origin;
            Color = sp.Color;
            Rotation = sp.Rotation;
            Scale = sp.Scale;
            SpriteEffect = sp.SpriteEffect;

            isActStoped = sp.isActStoped;
            isEnd = sp.isEnd;
            isShown = sp.isShown;
            isColli = sp.isColli;

            ColliOffset = sp.ColliOffset;
            ColliRect = sp.ColliRect;

            Tags = new Dictionary<string, object>(sp.Tags);

            _prePosition = sp._prePosition;
            _preRotation = sp._preRotation;

            MovedVlaue = sp.MovedVlaue;
            RotatedValue = sp.RotatedValue;

            isFirstFrame = sp.isFirstFrame;

            if (sp.Slaves != null)
            {
                this.Slaves = new List<ASprite>();
                foreach (Sprite slv in sp.Slaves)
                    this.Slaves.Add(slv.Copy());
            }
            if (sp.ActStoreList != null)
            {
                this.ActStoreList = new List<Act>();
                foreach (Act act in sp.ActStoreList)
                    this.ActStoreList.Add(act.Copy());
                this.ActPlayingList = new List<Act>();
                foreach (Act act in sp.ActPlayingList)
                    this.ActPlayingList.Add(act.Copy());
            }
        }

        public virtual void Initialize()
        {
            Scale = Vector2.One;
            Color = Color.White;
            Rotation = 0;
            SpriteEffect = SpriteEffects.None;
            Tags = new Dictionary<string, object>();

            isActStoped = false;
            isEnd = false;
            isShown = true;
            isColli = true;

            Owner = null;
            _prePosition = Position;
            _preRotation = Rotation;
            AttachToOwner = new Attach(true, true, false, true);
        }

        public void AddAct(Act act)
        {
            if (ActStoreList == null)
                ActStoreList = new List<Act>();
            if (ActPlayingList == null)
                ActPlayingList = new List<Act>();
            ActStoreList.Add(act);
        }

        public void PauseOrContinueAct(int index)
        {
            ActPlayingList[index].isPause = !ActPlayingList[index].isPause;
        }

        public void PlayAct()
        {
            this.isActStoped = false;
        }

        public void StopAct()
        {
            this.isActStoped = true;
        }

        public void ClearAct()
        {
            if (ActStoreList != null)
                ActStoreList.Clear();
            if (ActPlayingList != null)
                ActPlayingList.Clear();
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
            if (isFirstFrame)
            {
                _prePosition = Position;
                _preRotation = Rotation;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            #region Act
            if (!isActStoped)
            {
                if (ActStoreList != null)
                {
                    while (true)
                    {
                        if (ActStoreList.Count == 0)
                            break;
                        if (ActPlayingList.Count == 0 || !ActPlayingList[ActPlayingList.Count - 1].isSolid)
                        {
                            ActStoreList[0].setSprite(this);
                            ActPlayingList.Add(ActStoreList[0]);
                            ActStoreList.RemoveAt(0);
                        }
                        else
                            break;
                    }
                    foreach (Act act in ActPlayingList)
                    {
                        if (!act.isPause)
                            act.NextStep();
                    }
                    for (int i = 0; i < ActPlayingList.Count; i++)
                        if (ActPlayingList[i].isEnd)
                        {
                            ActPlayingList.RemoveAt(i);
                            i--;
                        }
                }
            }
            #endregion
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            if (isFirstFrame)
                isFirstFrame = false;
            else
            {
                MovedVlaue = Position - _prePosition;
                RotatedValue = Rotation - _preRotation;
                _prePosition = Position;
                _preRotation = Rotation;

                if(Slaves != null)
                    foreach (ASprite slv in Slaves)
                    {
                        if (slv.AttachToOwner.IsPosition)
                            slv.Position += MovedVlaue;
                        if (slv.AttachToOwner.IsRotation && RotatedValue != 0)
                        {
                            slv.Rotation += RotatedValue;
                            slv.Position = Position + (slv.Position - Position).Rotate(RotatedValue);
                        }
                        if (slv.AttachToOwner.IsColor)
                            slv.Color = this.Color;
                        if (slv.AttachToOwner.IsAlpha)
                            slv.Color.A = this.Color.A;
                    }
            }
        }

        public virtual void AllUpdate(GameTime gameTime)
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
            Slaves.Add(slave);
            addSlaveToManger(slave);
        }

        private void addSlaveToManger(ASprite slave)
        {
            if (slave.Manager != null)
                Manager.Remove(slave);
            slave.Manager = this.Manager;
            if (Manager != null)
                Manager.Add(slave);
            if (slave.Slaves != null)
                foreach (ASprite sslave in slave.Slaves)
                    addSlaveToManger(sslave);
        }
    }
}
