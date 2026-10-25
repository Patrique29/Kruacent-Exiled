using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Interfaces;
using PlayerRoles;
using ServerAchievementFramework.API.Feature;
using ServerAchievementFramework.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.Achievements
{
    public sealed class BlitzNeck : Achievement, IUsingEvents
    {
        public override string Name => "BlitzNeck";
        public override string Description => "As SCP-173, kill 5 people in 20 seconds";
        public override bool IsHidden => false;


        private Dictionary<Player, LastPlayerKill> time;

        public const byte NumberKill = 5;
        public const int Time = 20;

        public void SubscribeEvents()
        {
            time = new Dictionary<Player, LastPlayerKill>();
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            time = null;
        }

        private bool CheckRole(Player player)
        {
            return player.Role == RoleTypeId.Scp173;
        }

        public void OnDied(DiedEventArgs ev)
        {
            Player attacker = ev.Attacker;

            if (attacker == null) return;
            if (CheckRole(attacker)) return;
            if (!time.ContainsKey(attacker))
            {
                time[attacker] = new LastPlayerKill(attacker);
                
            }
            LastPlayerKill playerKill = time[attacker];
            playerKill.AddKill();


            if (playerKill.HasReachAchievementCondition())
            {
                GiveAchievement(attacker);
            }
        }

        private class LastPlayerKill
        {
            public Player player;
            public DateTime lastkill;
            public byte counter;

            public LastPlayerKill(Player player)
            {
                this.player = player;
            }

            public bool HasReachAchievementCondition()
            {
                return counter >= NumberKill;
            }

            public void AddKill()
            {
                if (lastkill.AddSeconds(Time) <= DateTime.Now)
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                }
                lastkill = DateTime.Now;
            }



        }
    }
}
