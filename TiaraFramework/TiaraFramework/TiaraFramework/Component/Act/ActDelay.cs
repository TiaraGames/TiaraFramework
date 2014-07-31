using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public class ActDelay: Act
    {
        int frames;

        public ActDelay(float Time)
            : base(true)
        {
            frames = (int)Math.Round(Time * Tool.GetFPS());
        }

        internal override void NextStep()
        {
            if (frames-- <= 0)
                this.isEnd = true;
        }
    }
}
