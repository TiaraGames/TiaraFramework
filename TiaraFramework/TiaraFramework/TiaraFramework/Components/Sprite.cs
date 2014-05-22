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
    public enum LoopStyle { Circulation, Reciprocation }
    public class LoopMode
    {
        public LoopStyle Style;
        public int LoopTime;
        public LoopMode(LoopStyle loopStyle, int time)
        {
            this.Style = loopStyle;
            this.LoopTime = time;
        }
    }

    public class Sprite : ASprite
    {
        int _startIndex;
        int _playFramesNum;

        public Texture2D Texture;
        public Point FrameSize;

        protected Point sheetSize;
        public int CurrentIndex;
        public Point CurrentFrame
        {
            set { CurrentIndex = FrameToIndex(value); }
            get { return IndexToFrame(CurrentIndex); }
        }
        public int OriginIndex;
        public Point OriginFrame
        {
            set { OriginIndex = FrameToIndex(value); }
            get { return IndexToFrame(OriginIndex); }
        }
        public Point PauseFrame;
        public int StartIndex
        {
            set { CurrentIndex = OriginIndex = _startIndex = value; }
            get { return _startIndex; }
        }
        public Point StartFrame
        {
            set { StartIndex = FrameToIndex(value); }
            get { return IndexToFrame(StartIndex); }
        }
        public int PlayFramesNum
        { 
            set 
            {
                int nah = value > 0 ? value -= 1 : value += 1;
                _playFramesNum = value; 
            }
            get { return _playFramesNum + 1; } 
        }
        public int EndIndex
        {
            get 
            {
                if(isPositive)
                    return StartIndex + _playFramesNum; 
                else
                    return StartIndex - _playFramesNum;
            }
        }
        public Point EndFrame
        {
            get { return IndexToFrame(EndIndex); }
        }

        public LoopMode Loop;
        protected int loopedTimes;

        public Rectangle DrawRect;

        public Vector2 LT { get { return new Vector2(0, 0); } }
        public Vector2 CT { get { return new Vector2(this.FrameSize.X / 2, 0); } }
        public Vector2 RT { get { return new Vector2(this.FrameSize.X, 0); } }
        public Vector2 LC { get { return new Vector2(0, this.FrameSize.Y / 2); } }
        public Vector2 CC { get { return new Vector2(this.FrameSize.X / 2, this.FrameSize.Y / 2); } }
        public Vector2 RC { get { return new Vector2(this.FrameSize.X, this.FrameSize.Y / 2); } }
        public Vector2 LB { get { return new Vector2(0, this.FrameSize.Y); } }
        public Vector2 CB { get { return new Vector2(this.FrameSize.X / 2, this.FrameSize.Y); } }
        public Vector2 RB { get { return new Vector2(this.FrameSize.X, this.FrameSize.Y); } }

        public override void Initialize()
        {
            base.Initialize();
            sheetSize = new Point(1, 1);
            Loop = new LoopMode(LoopStyle.Circulation, 0);
            isPositive = true;
            isStopAtLastFrame = false;
            loopedTimes = 0;
        }




        #region CONSTRUCTORS
        protected Sprite(Game game)
            : base(game)
        {
        }

        public Sprite(Vector2 position, Texture2D texture, Point sheetSize, Point frameSize, float depth, Game game)
            : this(game)
        {
            this.Position = position;
            this.Texture = texture;
            this.StartIndex = 0;
            this.sheetSize = sheetSize;
            this.OriginIndex = this.CurrentIndex = 0;
            this.PlayFramesNum = sheetSize.X * sheetSize.Y;
            this.FrameSize = frameSize;
            this.DrawRect = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            this.Depth = depth;
            this.mspf = 1000 / FPS;
            loopedTimes = 1;
        }

        public Sprite(Vector2 position, Texture2D texture, int startIndex, int playFramesNum, Point sheetSize, Point frameSize, float depth, Game game)
            : this(position, texture, sheetSize, frameSize, depth, game)
        {
            if (startIndex + playFramesNum > sheetSize.X * sheetSize.Y || startIndex + playFramesNum < 0)
                throw (new Exception("Frame boundary"));
            this.StartIndex = startIndex;
            this.PlayFramesNum = playFramesNum;
        }

        // Multi-frames Sprite
        /************************************************************************/
        // Single-frame Sprite

        protected Sprite(Vector2 position, Texture2D texture, float depth, Game game)
            : this(game)
        {
            this.Texture = texture;
            this.FrameSize = new Point(texture.Width, texture.Height);
            this.DrawRect = new Rectangle(0, 0, FrameSize.X, FrameSize.Y);
            this.Position = position;
            this.Depth = depth;
            isAniStoped = true;
        }

        protected Sprite(Vector2 position, Texture2D texture, Vector2 origin, float rotation, Color color, float depth, Game game)
            : this(position, texture, depth, game)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Color = color;
        }
        #endregion




        #region GetSingle funcs
        public static Sprite OneFrameSprite(Vector2 position, Texture2D texture, float depth, Game game)
        {
            return new Sprite(position, texture, depth, game);
        }

        public static Sprite OneFrameSprite(Vector2 position, Texture2D texture, Vector2 origin, float rotation, Color color, float depth,
            Game game)
        {
            return new Sprite(position, texture, origin, rotation, color, depth, game);
        }
        #endregion



        
        public override void Update(GameTime gameTime)
        {
            #region Act
            if (!isActStoped)
            {
                if (ActList != null)
                {
                    while (true)
                    {
                        if (ActList.Count == 0)
                            break;
                        if (ActPlayingList.Count == 0 || !ActPlayingList[ActPlayingList.Count - 1].occupyTime)
                        {
                            ActPlayingList.Add(ActList[0]);
                            ActList.RemoveAt(0);
                        }
                        else
                            break;
                    }
                    foreach (Act act in ActPlayingList)
                    {
                        switch (act.actType)
                        {
                            case ActType.MOVEVEC:
                                this.Position += act.TargetVec * act.Speed;
                                break;

                            case ActType.MOVEPOS:
                                if (act.notStop)
                                {
                                    if (act.TargetVec == Vector2.Zero)
                                    {
                                        act.TargetVec = act.TargetPos - this.Position;
                                        act.TargetVec.Normalize();
                                    }
                                    else
                                        this.Position += act.TargetVec * act.Speed;
                                }
                                else
                                {
                                    Vector2 vecOrien = act.TargetPos - this.Position;
                                    if (vecOrien.Length() < act.Speed)
                                    {
                                        this.Position = act.TargetPos;
                                        act.isEnd = true;
                                        break;
                                    }
                                    vecOrien.Normalize();
                                    this.Position += vecOrien * act.Speed;
                                }
                                break;

                            case ActType.MOVEROUND:


                            case ActType.DELAY:
                                if (act.DelayTime <= 0)
                                {
                                    act.isEnd = true;
                                    break;
                                }
                                act.DelayTime -= (int)(1000f / Manager.stageBase.fps);
                                break;

                            case ActType.ADDSP:
                                foreach (Sprite sp in act.spAddList)
                                    Manager.stageBase.SprMgrClct[act.smAdd].Add(sp);
                                break;
                        }
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

            #region Animation
            if (!isAniStoped)
            {
                tslf += gameTime.ElapsedGameTime.Milliseconds;
                if (tslf > mspf)
                {
                    tslf -= mspf;
                    if (isPositive)
                        CurrentIndex++;
                    else
                        CurrentIndex--;
                }

                if (isPositive ? CurrentIndex > OriginIndex + _playFramesNum : CurrentIndex < OriginIndex)
                {
                    loopedTimes++;
                    do
                    {
                        if (Loop.LoopTime != 0 && Loop.LoopTime == loopedTimes)
                        {
                            isAniStoped = true;
                            int nah = isPositive ? CurrentIndex-- : CurrentIndex++;
                            break;
                        }

                        switch (Loop.Style)
                        {
                            case LoopStyle.Circulation:
                                CurrentIndex = OriginIndex;
                                break;
                            case LoopStyle.Reciprocation:
                                int nah = isPositive ? CurrentIndex-=2 : CurrentIndex+=2;
                                isPositive = !isPositive;
                                break;
                        }                            
                    } while (false);
                }
            }
            #endregion
        }




        #region private funcs
        void resetFrame()
        {
            if(EndIndex<StartIndex)
            {
                // isPositive=
            }
        }
        #endregion




        #region public funcs

        public int FrameToIndex(Point Frame)
        {
            return Frame.X + Frame.Y * sheetSize.X;
        }

        public Point IndexToFrame(int Index)
        {
            Point pos = new Point();
            pos.X = Index % sheetSize.X;
            pos.Y = Index / sheetSize.X;
            return pos;
        }

        /// <summary>
        /// Can be only used on Single Frame Sprite.
        /// </summary>
        /// <param name="ttNew">The new texture</param>
        public void ChangeTexture(Texture2D ttNew, bool changeDrawRect)
        {
            this.Texture = ttNew;
            if (changeDrawRect)
            {
                this.DrawRect.Width = ttNew.Width;
                this.DrawRect.Height = ttNew.Height;
            }
            this.FrameSize = new Point(ttNew.Width, ttNew.Height);
        }

        public void PlayAnime()
        {
            this.isAniStoped = false;
        }

        public void PlayAct()
        {
            this.isActStoped = false;
        }

        public void StopAnime()
        {
            this.isAniStoped = true;
        }

        public void StopAnimeAt(int index)
        {
            StopAnime();
            CurrentIndex = index;
        }

        public void StopAnimeAt(Point frame)
        {
            StopAnime();
            CurrentFrame = frame;
        }

        public void StopAct()
        {
            this.isActStoped = true;
        }

        public void ClearActs()
        {
            ActList.Clear();
            ActPlayingList.Clear();
        }

        public void Rewind()
        {
            CurrentFrame = OriginFrame;
        }

        public void SetFPS(int FPS)
        {
            this.FPS = FPS;
            this.mspf = 1000 / FPS;
        }

        public void AddAction(Act act)
        {
            if (ActList == null)
                ActList = new List<Act>();
            if (ActPlayingList == null)
                ActPlayingList = new List<Act>();
            ActList.Add(act);
        }

        public ASprite CopyBase()
        {
            Sprite sp = (Sprite)this.MemberwiseClone();
            sp.Loop = new LoopMode(this.Loop.Style, this.Loop.LoopTime);
            if (sp.Slaves != null)
            {
                sp.Slaves = new List<ASprite>();
                foreach (Sprite slv in this.Slaves)
                    sp.Slaves.Add(slv.Copy());
            }
            if (this.ActList != null)
            {
                sp.ActList = new List<Act>();
                foreach (Act act in this.ActList)
                    sp.ActList.Add(act.Copy());
                sp.ActPlayingList = new List<Act>();
                foreach (Act act in this.ActPlayingList)
                    sp.ActPlayingList.Add(act.Copy());
            }
            return sp;
        }

        public Sprite Copy()
        {
            return (Sprite)this.CopyBase();
        }

        public Rectangle GetRect()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, FrameSize.X, FrameSize.Y);
        }

        public Rectangle GetAbsRect()
        {
            return new Rectangle(
                (int)Manager.smPosition.X + (int)Position.X - (int)(Origin.X * Scale.X),
                (int)Manager.smPosition.Y + (int)Position.Y - (int)(Origin.Y * Scale.Y),
                (int)(DrawRect.Width * Scale.X),
                (int)(DrawRect.Height * Scale.Y));
        }

        public Rectangle GetDrawRect()
        {
            return new Rectangle(
                CurrentFrame.X * FrameSize.X + DrawRect.X, 
                CurrentFrame.Y * FrameSize.Y + DrawRect.Y, 
                DrawRect.Width, 
                DrawRect.Height);
        }

        public Rectangle GetColliRect()
        {
            if (ColliRect == Rectangle.Empty)
                return new Rectangle(
                    (int)Position.X - (int)Origin.X + ColliOffset.X, 
                    (int)Position.Y - (int)Origin.Y + ColliOffset.Y, 
                    FrameSize.X - ColliOffset.X * 2, FrameSize.Y - ColliOffset.Y * 2);
            else
                return new Rectangle(
                    (int)(Manager.smPosition.X + Position.X - Origin.X + ColliRect.X), 
                    (int)(Manager.smPosition.Y + Position.Y - Origin.Y + ColliRect.Y), 
                    ColliRect.Width, ColliRect.Height);
        }

        public Vector2 GetAbsPosition()
        {
            return new Vector2(Position.X + Manager.smPosition.X, Position.Y + Manager.smPosition.Y);
        }
        #endregion
    }
}