using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Interfaces;
using PlayerRoles;
using ServerAchievementFramework.API.Feature;
using ServerAchievementFramework.API.Interface;
using System.Linq;

namespace KruacentExiled.Achievements
{
    public class AvengeFather : Achievement, IUsingEvents
    {
        public override string Name => "I've avenged you father";
        public override string Description => "Win as a SCP-049-2 instance with only other SCP-049-2 instances alive";
        public override bool IsHidden => false;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if(ev.ClassList.scps_except_zombies <= 0 && ev.ClassList.zombies > 0)
            {
                GiveAchievement(Player.Enumerable.Where(p => p.Role == RoleTypeId.Scp0492));
            }

        }
    }
}
