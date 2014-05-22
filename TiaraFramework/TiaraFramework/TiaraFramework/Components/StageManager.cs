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
using TiaraFramework.Stages;

namespace TiaraFramework.Component
{
    // 每新建一个Stage就需要在这里添加一个索引
    public enum StageIndex { Null, Stage_Blank };

    public class StageManager : Microsoft.Xna.Framework.GameComponent
    {
        StageIndex nowStage;
        StageIndex nextStage;

        public StageManager(Game game)
            : base(game)
        {
            nowStage = StageIndex.Null;
            nextStage = StageIndex.Stage_Blank ;  // 这个是启动游戏后第一个出现的Stage
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (nowStage != nextStage)
            {
                if (Game.Components.Count >= 2)
                    Game.Components.RemoveAt(1);
                switch (nextStage)  // 这里是索引向舞台实例的转换，按Stage1的格式复制即可。60f是该舞台的帧频
                {
                    case StageIndex.Stage_Blank:
                        Game.Components.Insert(1, new Stage_Blank(Game, 60f));
                        break;
                }
                nowStage = nextStage;
            }
            // Remove old stage and add new stage

            Stage temp = (Stage)Game.Components[1];
            if (temp.isFinished == true)
                nextStage = temp.nextStage;
            // Ready to change  stage
        }
    }
}