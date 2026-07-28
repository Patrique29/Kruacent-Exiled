using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.AutoScp079.Jobs;
using KruacentExiled.AutoScp079.Jobs.JobType;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079
{
    public class Auto079NPC
    {
        public JobTracker JobTracker { get; }
        public Npc NPC { get; }

        public Scp079Role RoleNPC => NPC.Role as Scp079Role;

        public Camera CurrentCamera => RoleNPC.Camera;

        public GetClosestCameraComponent GetClosestCameraComponent { get; }

        public Player HumanPlayer { get; }

        public AutoScp079HumanDisplay Display { get; }

        public Auto079NPC(Player human)
        {
            if(human == null)
            {
                throw new ArgumentNullException("human");
            }

            Log.Debug("creating auto079");
            NPC = Npc.Spawn("079", PlayerRoles.RoleTypeId.Scp079);

            HumanPlayer = human;


            GetClosestCameraComponent = HumanPlayer.GameObject.AddComponent<GetClosestCameraComponent>();
            GetClosestCameraComponent.Init(HumanPlayer, this);


            JobTracker = NPC.GameObject.AddComponent<JobTracker>();
            JobTracker.Init(this);

            Display = new AutoScp079HumanDisplay(this);
            



        }


        public void Destroy()
        {
            Display.Destroy();
            NPC.Destroy();
        }

    }
}
