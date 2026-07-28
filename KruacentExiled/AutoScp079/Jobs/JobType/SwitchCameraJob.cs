using Exiled.API.Features;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    internal sealed class SwitchCameraJob : Job
    {


        public Camera Objective { get; private set; }


        public static SwitchCameraJob CreateJob(Camera objective)
        {
            SwitchCameraJob job = Pool<SwitchCameraJob>.Get();

            //KELog.Debug("creating job " + objective.Name);
            job.Objective = objective;
            return job;
        }

        public SwitchCameraJob()
        {

        }

        protected override void OnUpdate()
        {
            

        }


        protected override void OnStart()
        {
            KELog.Debug("switching camera to " + Objective.Name + " curr = " + Auto079NPC.CurrentCamera.Name);

            if (Auto079NPC.CurrentCamera != Objective)
            {
                Auto079NPC.RoleNPC.Camera = Objective;

                //todo cost calc
                
            }
            EndJob();
        }


        protected override void OnCleanup()
        {
            Objective = null;
            Pool<SwitchCameraJob>.Return(this);
        }


        public override bool Equals(Job other)
        {
            if (other == null) return false;
            if(other is SwitchCameraJob job)
            {
                return job.Objective == Objective;
            }
            return false;
        }

        public override string ToString()
        {
            return "Switch to " + Objective?.Name;
        }
    }
}
