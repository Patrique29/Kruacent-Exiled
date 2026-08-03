using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Abilities
{
    internal class ScanZoneAbility : AbilityBase, ICancellable
    {
        public override float Cost => 40f;

        public void Cancel(Auto079NPC npc)
        {
            npc.CancelScanZone();
        }

        protected override void StartAbility(Auto079NPC npc)
        {
            npc.ScanZone(npc.HumanPlayer.Zone);
        }
    }
}
