using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.Items.ItemEffects;

namespace KruacentExiled.CustomItems.Items
{
    public class DeployableWall : KECustomKeycard, ILumosItem, ISwitchableEffect
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Deployable Wall",
                    [TranslationKeyDesc] = "Drop to deploy a wall",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Mur",
                    [TranslationKeyDesc] = "Lâcher pour faire un mur",
                },
            };
        }
        public override ItemType ItemType => ItemType.KeycardCustomManagement;
        public override string Name { get; set; } = "Deployable Wall";
        public override float Weight { get; set; } = 0.65f;
        public Color Color { get; set; } = Color.green;
        public override string KeycardLabel { get; set; } = "Wall";
        public override Color32? KeycardLabelColor { get; set; } = Color.black;
        public override Color32? KeycardPermissionsColor { get; set; } = Color.black;
        public override Color32? TintColor { get; set; } = new Color32(139, 127, 158, 255); // janitor card color

        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance=25,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance=25,
                    Location = SpawnLocationType.InsideLczArmory,
                }
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance=50,
                    Type = LockerType.RifleRack,
                },
            }

        };

        public DeployableWall()
        {
            Effect = new DeployableWallEffect();
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.IsThrown)
            {
                ev.IsAllowed = true;
                return;
            }
            
            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);
            Effect.Effect(ev);

        }

        

    }

}
