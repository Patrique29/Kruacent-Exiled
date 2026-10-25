using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using ServerAchievementFramework.API.Feature;
using ServerAchievementFramework.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.NonAllocLINQ;

namespace KruacentExiled.Achievements
{
    public class LoveUpDown : Achievement, IUsingEvents
    {
        public override string Name => "I love when you go up and down baby";
        public override string Description => "As any class, activate all elevator from the inside at least once in the round";
        public override bool IsHidden => false;

        public Dictionary<Player, HashSet<Lift>> Counter = new Dictionary<Player, HashSet<Lift>>();

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;
        }

        public void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            Player player = ev.Player;
            if (ev.IsCalledFromInside)
            {
                if (Counter.ContainsKey(player))
                {
                    Counter.Add(player, new HashSet<Lift>());
                }

                Counter[player].Add(ev.Lift);
            }

            if (Lift.List.All(v => Counter[player].Contains(v)))
                GiveAchievement(player);
        }
    }
}
