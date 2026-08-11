using Exiled.API.Features;
using Exiled.API.Features.Roles;
using KruacentExiled.CustomItems.Commands;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.AutoScp079.Jobs.RoomActions
{
    internal sealed class TeslaRoomAction : RoomAction
    {
        internal TeslaRoomAction(Room room,Auto079NPC npc) : base(room,npc)
        {
            players = new List<Player>();

            Exiled.API.Features.TeslaGate.IgnoredTeams.Add(PlayerRoles.Team.SCPs);
        }


        public Exiled.API.Features.TeslaGate Tesla => Room.TeslaGate;

        private List<Player> players;


        public override void OnUpdate()
        {
            if(players.Count != Player.Count)
            {
                players = Player.Enumerable.ToList();
            }

            foreach(Player player in players)
            {
                if (Tesla.IsPlayerInHurtRange(player))
                {
                    Npc.ActivateCurrentTesla();
                    return;
                }
            }

        }
    }
}
