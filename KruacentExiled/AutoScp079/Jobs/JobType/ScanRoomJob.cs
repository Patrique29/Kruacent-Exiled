using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    internal sealed class ScanRoomJob : Job
    {

        public ScanZoneJob ScanZoneJob { get; private set; }

        public static ScanRoomJob CreateJob(ScanZoneJob mainJob)
        {
            ScanRoomJob job = Pool<ScanRoomJob>.Get();

            job.ScanZoneJob = mainJob;

            return job;
        }

        public override string ToString()
        {
            return "Scan room";
        }

        public override void OnStart()
        {
            Camera currentCamera = Auto079NPC.CurrentCamera;
            Room currentRoom = currentCamera.Room;

            IEnumerable<Player> players = Helper.RaycastPlayer(currentCamera, currentRoom,true);
            
            if(ScanZoneJob != null)
            {
                foreach(Player player in players)
                {
                    ScanZoneJob.AddResult(new ScanResult(player, currentRoom));
                    if (Auto079NPC.PingVisibleToScp(player.Position, PingType.Human))
                    {
                        Auto079NPC.RoleNPC.Ping(player.Position, PingType.Human, false);
                    }
                }
                

            }

            EndJob();
        }

        protected override void OnUpdate()
        {
        }


        protected override void OnCleanup()
        {
            Pool<ScanRoomJob>.Return(this);
            base.OnCleanup();
        }
    }
}
