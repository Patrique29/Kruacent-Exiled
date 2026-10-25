using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using ServerAchievementFramework.API.Feature;
using ServerAchievementFramework.API.Interface;

namespace KruacentExiled.Achievements
{
    public class PinkCandy : Achievement, IUsingEvents
    {
        public override string Name => "Kaboom? Kaboom";
        public override string Description => "Explose yourself now!";
        public override bool IsHidden => false;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Explosion && ev.DamageHandler.IsSuicide)
            {
                GiveAchievement(ev.Attacker);
            }

        }
    }
}
