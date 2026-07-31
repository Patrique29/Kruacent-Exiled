using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.LowLevel;
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="room"></param>
        /// <param name="debug"></param>
        /// <returns>An <see cref="Enumerable"/> of <see cref="Player"/>s or null if nothing was found </returns>
        public static IEnumerable<Player> RaycastPlayer(Camera camera, Room room, bool debug = false)
        {
            HashSet<Player> result = new HashSet<Player>();
            foreach (Player nonScp in room.Players.Where(p => !SCPTeam.IsSCP(p.ReferenceHub) && p.Role is FpcRole))
            {
                FpcRole fpc = nonScp.Role as FpcRole;

                Vector3 start = camera.Position;
                Vector3 end;
                Vector3 direction;



                foreach (HitboxIdentity hitboxIdentity in fpc.Model.Hitboxes)
                {

                    foreach (Collider collider in hitboxIdentity.TargetColliders)
                    {
                        end = collider.transform.position;
                        direction = end - start;

                        if (Physics.Raycast(start, direction, out RaycastHit hit, 50, (int)(LayerMasks.Default | LayerMasks.Hitbox | LayerMasks.Door)))
                        {
                            if (debug)
                            {
                                Vector3 middle = hit.point;
                                DrawableLines.GenerateLine(10, Color.blue, start, middle);
                                DrawableLines.GenerateLine(10, Color.red, middle, end);
                            }

                            if(Player.TryGet(hit.collider,out Player player) && !SCPTeam.IsSCP(player.ReferenceHub))
                            {
                                result.Add(player);
                            }

                            
                        }
                    }
                }

            }


            return result;
        }

        public static Player RaycastPlayer(Camera camera,IEnumerable<Room> rooms, bool debug = false)
        {
            List<Player> players = ListPool<Player>.Pool.Get();


            if(rooms is null)
            {
                throw new ArgumentNullException("rooms");
            }

            foreach (Room room in rooms)
            {
                foreach (Player playerRoom in room.Players)
                {
                    if (!SCPTeam.IsSCP(playerRoom.ReferenceHub) && playerRoom.Role is FpcRole)
                    {
                        players.Add(playerRoom);
                    }
                }
            }



            Player player = null;
            bool playerSet = false;
            foreach (Player nonScp in players)
            {
                FpcRole fpc = nonScp.Role as FpcRole;

                Vector3 start = camera.Position;
                Vector3 end;
                Vector3 direction;



                foreach (HitboxIdentity hitboxIdentity in fpc.Model.Hitboxes)
                {

                    foreach (Collider collider in hitboxIdentity.TargetColliders)
                    {
                        end = collider.transform.position;
                        direction = end - start;

                        if (Physics.Raycast(start, direction, out RaycastHit hit, 50, (int)(LayerMasks.Default | LayerMasks.Hitbox | LayerMasks.Door)))
                        {
                            if (debug)
                            {
                                Vector3 middle = hit.point;
                                DrawableLines.GenerateLine(10, Color.blue, start, middle);
                                DrawableLines.GenerateLine(10, Color.red, middle, end);
                            }

                            if (!playerSet)
                            {
                                player = Player.Get(hit.collider);
                                //Log.Info($"name: {hit.collider.name} is player? {player != null} ");
                            }
                            

                            

                            if(player != null && !SCPTeam.IsSCP(player.ReferenceHub))
                            {
                                playerSet = true;

                                if (!debug)
                                {
                                    return player;
                                }
                            }
                        }
                    }
                }

            }

            //KELog.Debug("player null? " + player == null);

            return player;
        }
    
    
    }
}
