using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.AutoScp079.Jobs
{
    public struct ScanResult : IEquatable<ScanResult>
    {

        public Player playerDetected;

        public Room room;
        public Vector3 playerPosition;


        public ScanResult(Player playerDetected, Room room)
        {
            this.playerDetected = playerDetected;
            this.room= room;
            playerPosition = playerDetected.Position;
        }

        public bool Equals(ScanResult other)
        {
            return playerDetected == other.playerDetected;
        }
    }
}
