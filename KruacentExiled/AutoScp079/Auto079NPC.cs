using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.AutoScp079.Abilities;
using KruacentExiled.AutoScp079.Displays;
using KruacentExiled.AutoScp079.Jobs;
using KruacentExiled.AutoScp079.Jobs.JobType;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using PlayerRoles;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;
using Light = Exiled.API.Features.Toys.Light;

namespace KruacentExiled.AutoScp079
{
    public class Auto079NPC
    {

        private static List<Auto079NPC> _instances;
        public static IReadOnlyCollection<Auto079NPC> Instances => _instances;
        public JobTracker JobTracker { get; }
        public Npc NPC { get; }

        public Scp079Role RoleNPC => NPC?.Role as Scp079Role;

        public bool RoleAssigned => RoleNPC != null;

        public Camera CurrentCamera => RoleNPC?.Camera;

        public GetClosestCameraComponent GetClosestCameraComponent { get; }

        public Player HumanPlayer { get; }


        private static Dictionary<Camera, Light> _lightUnderCams = null;
        public void SwitchCamera(Camera newCamera)
        {
            Camera oldCamera = RoleNPC.Camera;

            RoleNPC.Camera = newCamera;
            Light light;
            if (!_lightUnderCams.ContainsKey(newCamera))
            {
                light = Light.Create(newCamera.Position, null, null, false);

                light.Color = Color.blue;

                light.LightType = LightType.Point;
                if(newCamera.Zone == ZoneType.Entrance)
                {
                    light.Intensity = .3f;
                }
                else
                {
                    light.Intensity = .9f;
                }
                _lightUnderCams[newCamera] = light;

                

            }
            else
            {
                light = _lightUnderCams[newCamera];
            }

            light.Spawn();

            if (_lightUnderCams.TryGetValue(oldCamera, out Light oldLight))
            {
                oldLight.UnSpawn();
            }

        }

        public float CurrentPower
        {
            get
            {
                return RoleNPC.Energy;
            }
            set
            {
                RoleNPC.Energy = value;
            }
        }


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

            if(_lightUnderCams == null)
            {
                _lightUnderCams = new Dictionary<Camera, Light>();
            }

            
            Debug = debug;

            Log.Debug("creating auto079");
            NPC = Npc.Spawn("079", PlayerRoles.RoleTypeId.Scp079);

            HumanPlayer = human;


            GetClosestCameraComponent = HumanPlayer.GameObject.AddComponent<GetClosestCameraComponent>();
            GetClosestCameraComponent.Init(HumanPlayer, this);


            JobTracker = NPC.GameObject.AddComponent<JobTracker>();
            JobTracker.Init(this);

            AbilityBase.RegisterAbility();

            Auto079Display.CreateDisplay(this);
        }


        public void ScanZone(ZoneType zone)
        {
            JobTracker.AddJob(ScanZoneJob.CreateJob(zone));
        }
        public void CancelScanZone()
        {
            Job current = JobTracker.CurrentJob;
            if(current is ScanZoneJob)
            {
                JobTracker.CancelCurrentJob();
            }

        }

        public void WaitAtRoom()
        {
            JobTracker.AddJob(WaitAtRoomJob.CreateJob(CurrentCamera.Room));
        }

        public void CancelCurrentJob()
        {
            JobTracker.CancelCurrentJob();
        }


        public void ToggleDoor(Door door)
        {
            if (door.IsElevator) return;

            JobTracker.AddJob(ToggleDoorJob.CreateJob(door));
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

        public void ActivateCurrentTesla()
        {
            if (RoleNPC.TeslaAbility.IsReady)
            {
                //remove 5 aux
                RoleNPC.ActivateTesla(false);
            }
            
            
        }


        public void Destroy()
        {
            _instances.Remove(this);
            Auto079Display.DestroyAll();
            NPC.Destroy();
        }

    }
}
