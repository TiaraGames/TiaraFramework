using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public class ActArrive : Act
    {
        Vector2 target;
        Vector2 vecOrient;
        int frames;
        float nowSpeed;
        bool isRelative;

        float speedUpRate;
        float speedDownRate;
        int speedUpFrames;
        int speedDownFrames;
        int speedMaxFrames;
        float speedUpStep;
        float speedDownStep;

        public ActArrive(Vector2 Target, float Time, bool isSolid = true, bool isRelative = false, float SpeedUpRate = 0, float SpeedDownRate = 0)
            : base(isSolid)
        {
            if (SpeedDownRate + SpeedUpRate > 1)
                throw (new Exception("In ActArrive the sum of two Rate must lower than 1."));
            if (SpeedUpRate < 0 || SpeedDownRate < 0)
                throw (new Exception("ActArrive's Rate must bigger than or equals to 0."));
            if (Time <= 0)
                throw (new Exception("ActArrive's Time must bigger than 0."));
            this.target = Target;
            this.frames = (int)(Time * Tool.GetFPS());
            this.isRelative = isRelative;
            this.speedUpRate = SpeedUpRate;
            this.speedDownRate = SpeedDownRate;
        }

        internal override void setSprite(ASprite sprite)
        {
            base.setSprite(sprite);
            this.target = isRelative ? Sprite.Position + this.target : this.target;
            vecOrient = (target - sprite.Position).GetNormalize();
            float length = (target - sprite.Position).Length();

            speedUpFrames = (int)Math.Round(frames * speedUpRate);
            speedDownFrames = (int)Math.Round(frames * speedDownRate);
            speedMaxFrames = (int)Math.Round(frames * (1 - speedUpRate - speedDownRate));
            float maxSpeed = length * 2 / ((2 - speedUpRate - speedDownRate) * frames);

            speedUpStep = speedUpFrames == 0 ? 0 : maxSpeed / speedUpFrames;
            speedDownStep = speedDownFrames == 0 ? 0 : maxSpeed / speedDownFrames;

            if (speedUpStep == 0)
                nowSpeed = maxSpeed;
            else
                nowSpeed = 0;
        }

        internal override void NextStep()
        {
            // speed up
            if (speedUpFrames != 0)
            {
                speedUpFrames--;
                nowSpeed += speedUpStep;
                Sprite.Position += nowSpeed * vecOrient;
            }
            // constant speed
            else if (speedMaxFrames != 0)
            {
                speedMaxFrames--;
                Sprite.Position += nowSpeed * vecOrient;
            }
            // speed down
            else if (speedDownFrames != 0)
            {
                speedDownFrames--;
                nowSpeed -= speedDownStep;
                Sprite.Position += nowSpeed * vecOrient;
            }
            // arrived
            else
                this.isEnd = true;
        }
    }
}
