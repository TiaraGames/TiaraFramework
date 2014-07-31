using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public class ActDye : Act
    {
        Vector3 targetColor;
        Vector3 trueColor;
        Vector3 step;
        int frames;
        bool isRelative;

        public ActDye(Color TargetColor, float Time, bool isSolid = true, bool isRelative = false)
            : base(isSolid)
        {
            this.targetColor = TargetColor.ToVector3();
            this.frames = (int)(Time * Tool.GetFPS());
            this.isRelative = isRelative;
        }

        internal override void setSprite(ASprite sprite)
        {
            base.setSprite(sprite);
            step = (targetColor - sprite.Color.ToVector3()) / frames;
            trueColor = sprite.Color.ToVector3();
        }

        internal override void NextStep()
        {
            base.NextStep();
            if (frames-- > 0)
            {
                trueColor += step;
                Sprite.Color = new Color(trueColor.X, trueColor.Y, trueColor.Z, Sprite.Color.A / 255f);
            }
            else
                isEnd = true;
        }
    }
}
