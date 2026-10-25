using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;
using ServerAchievementFramework.API.Feature;
using System.Linq;

namespace KruacentExiled.Achievements
{
    public class DeservedVictory : Achievement, IUsingEvents
    {
        public override string Name => "Deserved Victory";
        public override string Description => "As SCP-106, have everyone who entered the pocket dimension escape it and still win";
        public override bool IsHidden => false;


        private bool lockedOut = false;
        public void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.LeftPocketDimension += OnLeftPocketDimension;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.LeftPocketDimension -= OnLeftPocketDimension;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void OnLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev)
        {
            if (!ev.IsSuccessful)
            {
                lockedOut = true;
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (lockedOut) return;

            foreach (ReferenceHub hub in SCPTeam.SCPs)
            {
                Player player = Player.Get(hub);

                if (player != null && player.Role == RoleTypeId.Scp106)
                {
                    GiveAchievement(player);
                }
            }

        }
    }
}
