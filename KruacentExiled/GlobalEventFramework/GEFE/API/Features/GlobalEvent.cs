using CommandSystem.Commands.RemoteAdmin.Broadcasts;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Models.Hints;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.GlobalEventFramework;
using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Extensions;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Features
{

    /// <summary>
    /// Create an event at the start of the round
    /// </summary>
    public abstract class GlobalEvent : KEEvents
    {
        /// <summary>
        /// Handle the <see cref="GlobalEvent"/> in the round
        /// </summary>
        private class GlobalEventHandler : IUsingEvents
        {
            private bool _eventsub = false;

            public void SubscribeEvents()
            {
                if (_eventsub) return;

                Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded += OnEndingRound;
                LabApi.Events.Handlers.ServerEvents.MapGenerating += OnMapGenerating;

                _eventsub = true;
            }

            

            public void UnsubscribeEvents()
            {
                if (!_eventsub) return;
                Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded -= OnEndingRound;
                LabApi.Events.Handlers.ServerEvents.MapGenerating -= OnMapGenerating;

                _eventsub = false;
            }
            private void OnMapGenerating(LabApi.Events.Arguments.ServerEvents.MapGeneratingEventArgs ev)
            {
                ChooseGlobalEvent();
            }

            private void OnEndingRound(RoundEndedEventArgs _)
            {
                Log.Warn("ending round");
                DisableEvents(_activeGE);
            }
            private void OnRoundStarted()
            {
                if(ForcedGE.Count != 0)
                {
                    _activeGE = ForcedGE.ToHashSet();
                    ForcedGE.Clear();
                }
                
                EnableEvents(_activeGE);
                Show();
            }


        }


        private static Config Config => MainPlugin.Configs;
        private static GlobalEventHandler _handler = new GlobalEventHandler();

        /// <summary>
        /// Currently active <see cref="GlobalEvent"/>
        /// </summary>
        private static HashSet<GlobalEvent> _activeGE = new HashSet<GlobalEvent>();

        /// <summary>
        /// The color of all the <see cref="API.Enums."/>
        /// </summary>
        public static IReadOnlyDictionary<ImpactLevel, string> ImpactToColor = new Dictionary<ImpactLevel, string>()
        {
            { ImpactLevel.VeryLow, "#d8d8ff" },
            { ImpactLevel.Low, "#d8e8f0" },
            { ImpactLevel.Medium, "#d8fcde" },
            { ImpactLevel.High, "#fbfbd8" },
            { ImpactLevel.VeryHigh, "#f0e8d8" },
            { ImpactLevel.Insane, "#ffd8d8" },
        };

        /// <summary>
        /// <see cref="GlobalEvent"/> forced by a command
        /// </summary>
        public static HashSet<GlobalEvent> ForcedGE { get; } = new HashSet<GlobalEvent>();


        /// <summary>
        /// A list of all registered GlobalEvents
        /// </summary>
        public static IEnumerable<GlobalEvent> GlobalEventsList => List.Where(ev => ev is GlobalEvent).Cast<GlobalEvent>();
        
        /// <summary>
        /// The text shown to the player when the <see cref="GlobalEvent"/> is activated
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// Other form of the <see cref="Description"/>
        /// </summary>
        public virtual string[] AltDescription { get; } = null;
        /// <summary>
        /// The <see cref="Enums.ImpactLevel"/> of the <see cref="GlobalEvent"/>
        /// </summary>
        public virtual ImpactLevel ImpactLevel { get; } = ImpactLevel.Medium;
        /// <summary>
        /// Check if the <see cref="GlobalEvent"/> is currently active
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _activeGE.Contains(this);
            }
        }

        /// <summary>
        /// <para>Number of <see cref="GlobalEvent"/> chose</para>
        /// <para>-1 if it wasn't chose yet</para>
        /// </summary>
        public static int NumberOfGE { get; set; } = -1;


        /// <summary>
        /// Internal subscribe event
        /// </summary>
        protected sealed override void SubscribeEvents()
        {
            _handler.SubscribeEvents();
            base.SubscribeEvents();
        }
        /// <summary>
        /// Internal unsubscribe event
        /// </summary>
        protected sealed override void UnsubscribeEvents()
        {
            _handler.UnsubscribeEvents();
            
            base.UnsubscribeEvents();
        }

        /// <summary>
        /// Disable this <see cref="GlobalEvent"/>
        /// </summary>
        /// <param name="ev"></param>
        protected override void Disable(KEEvents ev)
        {

            _activeGE.Remove(ev as GlobalEvent);
            base.Disable(ev);
        }


        /// <summary>
        /// Choose a/multiple random(s) <see cref="GlobalEvent"/>(s)
        /// </summary>
        private static void ChooseGlobalEvent()
        {
            if(NumberOfGE == -1)
            {
                NumberOfGE = Random.value < .1f ? 2 : 1;
            }


            _activeGE = GetRandomEvent<GlobalEvent>(NumberOfGE).ToHashSet();

            foreach(GlobalEvent ge in _activeGE)
            {
                KELog.Debug("chose : " + ge.Name);
            }
            

        }
        /// <summary>
        /// Show the active <see cref="GlobalEvent"/> to the <see cref="Player"/>s
        /// </summary>
        private static void Show()
        {
            

            ShowConsole();


            string text = ShowText();

            foreach (Player player in Player.List)
            {
                AbstractHint hint = DisplayHandler.Instance.AddHint(MainPlugin.GEAnnouncement, player, text, 10);

                hint.FontSize = 30;
                
            }
        }

        /// <summary>
        /// Show the active <see cref="GlobalEvent"/> in the Local Admin
        /// </summary>
        private static void ShowConsole()
        {
            Log.Info($"Global Event(s) ({_activeGE.Count()}): ");
            foreach (GlobalEvent ge in _activeGE)
            {
                Log.Info(ge.Name);
            }

        }

        /// <summary>
        /// Get all of the description (<see cref="Description"/> and <see cref="AltDescription"/>)
        /// </summary>
        private List<string> AllDesc
        {
            get
            {
                List<string> allDesc = new List<string>()
                {
                    Description,
                };
                allDesc.AddRange(AltDescription);
                return allDesc;
            }
        }


        /// <summary>
        /// Get the colored description shown to the players
        /// </summary>
        /// <returns></returns>
        private static string ShowText()
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();

            builder.Append("Global Events: ");
            List<GlobalEvent> ge = ListPool<GlobalEvent>.Pool.Get(_activeGE);







            for (int i = 0; i < ge.Count(); i++)
            {
                GlobalEvent globalEvent = ge[i];




                builder.Append("<color=");
                builder.Append(ImpactToColor[globalEvent.ImpactLevel]);
                builder.Append(">");

                builder.Append(globalEvent.ImpactLevel.Shorten());



                if (globalEvent.IsRedacted())
                {
                    builder.Append("[REDACTED]");
                }
                else
                {
                    if (!Config.ActivateAltDescription || globalEvent.AltDescription == null)
                    {
                        builder.Append(globalEvent.Description);
                    }
                    else
                    {
                        builder.Append(globalEvent.AllDesc.GetRandomValue());
                    }
                }

                builder.AppendLine("</color>");
                if (ge.Count() > 1 && i < ge.Count() - 1)
                {
                    builder.Append(", ");
                }


            }
            ListPool<GlobalEvent>.Pool.Return(ge);


            return StringBuilderPool.Pool.ToStringReturn(builder);
        }

        /// <summary>
        /// Roll the chance of the current <see cref="GlobalEvent"/> being redacted
        /// </summary>
        /// <returns></returns>
        private bool IsRedacted()
        {
            float chanceRedacted;

            if(this is IChanceRedactable force)
            {
                chanceRedacted = force.ChanceRedacted;
            }
            else
            {
                chanceRedacted = Config.ChanceRedacted;
            }

            chanceRedacted = Mathf.Clamp(chanceRedacted, 0, 100);

            return Random.Range(0f, 100f) <= chanceRedacted;


        }

    }
}
