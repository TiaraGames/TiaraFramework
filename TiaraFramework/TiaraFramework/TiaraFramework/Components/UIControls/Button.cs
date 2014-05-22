using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TiaraFramework.Component
{
    public class Button : ASprite
    {
        public Sprite spMoveOff;
        public Sprite spMoveOn;
        public Sprite spPressing;
        public Sprite spPressed;
        public Sprite nowButton;
        bool isLPressed = false;
        bool isRPressed = false;
        bool isOn = false;
        public delegate void ActMoveOff(Button btn);
        public delegate void ActMoveOn(Button btn);
        public delegate void ActLPressing(Button btn);
        public delegate void ActLPressed(Button btn);
        public delegate void ActRPressing(Button btn);
        public delegate void ActRPressed(Button btn);
        public event ActMoveOff MoveOff;
        public event ActMoveOn MoveOn;
        public event ActLPressing LPressing;
        public event ActLPressed LPressed;
        public event ActRPressing RPressing;
        public event ActRPressed RPressed;
        public object tag1;
        public object tag2;
        public object tag3;
        public object tag4;
        public object tag5;
        public bool isEnabled = true;

        public Button(Sprite offMouse, Sprite onMouse, Sprite onPush, Sprite offPush, Game game)
            : base(game)
        {
            this.spMoveOff = offMouse;
            this.spMoveOn = onMouse;
            this.spPressing = onPush;
            this.spPressed = offPush;
            this.nowButton = spMoveOff;
        }

        public static Button SimpleButton(Vector2 position, Texture2D texture, float depth, Game game)
        {
            Sprite spBtn = Sprite.OneFrameSprite(position,texture,depth,game);
            Button btn = new Button(spBtn, spBtn, spBtn, spBtn, game);
            return btn;
        }

        public override void Update(GameTime gameTime)
        {
            if (isEnabled)
                mouseDetection(gameTime);
        }

        public void mouseDetection(GameTime gameTime)
        {
            if (nowButton == spMoveOn && nowButton.isEnd)
                nowButton = spMoveOff;
            if (nowButton == spPressed && nowButton.isEnd)
                nowButton = spMoveOn;

            MouseState ms = Mouse.GetState();
            if (ColliDetection.isPointInRect(new Point(ms.X, ms.Y), nowButton.GetAbsRect()))
            {
                isOn = true;
                nowButton.Rewind();
                nowButton = spMoveOn;
                if (MoveOn != null)
                    MoveOn(this);    // event move on
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    isLPressed = true;
                    nowButton.Rewind();
                    nowButton = spPressing;
                    if (LPressing != null)
                        LPressing(this);  // event L pressing
                }
                else
                    if (isLPressed == true)
                    {
                        isLPressed = false;
                        nowButton.Rewind();
                        nowButton = spPressed;
                        if (LPressed != null)
                            LPressed(this);   // event L pressed
                    }
                if (ms.RightButton == ButtonState.Pressed)
                {
                    isRPressed = true;
                    nowButton.Rewind();
                    nowButton = spPressing;
                    if (RPressing != null)
                        RPressing(this);  // event R pressing
                }
                else
                    if (isRPressed == true)
                    {
                        isRPressed = false;
                        nowButton.Rewind();
                        nowButton = spPressed;
                        if (RPressed != null)
                            RPressed(this);   // event R pressed
                    }
            }
            else
            {
                if (isOn)
                {
                    isOn = false;
                    nowButton.Rewind();
                    nowButton = spMoveOff;
                }
                if (MoveOff != null)
                    MoveOff(this);   // event move off
                isLPressed = false;
                isRPressed = false;
            }

            nowButton.Update(gameTime);
        }
    }
}

