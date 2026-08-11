using Exiled.API.Features;
using KruacentExiled.AutoScp079.Jobs.RoomActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{

    /// <summary>
    /// Waits at current position while doing the action of the room
    /// </summary>
    internal class WaitWhileAction : Job
    {



        private RoomAction RoomAction;
        public static WaitWhileAction CreateJob()
        {
            WaitWhileAction job = Pool<WaitWhileAction>.Get();
            return job;
        }

        public override void OnStart()
        {
            RoomAction = RoomAction.GetAction(Auto079NPC.CurrentCamera.Room,Auto079NPC);
            if(RoomAction != null)
            {
                Log.Info("found action in " + Auto079NPC.CurrentCamera.Room.Name);
            }
            

        }

        protected override void OnUpdate()
        {
            RoomAction?.OnUpdate();
        }

        

        public override string ToString()
        {
            return "Wait while action";
        }
    }
}
