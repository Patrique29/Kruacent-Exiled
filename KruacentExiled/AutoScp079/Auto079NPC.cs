using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.AutoScp079.Jobs;
using KruacentExiled.AutoScp079.Jobs.JobType;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using PlayerRoles;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlayerList;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079
{
    public class Auto079NPC
    {

        private static List<Auto079NPC> _instances;
        public static IReadOnlyCollection<Auto079NPC> Instances => _instances;
        public JobTracker JobTracker { get; }
        public Npc NPC { get; }

        public Scp079Role RoleNPC => NPC?.Role as Scp079Role;

        public Camera CurrentCamera => RoleNPC?.Camera;

        public GetClosestCameraComponent GetClosestCameraComponent { get; }

        public Player HumanPlayer { get; }

        public AutoScp079HumanDisplay Display { get; }

        public bool Debug { get; }

        public Auto079NPC(Player human, bool debug = false)
        {

            if(_instances == null)
            {
                _instances = new List<Auto079NPC>();
            }

            _instances.Add(this);

            if(human == null)
            {
                throw new ArgumentNullException("human");
            }
            Debug = debug;

            Log.Debug("creating auto079");
            NPC = Npc.Spawn("079", PlayerRoles.RoleTypeId.Scp079);

            HumanPlayer = human;


            GetClosestCameraComponent = HumanPlayer.GameObject.AddComponent<GetClosestCameraComponent>();
            GetClosestCameraComponent.Init(HumanPlayer, this);


            JobTracker = NPC.GameObject.AddComponent<JobTracker>();
            JobTracker.Init(this);

            Display = new AutoScp079HumanDisplay(this);
            



        }


        public bool PingVisibleToScp(Vector3 point,PingType pingType)
        {

            //float range = Scp079PingAbility.PingProcessors[(int)pingType].Range;
            //funny inaccessible from method
            float range = 60;
            float squaredRange = range * range;

            foreach (ReferenceHub hub in SCPTeam.SCPs)
            {
                if(hub.roleManager.CurrentRole is FpcStandardScp fpcStandardScp)
                {
                    if( (fpcStandardScp.FpcModule.Position - point).sqrMagnitude < squaredRange)
                    {
                        return true;
                    }
                }
            }


            return false;
        }


        public void Destroy()
        {
            _instances.Remove(this);
            Display.Destroy();
            NPC.Destroy();
        }

    }
}
