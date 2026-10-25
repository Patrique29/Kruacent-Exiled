using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Interfaces;
using ServerAchievementFramework.API.Feature;
using ServerAchievementFramework.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements
{
    public class FuckYouGuys : Achievement, IUsingEvents
    {
        public override string Name => "Fuck You Guys";
        public override string Description => "Respawn alone at ALL respawns in the round until the deadman switch";
        public override bool IsHidden => true;
        public List<Player> Counter = Player.List.ToList();

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
        }

        public void OnDetonated()
        {
            if (Warhead.DeadmanSwitchEnabled)
            {
                Counter.ForEach(p =>
                {
                    GiveAchievement(p);
                });
            }
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.Players.Count != 1)
            {
                Counter = new List<Player>();
                return;
            }

            Counter = Counter.Except(Player.List.Except(ev.Players)).ToList();
            
            
        }
    }
}
