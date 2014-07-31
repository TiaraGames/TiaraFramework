using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TiaraFramework.Component;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public class ActToward : Act
    {
        Vector2 vecOrient;
        float speed;

        public ActToward(Vector2 VecOrient, float Speed)
            : base(false)
        {
            this.vecOrient = VecOrient.GetNormalize();
            this.speed = Speed;
        }

        internal override void NextStep()
        {
            Sprite.Position += speed * vecOrient;
        }
    }
}
