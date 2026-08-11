using Exiled.API.Features.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    public sealed class ToggleDoorJob : Job
    {
        public Door Objective { get; private set; }



        public static ToggleDoorJob CreateJob(Door objective)
        {
            if (objective.IsElevator) throw new ArgumentException("elevator door not compatible");

            ToggleDoorJob job = Pool<ToggleDoorJob>.Get();

            //KELog.Debug("creating job " + objective.Name);
            job.Objective = objective;
            return job;
        }


        public override void OnStart()
        {
            if(Auto079NPC.CurrentCamera == null)
            {
                EndJob();
                return;
            }

            if (Objective.IsLocked)
            {
                EndJob();
                return;
            }

            if (Auto079NPC.CurrentCamera.Room.Doors.Contains(Objective))
            {
                Objective.IsOpen = !Objective.IsOpen;
            }
            EndJob();
        }


        public override string ToString()
        {
            return "Open door";
        }

        protected override void OnCleanup()
        {
            Objective = null;
            Pool<ToggleDoorJob>.Return(this);
        }

    }
}
