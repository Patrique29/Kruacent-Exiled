using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using KE.Utils.API.GifAnimator;
using KruacentExiled.ClientPrimitives;
using KruacentExiled.CustomItems.Items.ItemEffects;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class Convert : KEAbilities, ICustomIcon
    {
        public override string Name { get;  } = "Convert";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Convert",
                    [TranslationKeyDesc] = "Convert a zombie to your team",
                    ["ConvertNobody"] = "But nobody's here",
                    ["ConvertSameTeam"] = "I know you don't like them, but they're in your team",
                    ["ConvertNonZombie"] = "That ain't a zombie",
                    ["ConvertSuccess"] = "New friend acquired!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Convert",
                    [TranslationKeyDesc] = "Converti un zombie à la bonne foi",
                    ["ConvertNobody"] = "Mais personne n'est venu",
                    ["ConvertSameTeam"] = "Je sais que tu l'aimes pas mais il est bien avec toi",
                    ["ConvertNonZombie"] = "C'est pas un zombie ça",
                    ["ConvertSuccess"] = "Ami obtenu!",
                }
            };
        }

        public override float Cooldown { get;  } = 10*60f;

        public static float MaxDistance { get; set; } = 15f;

        public TextImage IconName => MainPlugin.Instance.icons["Convert"];





        protected override void AbilityAdded(Player player)
        {
            if (!player.GameObject.TryGetComponent<ConvertComponent>(out var comp))
            {
                comp = player.GameObject.AddComponent<ConvertComponent>();
                comp.Init(player, false);
            }
            base.AbilityAdded(player);
        }

        protected override void AbilityRemoved(Player player)
        {
            player.GameObject.GetComponent<ConvertComponent>().Destroy();
            base.AbilityRemoved(player);
        }


        protected override bool AbilityUsed(Player player)
        {
            Vector3 start = player.CameraTransform.position+ player.CameraTransform.forward*.2f;
            Vector3 end = start + player.CameraTransform.forward * MaxDistance;

            DrawableLines.IsDebugModeEnabled = MainPlugin.Instance.Config.Debug;
            DrawableLines.ServerGenerateLine(10f,null,start, end);


            

            if (!Physics.Linecast(start, end, out RaycastHit hit)) return false;


            Player playerHit = Player.Get(hit.collider);

            if (playerHit == null || playerHit == player)
            {
                ShowEffectHint(player, "But nobody's here");
                return false;
            }


            if (playerHit.Role.Side == player.Role.Side)
            {
                ShowEffectHint(player, "I know you don't like them but they're in your team");
                return false;
            }

            if (playerHit.IsScp && playerHit.Role != RoleTypeId.Scp0492)
            {
                ShowEffectHint(player, "That ain't a zombie");
                return false;
            }


            if (playerHit.IsScp)
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.None);
            }

            MainPlugin.ShowEffectHint(player, "New friend acquired!");
            return base.AbilityUsed(player);
        }

        public static bool Raycast(Player player,out RaycastHit hit)
        {
            Vector3 start = player.CameraTransform.position + player.CameraTransform.forward * .5f;
            Vector3 end = player.CameraTransform.position + player.CameraTransform.forward * MaxDistance;

            bool result = Physics.Raycast(player.CameraTransform.position + player.CameraTransform.forward * .5f, player.CameraTransform.forward, out hit, MaxDistance, (int)(LayerMasks.All));


            if (MainPlugin.Instance.Config.Debug)
            {
                DrawableLines.IsDebugModeEnabled = true;
                DrawableLines.GenerateLine(start, end);
            }



            return result;
        }



    }


    public class ConvertComponent : MonoBehaviour
    {
        public Player Player { get; private set; }
        private ClientSidePrimitive primitive = null;
        private bool debug;

        private bool _paused = false;
        public bool Paused
        {
            get => _paused;
            set
            {
                Log.Info("old=" + _paused);
                Log.Info("new=" + value);

                if (value && !_paused)
                {
                    DestroyPrimitive();
                }
                _paused = value;
            }
        }

        public void Init(Player player, bool debug = false)
        {
            Player = player;
            this.debug = debug;
            _paused = false;
        }



        private Color32 Color = new Color32(0, 0, 200, 150);
        private Vector3 lastPosition = Vector3.zero;
        private void MoveOrCreatePrimitive(Player playerHit)
        {
            if (primitive == null)
            {

                Vector3 scale = playerHit.Scale;
                primitive = new ClientSidePrimitive(playerHit.Position, Quaternion.identity, scale, PrimitiveType.Cube, Color, AdminToys.PrimitiveFlags.Visible, Player, false);
                primitive.SpawnClientPrimitive();
            }
            else
            {

                if (!lastPosition.Equals(playerHit.Position))
                {
                    primitive.Position = playerHit.Position;
                    primitive.Resync();
                }
            }
        }

        public void DestroyPrimitive()
        {
            primitive?.DestroyClientPrimitive();
            primitive = null;
        }

        public void Destroy()
        {
            DestroyPrimitive();
            Destroy(this);
        }

        private float counter = 1f;
        private const float TickTime = .1f;

        internal void Update()
        {
            counter -= Time.deltaTime;
            if (counter > 0)
            {
                return;
            }

            if (Paused)
            {
                return;
            }

            counter = TickTime;
            try
            {

                bool result = Convert.Raycast(Player, out var hit);

                if (result && Exiled.API.Features.Player.TryGet(hit.collider, out Exiled.API.Features.Player playerHit))
                {
                    if(SCPTeam.IsSCP(playerHit.ReferenceHub) && playerHit.Role.Type != RoleTypeId.Scp0492)
                    {
                        Log.Info("scp non zombie");
                        DestroyPrimitive();
                        return;
                    }


                    if(playerHit.LeadingTeam == Player.LeadingTeam)
                    {
                        Log.Info("same team");
                        DestroyPrimitive();
                        return;
                    }

                    MoveOrCreatePrimitive(playerHit);
                }
                else
                {
                    DestroyPrimitive();
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

    }

}

