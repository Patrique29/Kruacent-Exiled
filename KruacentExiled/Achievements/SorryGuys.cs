using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
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
    public class SorryGuys : Achievement, IUsingEvents
    {
        public override string Name => "Sorry Guys...";
        public override string Description => "Respawn alone";
        public override bool IsHidden => false;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.Players.Count == 1)
            {
                GiveAchievement(ev.Players.First());
            }
                
        }
    }
}
