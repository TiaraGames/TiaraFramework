using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiaraFramework.Component
{
    public static class Tool
    {
        internal static Game Game;

        public static float GetFPS()
        {
            return 1000f / Game.TargetElapsedTime.Milliseconds;
        }

        public static void SetFPS(float FPS)
        {
            Game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / FPS);
        }
    }
}
