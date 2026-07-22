using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomSpawnPoint
{
    public static class PoseRoomSpawnPointHandler
    {
        public class ItemSpawn : IEquatable<ItemSpawn>
        {
            public readonly RoomType roomType;

            //item position in the room at 0°
            public readonly Vector3 localposition;
            //item rotation
            public readonly Quaternion localrotation;

            /// <summary>
            /// don't use the corridor
            /// </summary>
            /// <param name="roomType"></param>
            /// <param name="position"></param>
            /// <param name="rotation"></param>
            public ItemSpawn(RoomType roomType,Vector3 position,Quaternion rotation)
            {

                this.roomType = roomType;
                localposition = position;
                localrotation = rotation;
            }

            private Room Room
            {
                get
                {
                    return Room.List.FirstOrDefault(r => r.Type == roomType);
                }
            }

            public Vector3 Position
            {
                get
                {
                    return Room.Position + Room.Rotation * localposition;
                }
            }

            public Quaternion Rotation
            {
                get
                {
                    return Room.Rotation * localrotation;
                }
            }


            public bool Equals(ItemSpawn other)
            {
                return other.roomType == roomType && other.localposition == localposition && other.localrotation == localrotation;
            }
        }

        public static readonly HashSet<ItemSpawn> AllPoses;
        private static HashSet<ItemSpawn> usablePoses;
        public static IReadOnlyCollection<ItemSpawn> UsablePoses => usablePoses;


        static PoseRoomSpawnPointHandler()
        {
            usablePoses = new HashSet<ItemSpawn>();
            AllPoses = new HashSet<ItemSpawn>();
        }

        public static ItemSpawn UseRandomPose(RoomType roomType)
        {

            if (UsablePoses.Count(r => r.roomType == roomType) <= 0)
            {
                return null;
            }
            Log.Debug("count before =" + UsablePoses.Count(r => r.roomType == roomType));
            ItemSpawn result = UsablePoses.GetRandomValue(r => r.roomType == roomType);
            usablePoses.Remove(result);
            Log.Debug("count after =" + UsablePoses.Count(r => r.roomType == roomType));
            return result;

        }

        public static void AddRoomPose(HashSet<ItemSpawn> poses)
        {

            foreach(ItemSpawn item in poses)
            {
                AddRoomPose(item);
            }

        }

        public static void AddRoomPose(ItemSpawn itemspawn)
        {
            AllPoses.Add(itemspawn);
            usablePoses.Add(itemspawn);
        }


        public static void AddRoomPose(Dictionary<RoomType,List<Vector3>> roomLocalPosition)
        {
            foreach(var kvp in roomLocalPosition)
            {
                foreach (Vector3 position in kvp.Value)
                {
                    ItemSpawn itemSpawn = new ItemSpawn(kvp.Key, position, Quaternion.identity);
                    AddRoomPose(itemSpawn);
                }

                
            }
        }

        public static IEnumerable<ItemSpawn> GetPoseInRoom(RoomType room)
        {
            return AllPoses.Where(p => p.roomType == room);
        }

        public static void Reset()
        {
            usablePoses.Clear();
            foreach(ItemSpawn spawn in AllPoses)
            {
                usablePoses.Add(spawn);
            }
        }

        

    }


    
}
