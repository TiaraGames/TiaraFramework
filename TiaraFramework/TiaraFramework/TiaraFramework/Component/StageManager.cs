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
    // Each stage should be manually added an index here
    public enum StageIndex { Null, Blank};

    public class StageManager : Microsoft.Xna.Framework.GameComponent
    {
        StageIndex nowStage;
        StageIndex nextStage;

        public StageManager(Game game, StageIndex firstStage = StageIndex.Null)
            : base(game)
        {
            nowStage = StageIndex.Null;
            if (firstStage == StageIndex.Null)
                nextStage = StageIndex.Blank;  // Default value of the first stage
            else
                nextStage = firstStage;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Remove old stage and add new stage
            if (nowStage != nextStage)
            {
                if (Game.Components.Count >= 2)
                    Game.Components.RemoveAt(1);
                switch (nextStage)  // Instantiate stage according to the index
                {
                    case StageIndex.Blank:
                        Game.Components.Insert(1, new Stage_Blank(Game, 60f));
                        break;
                }
                nowStage = nextStage;
            }

            // Ready to change stage
            Stage temp = (Stage)Game.Components[1];
            if (temp.isFinished == true)
                nextStage = temp.NextStage;
        }
    }
}