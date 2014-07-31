using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    class ActFade : Act
    {
        int targetAlpha;
        int frames;
        float step;
        float trueAlpha;
        bool isRelative;

        public ActFade(int TargetAlpha, float Time, bool isSolid = true, bool isRelative = false)
            : base(isSolid)
        {
            if (Time <= 0)
                throw (new Exception("Time must bigger than 0."));
            this.targetAlpha = TargetAlpha;
            this.frames = (int)Math.Round(Time * Tool.GetFPS());
            this.isRelative = isRelative;
        }

        internal override void setSprite(ASprite sprite)
        {
            base.setSprite(sprite);
            trueAlpha = Sprite.Color.A;
            step = (isRelative ? targetAlpha : targetAlpha - Sprite.Color.A) / (float)frames;
        }

        internal override void NextStep()
        {
            if (frames-- > 0)
            {
                trueAlpha += step;
                Sprite.Color.A = (byte)(Math.Round(trueAlpha) < 0 ? 0 : Math.Round(trueAlpha));
            }
            else
                isEnd = true;
        }
    }
}
