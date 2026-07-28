using Exiled.API.Features;
using KE.Utils.API.Features;
using System.Linq;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079
{
    public static class Helper
    {


        public static Camera GetClosestCameraInSameRoom(Player player)
        {
            
            Vector3 position = player.Position;
            Room room = player.CurrentRoom;


            if(room == null || room.Cameras.Count == 0)
            {
                return null;
            }

            if (room.Cameras.Count == 1)
            {
                return room.Cameras.First();
            }


            Camera result = null;
            float distance = float.MaxValue;
            float newDistance;
            foreach (Camera camera in room.Cameras)
            {
                newDistance = Vector3.Distance(position, camera.Position);
                //KELog.Debug("camera : " + camera.Name + " dist : " + newDistance);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    result = camera;
                }
            }
            //KELog.Debug("---");
            return result;


        }
    }
}
