using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.AutoScp079;
using KruacentExiled.AutoScp079.Jobs.JobType;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled
{
    /// <summary>
    /// Used to test other part of the plugin do not remove
    /// </summary>
    internal class TestEvent : IUsingEvents
    {

        private Auto079NPC Auto079NPC = null;
        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;

            Auto079NPC?.Destroy();
            Auto079NPC = null;
        }



        private void OnRoundStarted()
        {
            Timing.RunCoroutine(Auto079());
        }


        private IEnumerator<float> Auto079()
        {
            yield return Timing.WaitForSeconds(3);
            Round.IsLocked = true;
            Player firstPlayer = Player.List.First(p => !p.IsNPC);

            firstPlayer.Role.Set(PlayerRoles.RoleTypeId.Scp049);

            try
            {
                Auto079NPC = new Auto079NPC(firstPlayer, true);
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            



            Npc test = Npc.Spawn("testping", PlayerRoles.RoleTypeId.ClassD,position:firstPlayer.Position);


            //SwitchCameraJob job = SwitchCameraJob.CreateJob(Camera.Get(Exiled.API.Enums.CameraType.LczCellblockEntry));
            //Auto079NPC.JobTracker.AddJob(job);

            //firstPlayer.Role.Set(PlayerRoles.RoleTypeId.Scp079);
            //yield return Timing.WaitForSeconds(1);

            //Scp079Role role = firstPlayer.Role as Scp079Role;

        }

    }
}
