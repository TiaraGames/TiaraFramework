using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace TiaraFramework.Component
{
    public class Stage : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        protected Rectangle windowBounds;
        public bool isFinished;
        public StageIndex nextStage;
        public float fps;
        protected long deltaFrames;
        protected long framesTimer;
        protected bool enable;
        public Dictionary<string, SpriteManager> SprMgrClct;
        public Window Window;
        public Color BackgoundColor = new Color(40, 40, 40);
        // Input
        protected MouseState MouseState;
        protected Point MousePosP { get { return new Point(MouseState.X, MouseState.Y); } }
        protected Vector2 MousePosV { get { return new Vector2(MouseState.X, MouseState.Y); } }
        protected Vector2 MouseLastPos;
        protected Vector2 MouseMove;
        protected int ScrollWheelLastValue;
        protected int ScrollWheelChange;
        protected KeyboardState KeyboardState;
        protected bool isMouseLBDown = false;
        protected bool isMouseRBDown = false;

        public Stage(Game game, float fps)
            : base(game)
        {
            this.windowBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            this.fps = fps;
            this.enable = true;
            this.isFinished = false;

            this.SprMgrClct = new Dictionary<string, SpriteManager>();
            SprMgrClct.Add("Player", new SpriteManager(Game, this));
            SprMgrClct.Add("Enemy", new SpriteManager(Game, this));
            SprMgrClct.Add("UI", new SpriteManager(Game, this));

            this.Window = new Window(game);
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(ResizeWindowBounds);

            this.MouseMove = Vector2.Zero;
            this.MouseLastPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            this.ScrollWheelLastValue = Mouse.GetState().ScrollWheelValue;
        }

        public override void Initialize()
        {
            base.Initialize();
            deltaFrames = 0;
            framesTimer = 0;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        protected void ChangeStage(StageIndex nextStage)
        {
            SprMgrClct.Clear();
            isFinished = true;
            this.nextStage = nextStage;
        }

        #region Timers
        protected long SetTimer(double interval)
        {
            return framesTimer += (long)(interval * fps);
        }

        protected long SetTimer(int minites, double seconds)
        {
            return framesTimer += (long)((minites * 60 + seconds) * fps);
        }

        protected long SetTempTimer(double interval)
        {
            return framesTimer + (long)(interval * fps);
        }

        protected void SetTimerWithFrame(int frame)
        {
            framesTimer += frame;
        }

        protected void ClearTimer()
        {
            framesTimer = 0;
        }

        protected int TimeToFrames(double interval)
        {
            return (int)(interval * fps);
        }
        #endregion

        public void AddSpriteManager(string name)
        {
            SprMgrClct.Add(name, new SpriteManager(Game, this));
        }

        public void MoveSMs(Vector2 moveVec, SpriteManager[] SMs)
        {
            foreach (SpriteManager sm in SMs)
                sm.smPosition += moveVec;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            deltaFrames++;
            foreach (KeyValuePair<string, SpriteManager> pair in SprMgrClct)
                pair.Value.Update(gameTime);

            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            MouseMove = MousePosV - MouseLastPos;
            MouseLastPos = MousePosV;
            ScrollWheelChange = MouseState.ScrollWheelValue - ScrollWheelLastValue;
            ScrollWheelLastValue = MouseState.ScrollWheelValue;
        }

        void ResizeWindowBounds(object sender, EventArgs e)
        {
            windowBounds = Game.Window.ClientBounds;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Game.GraphicsDevice.Clear(BackgoundColor);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            foreach (KeyValuePair<string, SpriteManager> pair in SprMgrClct)
                foreach (ASprite asp in pair.Value.spriteList)
                {
                    if (asp is Sprite && asp.isShown)
                    {
                        Sprite s = (Sprite)asp;
                            spriteBatch.Draw(
                                s.Texture,
                                s.GetAbsPosition(),
                                s.GetDrawRect(),
                                s.Color,
                                s.Rotation,
                                s.Origin,
                                s.Scale,
                                s.SpriteEffect,
                                s.Depth);
                        if (s.Slaves != null && s.isShown)
                            foreach (Sprite slave in s.Slaves)
                            {
                                if (slave.isShown)
                                    spriteBatch.Draw(
                                        slave.Texture,
                                        slave.GetAbsPosition(),
                                        slave.GetDrawRect(),
                                        slave.Color,
                                        slave.Rotation,
                                        slave.Origin,
                                        slave.Scale,
                                        slave.SpriteEffect,
                                        slave.Depth);
                            }
                    }
                    if (asp is Button)
                    {
                        Sprite s = ((Button)asp).nowButton;
                        spriteBatch.Draw(
                                s.Texture,
                                s.GetAbsPosition(),
                                s.GetDrawRect(),
                                asp.Color,
                                asp.Rotation,
                                asp.Origin,
                                asp.Scale,
                                asp.SpriteEffect,
                                asp.Depth);
                    }
                    if (asp is Label)
                    {
                        Label l = (Label)asp;
                        spriteBatch.DrawString(
                            l.SpriteFont,
                            l.Text,
                            l.Position,
                            l.Color,
                            l.Rotation,
                            l.Origin,
                            l.Scale,
                            l.SpriteEffect,
                            l.Depth);
                    }
                }
            spriteBatch.End();
        }
    }

    public class Window
    {
        public GameWindow window;
        public Vector2 LT { get { return new Vector2(0, 0); } }
        public Vector2 CT { get { return new Vector2((float)window.ClientBounds.Width / 2f, 0); } }
        public Vector2 RT { get { return new Vector2((float)window.ClientBounds.Width, 0); } }
        public Vector2 LC { get { return new Vector2(0, 0); } }
        public Vector2 CC { get { return new Vector2((float)window.ClientBounds.Width / 2f, (float)window.ClientBounds.Height / 2f); } }
        public Vector2 RC { get { return new Vector2((float)window.ClientBounds.Width, (float)window.ClientBounds.Height / 2); } }
        public Vector2 LB { get { return new Vector2(0, 0); } }
        public Vector2 CB { get { return new Vector2((float)window.ClientBounds.Width / 2f, (float)window.ClientBounds.Height); } }
        public Vector2 RB { get { return new Vector2((float)window.ClientBounds.Width, (float)window.ClientBounds.Height); } }

        public Window(Game game)
        {
            window = game.Window;
        }
    }
}