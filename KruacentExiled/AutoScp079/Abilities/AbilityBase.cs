using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Abilities
{
    public abstract class AbilityBase
    {
        public abstract float Cost { get; }


        protected abstract void StartAbility(Auto079NPC NPC);



        public void Start(Auto079NPC NPC)
        {
            if (NPC.CurrentPower > Cost)
            {
                StartAbility(NPC);
                NPC.CurrentPower -= Cost;
            }
        }




        protected virtual void Init()
        {

        }

        public static T Get<T>() where T : AbilityBase
        {
            if (_abilities == null) return null;

            foreach (AbilityBase ability in _abilities)
            {
                if(ability is T t)
                {
                    return t;
                }
            }
            return null;
        }

        private static List<AbilityBase> _abilities = null;

        public static void RegisterAbility()
        {
            if(_abilities != null)
            {
                return;
            }

            _abilities = KE.Utils.API.ReflectionHelper.GetObjects<AbilityBase>().ToList();


            foreach(AbilityBase abilityBase in _abilities)
            {
                abilityBase.Init();
            }


        }

    }
}
