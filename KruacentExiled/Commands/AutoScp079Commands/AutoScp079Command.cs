using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.AutoScp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.AutoScp079Commands
{
    
    internal abstract class AutoScp079Command : KECommand
    {

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Auto079NPC npc = null;
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "wrong player";
                return false;
            }

            foreach (Auto079NPC auto in Auto079NPC.Instances)
            {
                if (auto.HumanPlayer == player)
                {
                    npc = auto;
                    break;
                }
            }

            if (npc == null)
            {
                response = "no auto 079 found";
                return false;
            }

            return Auto079Command(arguments, player, npc, out response);
        }

        public abstract bool Auto079Command(ArraySegment<string> arguments,Player sender, Auto079NPC npc, out string response);
    }
}
