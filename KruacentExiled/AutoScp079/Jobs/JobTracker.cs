using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using InventorySystem.Items.Firearms.Modules.Scp127;
using KE.Utils.API.Features;
using KruacentExiled.AutoScp079.Jobs.JobType;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079.Jobs
{
    public class JobTracker : MonoBehaviour
    {

        private JobQueue _queue;

        public JobQueue Queue => _queue;


        private Auto079NPC _player;

        private bool init = false;

        private void Awake()
        {
            _queue = new JobQueue();
        }
        public void Init(Auto079NPC player)
        {
            _player = player;
            _currentCooldown = Cooldown;
            if (_player.Debug)
            {
                DrawableLines.IsDebugModeEnabled = true;
            }
            rooms = new List<Room>();

            init = true;
        }

        private const float Cooldown = 20;

        private float _currentCooldown;

        private void Update()
        {
            if (!init) return;
            _queue.Update();


            try
            {
                PingUpdate();
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            

            
            
        }

        private List<Room> rooms;

        private void PingUpdate()
        {

            Camera currentCamera = _player.CurrentCamera;

            if (currentCamera == null)
            {
                return;
            }

            _currentCooldown -= Time.deltaTime;
            if (_currentCooldown > 0)
            {
                return;
            }
            _currentCooldown = Cooldown;


            rooms.Clear();

            rooms.Add(currentCamera.Room);
            

            rooms.AddRange(currentCamera.Room.NearestRooms);

            //Player scanned = Helper.RaycastAPlayer(currentCamera, currentCamera.Room, _player.Debug);

            Player scanned = Helper.RaycastPlayer(currentCamera, rooms, _player.Debug);

            Log.Info("scanned null? " + (scanned == null));
            if (scanned != null)
            {
                Log.Info(scanned.Id);
                _player.RoleNPC.Ping(scanned.Position, PingType.Human, false);
            }





        }

        public void AddJob(Job job)
        {
            if(_player == null)
            {
                return;
            }
            job.Auto079NPC = _player;
            _queue.Add(job);
        }

    }
}
