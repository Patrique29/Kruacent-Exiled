using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using ServerAchievementFramework.API.Feature;
using System.Linq;

namespace KruacentExiled.Achievements
{
    public class Speedrun : Achievement, IUsingEvents
    {
        public override string Name => "Speedrun";
        public override string Description => "As any SCP, win before any respawn wave";
        public override bool IsHidden => false;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RespawnedTeam += OnRespawnTeam;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RespawnedTeam -= OnRespawnTeam;
        }

        private void OnRespawnTeam(RespawnedTeamEventArgs ev)
        {
            lockedOut = true;
        }

        private bool lockedOut = false;
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (lockedOut) return;

            foreach (ReferenceHub hub in SCPTeam.SCPs)
            {
                Player player = Player.Get(hub);

                if (player != null)
                {
                    GiveAchievement(player);
                }
            }

        }
    }
}
