using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.AutoScp079.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Displays
{
    internal class CurrentCameraDisplay : Auto079Display
    {
        public CurrentCameraDisplay(Auto079NPC auto079NPC) : base(auto079NPC)
        {
            HintPosition = new CurrentCameraPlacement();
            CreateHint();
        }

        protected override string Text()
        {
            if (!Auto079NPC.RoleAssigned)
            {
                return " ";
            }
            sb.Clear();
            Camera current = Auto079NPC.CurrentCamera;

            if (current == null)
            {
                return " ";
            }


            sb.Append(current.Name);



            return sb.ToString();
        }
    }

    public class CurrentCameraPlacement : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 200;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
