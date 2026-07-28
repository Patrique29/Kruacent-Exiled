using Exiled.API.Extensions;
using Exiled.API.Features;
using KruacentExiled.AutoScp079.Jobs.JobType;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.AutoScp079.Jobs
{
    public class JobTracker : MonoBehaviour
    {

        private JobQueue _queue;

        public JobQueue Queue => _queue;


        private Auto079NPC _player;

        private void Awake()
        {
            _queue = new JobQueue();
        }
        public void Init(Auto079NPC player)
        {
            _player = player;
            _currentCooldown = Cooldown;
        }

        private const float Cooldown = 20;

        private float _currentCooldown;

        private void Update()
        {
            _queue.Update();

            PingUpdate();

            
            
        }

        private void PingUpdate()
        {
            _currentCooldown -= Time.deltaTime;
            if (_currentCooldown > 0)
            {
                return;
            }
            Player player = _player.CurrentCamera.Room.Players.Where(p => !SCPTeam.IsSCP(p.ReferenceHub)).GetRandomValue();

            if(player == null)
            {
                return;
            }
            _currentCooldown = Cooldown;

            _player.RoleNPC.Ping(player.Position, Exiled.API.Enums.PingType.Human, false);
            
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
