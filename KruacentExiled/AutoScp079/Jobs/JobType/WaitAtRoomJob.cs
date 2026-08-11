using Exiled.API.Extensions;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    internal class WaitAtRoomJob : CompositeJob,IInfiniteJob
    {
        public Room WaitingAt { get; set; }


        protected override void AttributeJobs()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = true;
            if (!WaitingAt.Cameras.Contains(Auto079NPC.CurrentCamera))
            {
                AddJob(SwitchCameraJob.CreateJob(WaitingAt.Cameras.GetRandomValue()));
            }
            AddJob(WaitWhileAction.CreateJob());
        }

        public static WaitAtRoomJob CreateJob(Room waitingAt)
        {
            WaitAtRoomJob job = Pool<WaitAtRoomJob>.Get();
            job.WaitingAt = waitingAt;

            return job;
        }



        protected override void OnCleanup()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = false;
            base.OnCleanup();
        }


        public override string ToString()
        {
            return "Waiting at " + WaitingAt.Name;
        }

        
    }
}
