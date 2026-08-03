using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.AutoScp079.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Displays
{
    internal class QueueDisplay : Auto079Display
    {
        public QueueDisplay(Auto079NPC auto079NPC) : base(auto079NPC)
        {
            HintPosition = new QueuePlacement();
            CreateHint();
        }

        protected override string Text()
        {
            sb.Clear();
            JobQueue queue = Auto079NPC.JobTracker.Queue;

            sb.Append(queue.QueueSize);

            if (queue.QueueSize > 0)
            {
                sb.Append(Auto079NPC.JobTracker.Queue.Peek().ToString());
            }


            return sb.ToString();

        }
    }

    public class QueuePlacement : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 500;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
