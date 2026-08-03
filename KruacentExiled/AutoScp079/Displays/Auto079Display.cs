using Exiled.API.Features.Pools;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Displays
{
    public abstract class Auto079Display
    {

        private AbstractHint hint;
        protected HintPosition HintPosition;

        protected StringBuilder sb { get; }

        private Auto079NPC _comp;

        protected Auto079NPC Auto079NPC => _comp;

        internal Auto079Display(Auto079NPC auto079NPC)
        {
            _comp = auto079NPC;

            sb = StringBuilderPool.Pool.Get();
            
        }


        protected void CreateHint()
        {
            hint = DisplayHandler.Instance.CreateAuto(_comp.HumanPlayer, (args) => Text(), HintPosition.HintPlacement, HintSyncSpeed.Fastest);
        }


        protected abstract string Text();


        public void Destroy()
        {
            DisplayHandler.Instance.RemoveHint(_comp.HumanPlayer, hint);
            StringBuilderPool.Pool.Return(sb);
        }


        private static List<Auto079Display> _displays = new List<Auto079Display>();

        public static IReadOnlyCollection<Auto079Display> Displays => _displays;

        public static void CreateDisplay(Auto079NPC comp)
        {
            _displays.Add(new QueueDisplay(comp));
            _displays.Add(new PowerDisplay(comp));
            _displays.Add(new CurrentCameraDisplay(comp));
        }


        public static void DestroyAll()
        {
            foreach(Auto079Display display in Displays)
            {
                display.Destroy();
            }
        }
    }
}
