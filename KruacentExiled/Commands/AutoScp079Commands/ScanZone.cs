using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.AutoScp079;
using KruacentExiled.AutoScp079.Jobs.JobType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.AutoScp079Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ScanZone : KECommand
    {


        public override string Command => "scanzone";

        public override string[] Aliases => new string[] { "sz" };

        public override string Description => "Scan the current zone of the auto079";

        public override string[] Usage => new string[0];


        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Auto079NPC npc = null;
            Player player = Player.Get(sender);
            if(player == null)
            {
                response = "wrong player";
                return false;
            }

            foreach (Auto079NPC auto in Auto079NPC.Instances)
            {
                if(auto.HumanPlayer == player)
                {
                    npc = auto;
                    break;
                }
            }

            if(npc == null)
            {
                response = "no auto 079 found";
                return false;
            }

            npc.JobTracker.AddJob(ScanZoneJob.CreateJob(player.Zone));


            response = "ok";
            return true;
        }


    }
}
