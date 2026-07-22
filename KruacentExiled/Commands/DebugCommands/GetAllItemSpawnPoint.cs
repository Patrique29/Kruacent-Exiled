using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class GetAllItemSpawnPoint : KECommand
    {
        public override string Command => "getallitemspawnpoint";

        public override string[] Aliases => new string[] { "gaisp" };

        public override string Description => "";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Dictionary<RoomType, byte> numberPerRooms = new Dictionary<RoomType, byte>();

            foreach (PoseRoomSpawnPointHandler.ItemSpawn spawn in PoseRoomSpawnPointHandler.AllPoses)
            {
                RoomType type = spawn.roomType;
                if (!numberPerRooms.ContainsKey(type))
                {
                    numberPerRooms[type] = 1;
                }
                else
                {
                    numberPerRooms[type]++;
                }
            }

            StringBuilder sb = StringBuilderPool.Pool.Get();
            sb.AppendLine();

            foreach (var kvp in numberPerRooms)
            {
                sb.Append(kvp.Key)
                    .Append(" - ")
                    .AppendLine(kvp.Value.ToString());
            }

            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;

        }
    }
}
