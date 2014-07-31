using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public abstract class Act
    {
        public bool isEnd;
        internal bool isSolid;
        internal bool isPause;
        private ASprite _sprite;
        internal ASprite Sprite { get { return _sprite; } set { setSprite(value); } }

        public Act(bool isSolid) { this.isSolid = isSolid; }

        public virtual void Play() { isPause = false; }
        public virtual void Pause() { isPause = true; }
        internal virtual void setSprite(ASprite sprite) { this._sprite = sprite; }
        internal virtual void NextStep() { }

        public virtual Act Copy() { return (Act)this.MemberwiseClone(); }
    }
}
