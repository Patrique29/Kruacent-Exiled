using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs.RoomActions
{
    internal abstract class RoomAction
    {

        public Room Room { get; }

        public Auto079NPC Npc { get; }



        public abstract void OnUpdate();


        protected RoomAction(Room room, Auto079NPC npc) 
        {
            Room = room;
            Npc = npc;
        }

        public static RoomAction GetAction(Room room, Auto079NPC npc)
        {

            return room.Type switch
            {
                RoomType.HczTesla => new TeslaRoomAction(room, npc),
                _ => null
            };

        }
    }
}
