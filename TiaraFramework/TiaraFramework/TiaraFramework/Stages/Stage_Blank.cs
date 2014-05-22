// 新建Stage时只要复制这个里的内容就OK了

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
using TiaraFramework.Component;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Stages
{
    class Stage_Blank : Stage
    {
        public Stage_Blank(Game game, float fps)
            : base(game, fps)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }
    }
}
