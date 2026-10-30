using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using System;
using System.Text;

namespace KruacentExiled.Misc.Features.DisplayPluginVersion
{
    internal class DisplayPluginVersion : MiscFeature
    {
        public static HintPosition HintPosition = new DisplayPluginVersionPosition();

        private StringBuilder stringBuilder;

        public override void SubscribeEvents()
        {
            stringBuilder = StringBuilderPool.Pool.Get();
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            StringBuilderPool.Pool.Return(stringBuilder);

            base.UnsubscribeEvents();
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            Player player = ev.Player;

            if (ev.Player == null)
            {
                return;
            }

            Timing.CallDelayed(1f, () =>
            {
                if (Round.IsLobby)
                {
                    //probably better with the Addhint
                    DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(), HintPosition.HintPlacement);
                }
            });

        }

        private string GetPlayers()
        {
            if (!Round.IsLobby) return string.Empty;
            stringBuilder.Clear();
            stringBuilder.Append('v');
            stringBuilder.Append(KruacentExiled.MainPlugin.Instance.Version);

            return stringBuilder.ToString();
        }

        private void OnRoundStarted()
        {
            foreach (Player player in Player.List)
            {
                DisplayHandler.Instance.RemoveHint(player, HintPosition.HintPlacement);
            }

        }
    }
}
