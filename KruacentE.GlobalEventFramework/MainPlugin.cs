﻿using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using Exiled.API.Enums;
using Player = Exiled.API.Features.Player;
using GEFExiled.GEFE.API.Features;
using GEFExiled.GEFE.API.Interfaces;
using GEFExiled.GEFE.Examples.GE;
namespace GEFExiled
{
    internal class MainPlugin : Plugin<Config>
	{
		public override PluginPriority Priority => PluginPriority.Highest;

		internal Handlers.ServerHandler _server;

		internal static MainPlugin _instance;

		public static List<CoroutineHandle> coroutineHandles = new List<CoroutineHandle>();
		public override void OnEnabled()
		{
			_instance = this;
			List<IGlobalEvent> globalEvents = new List<IGlobalEvent>(){ new SystemMalfunction(), new Speed(),new RandomSpawn() };
			GlobalEvent.Register(globalEvents);

			RegisterEvents();
			
			base.OnEnabled();
        }

		public override void OnDisabled()
		{
			UnregisterEvents();
			Timing.KillCoroutines();
			base.OnDisabled();
			_instance = null;
		}

		private void RegisterEvents()
		{
			_server = new Handlers.ServerHandler(this);

			Exiled.Events.Handlers.Server.RoundStarted += _server.OnRoundStarted;
			Exiled.Events.Handlers.Server.EndingRound += _server.OnEndingRound;
		}

		private void UnregisterEvents()
		{
			Exiled.Events.Handlers.Server.RoundStarted -= _server.OnRoundStarted;
			Exiled.Events.Handlers.Server.EndingRound -= _server.OnEndingRound;

			_server = null;
		}

		public void Show()
		{
			foreach (Player player in Player.List)
			{
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast();
				b.Content = ShowText();
				b.Duration = 10;
				player.Broadcast(b);
			}
		}

		private String ShowText()
		{
			String result = "Global Events: ";
			for (int i = 0; i < GlobalEvent.ActiveGE.Count(); i++)
			{
				result += GlobalEvent.ActiveGE[i].Description;
				if (GlobalEvent.ActiveGE.Count() > 1)
				{
					result += ",";
				}
			}
			return result;
		}

		public List<IGlobalEvent> ChooseGE()
		{
            List<IGlobalEvent> activeGE = ChooseRandomGE();

            foreach (IGlobalEvent ge in activeGE)
			{
				Log.Debug($"GE : {ge.Name}");
				var a = Timing.RunCoroutine(ge.Start());
                Log.Debug("fin start corutn");
                coroutineHandles.Add(a); //crash
				Log.Debug("fin add corutn");
            }
			return activeGE;
        }


		private List<IGlobalEvent> ChooseRandomGE(int nbGE = 1)
		{
			List<IGlobalEvent> result = new List<IGlobalEvent>();
			List<IGlobalEvent> gelist = GlobalEvent.GlobalEventsList.ToList();

			double totalWeight = GlobalEvent.GlobalEventsList.Sum(x => x.Weight);
			double randomWeight = UnityEngine.Random.value * totalWeight;
			double cumulativeWeight = 0.0;

			
			for (int i = 0; i < nbGE; i++)
			{
                bool found = false;
				foreach (IGlobalEvent ge in gelist)
				{
					cumulativeWeight += ge.Weight;

					if (randomWeight <= cumulativeWeight)
					{
                        result.Add(ge);
						break;
					}
				}
				if (!found)
				{
                    result.Add(gelist.Last());
					gelist.Remove(gelist.Last());
				}

			}
            return result;
		}

		public void StopCoroutines()
		{
			Timing.KillCoroutines();
		}
	}
}
