using Exiled.Events.EventArgs.Player;
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
    public class DarkPlace : Achievement, IUsingEvents
    {
        public override string Name => "Dark Place";
        public override string Description => "As a human, be warped in the Pocket dimension while you & SCP-106 both are in a blackouted room";
        public override bool IsHidden => false;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
        }

        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (ev.Scp106.CurrentRoom.AreLightsOff && ev.Player.CurrentRoom.AreLightsOff)
            {
                GiveAchievement(ev.Player);
            }

        }
    }
}
