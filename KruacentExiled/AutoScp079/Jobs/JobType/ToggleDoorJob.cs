using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.Utils.API.Features;
using KruacentExiled.Map.Heavy;
using RoundRestarting;
using System;
using System.Linq;

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
            KELog.Debug("try toggle " + Objective.Name);

            if(Objective == null)
            {
                throw new ArgumentNullException(nameof(Objective));
            }

            if (Auto079NPC.CurrentCamera == null)
            {
                KELog.Debug("no cam");
                EndJob();
                return;
            }

            if (Objective.IsLocked)
            {
                KELog.Debug("lcoked");
                EndJob();
                return;
            }

            if(BulkDoor049.Door == Objective.Base && BulkDoor049.DoorRoom == Auto079NPC.CurrentCamera.Room)
            {
                ToggleDoor();
                return;
            }




            if (Auto079NPC.CurrentCamera.Room.Doors.Contains(Objective))
            {
                ToggleDoor();
                return;
            }


            Room room = Room.FindParentRoom(Objective.Base.gameObject);

            if(room != null && room == Auto079NPC.CurrentCamera.Room)
            {
                ToggleDoor();
                return;
            }

            Log.Error("door not poened");
            EndJob();
        }

        private void ToggleDoor()
        {
            KELog.Debug("toggle " + Objective.Name);
            Objective.IsOpen = !Objective.IsOpen;
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
