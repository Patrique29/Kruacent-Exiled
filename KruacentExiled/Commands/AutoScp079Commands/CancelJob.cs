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
    internal class CancelJob : AutoScp079Command
    {


        public override string Command => "canceljob";

        public override string[] Aliases => new string[] { "cj" };

        public override string Description => "cancel job";

        public override string[] Usage => new string[0];


        public override bool Auto079Command(ArraySegment<string> arguments, Player sender, Auto079NPC npc, out string response)
        {
            npc.CancelCurrentJob();

            response = "ok";
            return true;
        }


    }
}
