using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.AutoScp079;
using KruacentExiled.AutoScp079.Abilities;
using KruacentExiled.AutoScp079.Jobs.JobType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.AutoScp079Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ScanZone : AutoScp079Command
    {


        public override string Command => "scanzone";

        public override string[] Aliases => new string[] { "sz" };

        public override string Description => "Scan the current zone of the auto079";

        public override string[] Usage => new string[0];


        public override bool Auto079Command(ArraySegment<string> arguments, Player sender, Auto079NPC npc, out string response)
        {

            AbilityBase.Get<ScanZoneAbility>().Start(npc);

            response = "ok";
            return true;
        }


    }
}
