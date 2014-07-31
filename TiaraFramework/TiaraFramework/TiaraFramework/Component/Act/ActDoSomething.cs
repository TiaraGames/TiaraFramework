using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiaraFramework.Component.Extend;

namespace TiaraFramework.Component
{
    public class ActDoSomething : Act
    {
        public delegate void func();
        event func evtFuncs;

        public ActDoSomething(params func[] funcs)
            : base(false)
        {
            foreach (func f in funcs)
                evtFuncs += f;
        }

        internal override void NextStep()
        {
            evtFuncs();
            this.isEnd = true;
        }
    }
}
