using CommandSystem;
using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint;
using ProjectMER.Commands.ToolGunLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SelectCustomSpawnPoint : KECommand
    {
        public override string Command => "selectcustomspawnpoint";

        public override string[] Aliases => new string[] { "scsp" };

        public override string Description => "Select a custom spawn point to be moved";

        public override string[] Usage => new string[0];


        public static readonly Dictionary<Player, PoseRoomSpawnPointHandler.ItemSpawn> selected = new Dictionary<Player, PoseRoomSpawnPointHandler.ItemSpawn>();

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Player.TryGet(sender,out Player player))
            {
                response = "player not valid";
                return false;
            }

            if(player.CurrentRoom== null)
            {
                response = "player not in room";
                return false;
            }

            RoomType room = player.CurrentRoom.Type;
            Vector3 position = player.Position;

            IEnumerable<PoseRoomSpawnPointHandler.ItemSpawn> spawns = PoseRoomSpawnPointHandler.GetPoseInRoom(room);

            if(spawns.Count() == 0)
            {
                response = "no itemspawn in room";
                return false;
            }

            float distance = float.PositiveInfinity;
            PoseRoomSpawnPointHandler.ItemSpawn current = null;

            foreach (PoseRoomSpawnPointHandler.ItemSpawn spawn in spawns)
            {
                float newDistance = Vector3.Distance(spawn.Position, position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    current = spawn;
                }
            }



            selected[player] = current;


            response = "selected";
            return true;

        }
    }
}
