using Exiled.API.Features;
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
        private class QueuePlacement : HintPosition
        {
            public override float Xposition => 0;

            public override float Yposition => 500;

            public override HintAlignment HintAlignment => HintAlignment.Center;
        }
        private class CurrentCameraPlacement : HintPosition
        {
            public override float Xposition => 0;

            public override float Yposition => 200;

            public override HintAlignment HintAlignment => HintAlignment.Center;
        }

        private Auto079NPC _comp;

        public readonly HintPosition queue = new QueuePlacement();
        public readonly HintPosition currentCamera = new CurrentCameraPlacement();
        private AbstractHint queuehint;
        private AbstractHint currentCamerahint;
        private StringBuilder sbQueue;
        private StringBuilder sbCurrentCamera;

        public AutoScp079HumanDisplay(Auto079NPC comp)
        {
            _comp = comp;
            CreateHints();
            sbQueue = StringBuilderPool.Pool.Get();
            sbCurrentCamera = StringBuilderPool.Pool.Get();
        }

        public void CreateHints()
        {
            queuehint = DisplayHandler.Instance.CreateAuto(_comp.HumanPlayer, (args) => TextQueue(), queue.HintPlacement, HintSyncSpeed.Fastest);
            currentCamerahint = DisplayHandler.Instance.CreateAuto(_comp.HumanPlayer, (args) => TextCurrentCamera(), currentCamera.HintPlacement, HintSyncSpeed.Fastest);
        }



        private string TextQueue()
        {
            sbQueue.Clear();
            JobQueue queue = _comp.JobTracker.Queue;

            sbQueue.Append(queue.QueueSize);

            if(queue.QueueSize > 0)
            {
                sbQueue.Append(_comp.JobTracker.Queue.Peek().ToString());
            }


            return sbQueue.ToString();
        }

        private string TextCurrentCamera()
        {
            sbCurrentCamera.Clear();
            Camera current = _comp.CurrentCamera;
            
            if(current == null)
            {
                return " ";
            }


            sbCurrentCamera.Append(current.Name);



            return sbCurrentCamera.ToString();
        }


        public void Destroy()
        {
            StringBuilderPool.Pool.Return(sbQueue);
            StringBuilderPool.Pool.Return(sbCurrentCamera);
            DisplayHandler.Instance.RemoveHint(_comp.HumanPlayer, queuehint);
            DisplayHandler.Instance.RemoveHint(_comp.HumanPlayer, currentCamerahint);
        }
    }

    
}
