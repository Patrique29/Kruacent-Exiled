using Exiled.API.Features;
using ServerAchievementFramework.API.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using PlayerRoles;
using KE.Utils.API.Interfaces;
using UnityEngine;
using KruacentExiled.CustomRoles.CustomSCPTeam;

namespace KruacentExiled.Achievements
{
    public class ActualKGBAgent : Achievement, IUsingEvents
    {
        public override string Name => "Holy hell, Actual KGB Agent";
        public override string Description => "Stay around a SCP for 3 min in a row without dying";
        public override bool IsHidden => false;
        public float RefreshRate => 1f;

        public float RadiusToCheck = 5f;
        private Dictionary<Player, uint> time = new Dictionary<Player, uint>();

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        public void OnRoundStarted()
        {

            Timing.RunCoroutine(CheckAroundSCPs());
        }

        private IEnumerator<float> CheckAroundSCPs()
        {
            while (Round.InProgress)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                foreach(Player scp in Player.Enumerable.Where(player => SCPTeam.IsSCP(player.ReferenceHub) && player.Role != RoleTypeId.Scp0492))
                {
                    foreach(Player human in Player.Enumerable.Where(human => human.IsAlive && !SCPTeam.IsSCP(human.ReferenceHub)))
                    {
                        if (Vector3.Distance(human.Position, scp.Position) <= Math.Pow(RadiusToCheck, 2))
                        {
                            if (!time.ContainsKey(human))
                                time[human] = 1;
                            else
                            {
                                Log.Debug($"{human.Nickname} is in the scp radius  ({time[human]})");
                                time[human]++;
                            }
                        }
                        else
                        {
                            time[human] = 0;
                        }
                        if (time[human] >= 180)
                        {
                            if (HasUnlockedIt(human))
                            {
                                GiveAchievement(human);
                            }
                        }
                        
                    }
                }


            }
        }
    }
}
