using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiaraFramework.Component
{
    public enum ActType { MOVEVEC, MOVEPOS, MOVEROUND, DELAY, ADDSP };
    public class Act
    {
        internal ActType actType { get; private set; }
        public Vector2 TargetVec;
        public Vector2 TargetPos;
        public Vector2 O;
        public float Angle;
        public bool isCW;
        public float Speed;
        public int DelayTime;
        internal bool occupyTime;
        public List<ASprite> spAddList;
        public string smAdd;
        internal bool isEnd;
        internal bool notStop;

        public Act MoveVec(Vector2 TargetVec, float Speed)
        {
            this.actType = ActType.MOVEVEC;
            this.TargetVec = TargetVec;
            this.TargetVec.Normalize();
            this.Speed = Speed;
            this.occupyTime = false;
            this.isEnd = false;

            return this;
        }

        public Act MovePos(Vector2 TargetPos, float Speed, bool occupyTime, bool notStop)
        {
            this.actType = ActType.MOVEPOS;
            this.TargetPos = TargetPos;
            this.Speed = Speed;
            this.occupyTime = occupyTime;
            this.isEnd = false;
            this.notStop = notStop;

            return this;
        }

        public Act MoveRound(Vector2 O, float angle, float time, bool isCW, bool occupyTime)
        {
            this.O = O;
            this.Angle = angle;
            this.isCW = isCW;
            this.occupyTime = occupyTime;

            return this;
        }

        public Act Delay(int DelayTime)
        {
            this.actType = ActType.DELAY;
            this.DelayTime = DelayTime;
            this.occupyTime = true;
            this.isEnd = false;

            return this;
        }

        public Act AddSP(List<ASprite> sprites, string SM)
        {
            this.actType = ActType.ADDSP;
            this.spAddList = sprites;
            this.smAdd = SM;
            this.occupyTime = false;
            this.isEnd = true;

            return this;
        }

        public Act Copy()
        {
            Act act = (Act)this.MemberwiseClone();
            if (this.spAddList != null)
            {
                act.spAddList = new List<ASprite>();
                foreach (Sprite sp in spAddList)
                {
                    act.spAddList.Add(sp.Copy());
                }
            }
            return act;
        }
    }
}
