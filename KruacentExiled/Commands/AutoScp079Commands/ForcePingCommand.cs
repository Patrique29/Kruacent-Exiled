using CommandSystem;
using Exiled.API.Features;
using KruacentExiled.AutoScp079;
using System;

namespace KruacentExiled.Commands.AutoScp079Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ForcePingCommand : AutoScp079Command
    {
        public override string Command => "forceping";

        public override string[] Aliases => new string[0];

        public override string Description => "force a ping";

        public override string[] Usage => new string[0];

        public override bool Auto079Command(ArraySegment<string> arguments, Player sender, Auto079NPC npc, out string response)
        {
            npc.JobTracker.Ping(npc.CurrentCamera);
            response = "ok";
            return true;
        }
    }
}
