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
        public bool isFinished;
        public StageIndex NextStage;
        public Dictionary<string, SpriteManager> SprMgrClct;
        public Window Window;
        public Color BackgoundColor = new Color(40, 40, 40);
        protected Rectangle windowBounds;
        protected long deltaFrames;
        protected long framesTimer;
        protected bool enable;
        private SpriteBatch spriteBatch;
        // Input
        protected MouseState MouseState;
        protected Point MousePosP { get { return new Point(MouseState.X, MouseState.Y); } }
        protected Vector2 MousePosV { get { return new Vector2(MouseState.X, MouseState.Y); } }
        protected Vector2 MouseLastPos;
        protected Vector2 MouseMove;
        private int[] mousePressedFrame = new int[] { 0, 0, 0 };
        protected MouseButton MouseButton;
        protected int ScrollWheelLastValue;
        protected int ScrollWheelChange;
        protected KeyboardState KeyboardState;

        public Stage(Game game, float fps)
            : base(game)
        {
            this.windowBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            Tool.SetFPS(fps);
            this.enable = true;
            this.isFinished = false;

            this.SprMgrClct = new Dictionary<string, SpriteManager>();
            SprMgrClct.Add("Player", new SpriteManager(Game, this));
            SprMgrClct.Add("Enemy", new SpriteManager(Game, this));
            SprMgrClct.Add("UI", new SpriteManager(Game, this));
            SprMgrClct.Add("Map", new SpriteManager(Game, this));

            this.Window = new Window(game);
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(ResizeWindowBounds);

            this.MouseMove = Vector2.Zero;
            this.MouseLastPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            this.ScrollWheelLastValue = Mouse.GetState().ScrollWheelValue;
            this.MouseButton = new MouseButton();

            Pixel.Init(game);
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
            this.NextStage = nextStage;
        }

        #region Timers
        protected long SetTimer(double interval)
        {
            return framesTimer += (long)(interval * Tool.GetFPS());
        }

        protected long SetTimer(int minites, double seconds)
        {
            return framesTimer += (long)((minites * 60 + seconds) * Tool.GetFPS());
        }

        protected long SetTempTimer(double interval)
        {
            return framesTimer + (long)(interval * Tool.GetFPS());
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
            return (int)(interval * Tool.GetFPS());
        }
        #endregion

        public void AddSpriteManager(string name)
        {
            SprMgrClct.Add(name, new SpriteManager(Game, this));
        }

        public void MoveSMs(Vector2 moveVec, SpriteManager[] SMs)
        {
            foreach (SpriteManager sm in SMs)
                sm.MgrPosition += moveVec;
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

            // MouseButton
            ButtonState[] bss = new ButtonState[] { MouseState.LeftButton, MouseState.RightButton, MouseState.MiddleButton };
            for (int i = 0; i < 3; i++)
            {
                MouseButton[i].IsPressed = false;
                if (bss[i] == ButtonState.Pressed)
                {
                    if (++mousePressedFrame[i] == 1)
                        MouseButton[i].IsClick = true;
                    else
                        MouseButton[i].IsClick = false;
                    MouseButton[i].IsPressing = true;
                }
                else
                {
                    if (mousePressedFrame[i] != 0)
                        MouseButton[i].IsPressed = true;
                    mousePressedFrame[i] = 0;
                    MouseButton[i].IsClick = false;
                    MouseButton[i].IsPressing = false;
                }
            }
        }

        void ResizeWindowBounds(object sender, EventArgs e)
        {
            windowBounds = Game.Window.ClientBounds;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Game.GraphicsDevice.Clear(BackgoundColor);
            List<ASprite> liNeedDraw = new List<ASprite>();

            spriteBatch.Begin(SpriteSortMode.FrontToBack,BlendState.NonPremultiplied);
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
                    }
                    else if (asp is Button)
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
                    else if (asp is Label)
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

        private void collectAllASprite(ASprite source, List<ASprite> target)
        {
            target.Add(source);
            if (source.Slaves != null && source.isShown)
                for (int i = 0; i < source.Slaves.Count; i++)
                    collectAllASprite(source.Slaves[i], target);
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

    public class MouseButton
    {
        public class MouseButtonState
        {
            public bool IsClick = false;
            public bool IsPressing = false;
            public bool IsPressed = false;
        }
        public MouseButtonState L { get; internal set; }
        public MouseButtonState R { get; internal set; }
        public MouseButtonState M { get; internal set; }
        public MouseButtonState this[int index]
        {
            get
            {
                index %= 3;
                if (index == 0)
                    return L;
                else if (index == 1)
                    return R;
                else
                    return M;
            }
        }
        public MouseButton()
        {
            L = new MouseButtonState();
            R = new MouseButtonState();
            M = new MouseButtonState();
        }
    }
}