﻿using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using ServerHandle = Exiled.Events.Handlers.Server;
using Nine14Handle = Exiled.Events.Handlers.Scp914;
using MEC;
using Exiled.API.Features.Doors;
using System.Linq;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Scp914;
using Scp914;
using PlayerRoles;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.KruacentE.Misc;

namespace KE.Misc
{
    public class MainPlugin : Plugin<Config>
    {
        internal static MainPlugin Instance { get; private set; }
        private ServerHandler ServerHandler;
        internal _914 _914 { get; private set; }
        internal AutoElevator AutoElevator { get; private set; }
        internal ClassDDoor ClassDDoor { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            _914 = new _914();
            AutoElevator = new AutoElevator();
            ClassDDoor = new ClassDDoor();

            ServerHandler = new ServerHandler();
            ServerHandle.RoundStarted += ServerHandler.OnRoundStarted;
            Nine14Handle.UpgradingPlayer += _914.OnUpgradingPlayer;
            Exiled.Events.Handlers.Player.Dying += ScpNoeDeathMessage;

        }

        public override void OnDisabled()
        {
            ServerHandle.RoundStarted -= ServerHandler.OnRoundStarted;
            Nine14Handle.UpgradingPlayer -= _914.OnUpgradingPlayer;
            Exiled.Events.Handlers.Player.Dying -= ScpNoeDeathMessage;


            _914 = null;
            ClassDDoor = null;
            ServerHandler = null;
            AutoElevator = null;
            Instance = null;
        }



        internal void RandomFF()
        {
            Server.FriendlyFire = UnityEngine.Random.Range(0, 101) < Instance.Config.ChanceFF;
        }


        internal IEnumerator<float> NukeAnnouncement()
        {
            yield return Timing.WaitUntilTrue(() => 25 <= Round.ElapsedTime.TotalMinutes);
            Cassie.MessageTranslated("Warning automatic warhead will detonate in 5 minutes", 
                "Warning automatic warhead will detonate in <color=#FF0000>5</color> minutes");
        }


        internal IEnumerator<float> PeanutLockdown()
        {
            Door peanutDoor = Door.List.ToList().Where(x => x.Type == DoorType.Scp173NewGate).ToList()[0];
            peanutDoor.IsOpen = false;
            peanutDoor.ChangeLock(DoorLockType.Lockdown2176);
            yield return Timing.WaitForSeconds(135-Player.List.Count*15);
            peanutDoor.IsOpen = true;
            peanutDoor.Unlock();
        }
        


        internal void ScpNoeDeathMessage(DyingEventArgs ev)
        {
            if (!ev.Player.UserId.Equals("76561199066936074@steam"))
                return;
            Cassie.CustomScpTermination("69420", ev.DamageHandler);
        }

        


        
    }
}
