using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.AutoScp079
{
    public class AuxManager : MonoBehaviour
    {


        public Auto079NPC Auto079;

        public const float BaseRegenSpeed = .2f;


        public float RegenMultiplier
        {
            get
            {
                float result = RegenPerGenerator[_nbGeneratorEngaged] + 1 ;

                if (!hadFirstRespawn)
                {
                    result /= 2;
                }
                return result;

            }
        }


        private static readonly float[] RegenPerGenerator = new float[]
        {
            0,.5f,1f,2.5f
        };

        private bool hadFirstRespawn = false;

        public bool HadARespawn => hadFirstRespawn;


        public float RegenSpeed => BaseRegenSpeed * RegenMultiplier;

        public TimeSpan TimeToMax
        {
            get
            {
                return TimeTo(MaxAux);
            }
        }


        public TimeSpan TimeTo(float aux)
        {
            return TimeSpan.FromSeconds(aux / RegenSpeed);
        }

        private bool _init = false;

        public void Init(Auto079NPC auto079)
        {
            if (_init)
            {
                Log.Warn(GetType().Name + " already init");
                return;
            }
            Auto079 = auto079;
            CurrentAux = 0;
            LabApi.Events.Handlers.ObjectiveEvents.ActivatedGeneratorCompleted += OnActivatedGeneratorCompleted;
            LabApi.Events.Handlers.ServerEvents.WaveRespawned += OnWaveRespawned;
            _init = true;
        }

        private void OnWaveRespawned(LabApi.Events.Arguments.ServerEvents.WaveRespawnedEventArgs ev)
        {
            hadFirstRespawn = true;
        }

        private int _nbGeneratorEngaged = 0;


        internal int GeneratorEngaged
        {
            get => _nbGeneratorEngaged;
            set => _nbGeneratorEngaged = Mathf.Clamp(value + 1, 0, 3);
        }

        private void OnActivatedGeneratorCompleted(LabApi.Events.Arguments.ObjectiveEvents.GeneratorActivatedObjectiveEventArgs ev)
        {
            GeneratorEngaged++;
        }


        private void OnDestroy()
        {
            LabApi.Events.Handlers.ObjectiveEvents.ActivatedGeneratorCompleted -= OnActivatedGeneratorCompleted;
            LabApi.Events.Handlers.ServerEvents.WaveRespawned -= OnWaveRespawned;
        }


        public const float MaxAux = 100f;

        private float _currentAux;
        public float CurrentAux
        {
            get => _currentAux;
            private set
            {
                value = Mathf.Clamp(value, 0f, MaxAux);
                _currentAux = value;
            }
        }


        public void RemoveAux(float aux)
        {
            CurrentAux -= Mathf.Clamp(aux,0f,CurrentAux);
        }

        internal void Update()
        {
            Regenerate();
        }

        private void Regenerate()
        {
            CurrentAux += Time.deltaTime * RegenSpeed;
        }

    }
}
