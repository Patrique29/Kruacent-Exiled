using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.CR.MTF.Engineer
{
    internal class EngineerRole : KECustomRole
    {


        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Engineer",
                    [TranslationKeyDesc] = "Clank clank clank",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Ingénieur",
                    [TranslationKeyDesc] = "Clank clank clank",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Engineer",
                    [TranslationKeyDesc] = " ",
                },
            };
        }








    }

}
