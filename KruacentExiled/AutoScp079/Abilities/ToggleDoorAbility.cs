using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.Patches.Events.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.AutoScp079.Abilities
{
    internal class ToggleDoorAbility : AbilityBase
    {
        public override float Cost => 1f;


        protected override void StartAbility(Auto079NPC npc)
        {
            Vector3 origin = npc.HumanPlayer.CameraTransform.position;
            Vector3 forward = npc.HumanPlayer.CameraTransform.forward;




            

            if (Physics.Raycast(origin, forward, out RaycastHit hitInfo, 50, (int)(LayerMasks.Door | LayerMasks.Default)))
            {
                Door door = Door.Get(hitInfo.collider.gameObject);
                if (MainPlugin.Instance.Config.Debug)
                {
                    DrawableLines.IsDebugModeEnabled = true;
                    DrawableLines.GenerateLine(10, Color.green, origin, hitInfo.point);
                }
                

                if (door != null)
                {
                    Log.Info("found door");
                    
                    //npc.RoleNPC.DoorStateChanger.GetCostForDoor(door.Base.)
                    npc.ToggleDoor(door);
                }
            }
        }



    }
}
