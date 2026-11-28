using Exiled.API.Features;
using Exiled.API.Features.Pools;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Arguments;
using HintServiceMeow.Core.Models.Hints;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.HintPositions;
using KruacentExiled.CustomRoles.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.API.Features
{
    public class KEAbilityUI
    {

        public StringBuilder sb;

        public KEAbilityUI()
        {
            sb = StringBuilderPool.Pool.Get();
        }

        ~KEAbilityUI()
        {
            StringBuilderPool.Pool.Return(sb);
        }

        public Dictionary<Player, List<AbstractHint>> PlayersHints { get; } = new Dictionary<Player, List<AbstractHint>>();
        public Dictionary<AbstractHint, AbstractHint> AddonHints { get; } = new Dictionary<AbstractHint, AbstractHint>();

        public void InitHints(Player player)
        {
            if (!PlayersHints.TryGetValue(player, out var _))
            {
                PlayersHints.Add(player, new List<AbstractHint>());
                for (int i = 0; i < KEAbilities.InitialAbilitySlot; i++)
                {
                    AbilitiesPosition position = AbilitiesPosition.GetIndex(i);
                    AbstractHint hint = DisplayHandler.Instance.CreateAuto(player, arg => UpdateHint(arg), position.HintPlacement);
                    PlayersHints[player].Add(hint);
                    AddonHints.Add(hint, DisplayHandler.Instance.CreateAuto(player, arg => UpdateAddon(arg, hint), AddonAbilitiesPosition.Get(position).HintPlacement, HintSyncSpeed.Slow));
                }
            }
        }

        public void Stop(Player player)
        {
            if (PlayersHints.TryGetValue(player, out var hints))
            {
                foreach (AbstractHint hint in hints)
                {
                    AddonHints.Remove(hint);
                }
                PlayersHints.Remove(player);
            }
        }

        public void Reset()
        {
            PlayersHints.Clear();
            AddonHints.Clear();
            addonAbilitiesexception.Clear();
        }


        private string UpdateHint(AutoContentUpdateArg arg)
        {
            Player player = Player.Get(arg.PlayerDisplay.ReferenceHub);
            int index = PlayersHints[player].FindIndex(h => h == arg.Hint);
            if (KEAbilities.PlayersAbility[player].TryGet(index, out KEAbilities ability))
            {

                sb.Clear();
                ability.Gui(sb, player);
                return sb.ToString();
            }
            return " ";
        }



        private HashSet<KEAbilities> addonAbilitiesexception = new HashSet<KEAbilities>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="hint">the other hint</param>
        /// <returns></returns>
        private string UpdateAddon(AutoContentUpdateArg arg, AbstractHint hint)
        {
            Player player = Player.Get(arg.PlayerDisplay.ReferenceHub);
            int index = PlayersHints[player].FindIndex(h => h == hint);

            if (KEAbilities.PlayersAbility[player].TryGet(index, out KEAbilities ability))
            {
                if (ability is ICustomIcon icon && !addonAbilitiesexception.Contains(ability))
                {
                    sb.Clear();
                    try
                    {
                        sb.Append(icon.IconName.RawString);
                    }
                    catch (KeyNotFoundException)
                    {
                        addonAbilitiesexception.Add(ability);
                        Log.Error("Icon for ability " + ability.Name + " is missing");
                        return " ";
                    }
                    return sb.ToString();
                }
            }
            return " ";
        }




    }
}
