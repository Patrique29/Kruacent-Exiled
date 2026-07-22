using CommandSystem;
using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Toys;
using KE.Utils.API.Commands;
using KE.Utils.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static KruacentExiled.CustomSpawnPoint.PoseRoomSpawnPointHandler;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ShowItemSpawnPoint : KECommand
    {
        public override string Command => "showitemspawnpoint";
        public override string[] Aliases => new string[] { "sisp" };

        public override string Description => "show the spawn point of custom item";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Player.TryGet(sender,out Player player))
            {
                response = "player null";
                return false;
            }

            Room room = Room.Get(player.Position);

            if (room == null)
            {
                response = "room not found";
                return false;
            }
            bool line = false;
            if (arguments.Count > 0 && arguments.At(0) == "l")
            {
                string arg = arguments.At(0);

                switch (arg)
                {
                    case "l":
                        line = true;
                        break;
                    default:
                        break;
                }
            }
            StringBuilder sb = StringBuilderPool.Pool.Get();

            int num = ShowPoses(sb,room.Type, line,player);


            response = "found "+ num + " item spawn point:" + StringBuilderPool.Pool.ToStringReturn(sb);
            return true;
        }
        public static int ShowPoses(StringBuilder sb, RoomType roomType, bool line,Player player)
        {
            List<Primitive> primitives = ListPool<Primitive>.Pool.Get();

            Room room = Room.Get(roomType);
            foreach (ItemSpawn pose in AllPoses.OrderBy(s => Vector3.Distance(player.Position, s.Position)))
            {
                if(roomType != pose.roomType)
                {
                    continue;
                }
                Color color = Color.red;
                float distance = Vector3.Distance(player.Position, pose.Position);

                if (UsablePoses.Contains(pose))
                {
                    color = Color.green;
                }

                

                Vector3 position = pose.Position;

                KELog.Debug("position=" + position);
                KELog.Debug("positionplayer=" + player.Position);



                Primitive prim = Primitive.Create(position, null, Vector3.one * .1f, false, color);
                prim.Collidable = false;
                prim.Spawn();
                primitives.Add(prim);


                if (line)
                {
                    DrawableLines.IsDebugModeEnabled = true;
                    Draw.Line(player.Position, position, color, 10);

                }


            }


            Timing.CallDelayed(5, delegate
            {
                foreach (Primitive primive in primitives)
                {
                    primive.Destroy();
                }
                ListPool<Primitive>.Pool.Return(primitives);
            });

            return primitives.Count;
        }
    }
}
