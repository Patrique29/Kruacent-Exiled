using AdminToys;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.ClientPrimitives
{
    public static class Loader
    {


        public static uint PrimitiveAssetId { get; private set; }


        public static void Load()
        {

            foreach (GameObject prefab in NetworkClient.prefabs.Values)
            {
                if (prefab.TryGetComponent<PrimitiveObjectToy>(out _))
                {
                    PrimitiveAssetId = prefab.GetComponent<NetworkIdentity>().assetId;
                    break;
                }
            }

        }
    }
}
