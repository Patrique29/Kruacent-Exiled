using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.AutoScp079.Jobs;
using KruacentExiled.Map.Surface.Turrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Displays
{
    internal class PowerDisplay : Auto079Display
    {
        public PowerDisplay(Auto079NPC auto079NPC) : base(auto079NPC)
        {
            HintPosition = new PowerPlacement();
            CreateHint();
        }


        protected override string Text()
        {

            if (!Auto079NPC.RoleAssigned)
            {
                return " ";
            }
            sb.Clear();

            TimeSpan time = Auto079NPC.AuxManager.TimeToMax;

            sb.Append(Auto079NPC.CurrentPower)
                .Append('/')
                .Append(AuxManager.MaxAux)
                .AppendLine()
                .Append("regen=")
                .Append(Auto079NPC.AuxManager.RegenSpeed)
                .AppendLine()
                .Append("Time to Max=")
                .Append(time.ToString())
                .AppendLine()
                .Append("time to 40=")
                .Append(Auto079NPC.AuxManager.TimeTo(40));
            return sb.ToString();

        }
    }

    public class PowerPlacement : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 100;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
