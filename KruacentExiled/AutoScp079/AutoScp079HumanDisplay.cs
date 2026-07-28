using Exiled.API.Features.Pools;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.AutoScp079.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079
{
    public class AutoScp079HumanDisplay
    {
        private class Placement : HintPosition
        {
            public override float Xposition => 0;

            public override float Yposition => 500;

            public override HintAlignment HintAlignment => HintAlignment.Center;
        }

        private Auto079NPC _comp;

        public readonly HintPosition hintPosition = new Placement();
        private AbstractHint hint;
        private StringBuilder sb;

        public AutoScp079HumanDisplay(Auto079NPC comp)
        {
            _comp = comp;
            CreateHints();
            sb = StringBuilderPool.Pool.Get();
        }

        public void CreateHints()
        {
            hint = DisplayHandler.Instance.CreateAuto(_comp.HumanPlayer, (args) => Text(), hintPosition.HintPlacement, HintSyncSpeed.Fastest);
        }



        private string Text()
        {
            sb.Clear();
            JobQueue queue = _comp.JobTracker.Queue;

            sb.Append(queue.QueueSize);

            if(queue.QueueSize > 0)
            {
                sb.Append(_comp.JobTracker.Queue.Peek().ToString());
            }


            return sb.ToString();


        }


        public void Destroy()
        {
            StringBuilderPool.Pool.Return(sb);
            DisplayHandler.Instance.RemoveHint(_comp.HumanPlayer, hint);
        }
    }

    
}
