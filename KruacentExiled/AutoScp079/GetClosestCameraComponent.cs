using Exiled.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.AutoScp079.Jobs.JobType;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079
{
    public class GetClosestCameraComponent : MonoBehaviour
    {

        private Player _player;
        private Auto079NPC _comp;

        private float _cooldown;
        private const float Cooldown = .5f;

        internal void Init(Player player, Auto079NPC comp)
        {
            _player = player;
            _comp = comp;
            Paused = false;
        }

        public bool Paused { get; set; }



        private void Update()
        {

            if(Paused)
            {
                return;
            }

            _cooldown -= Time.deltaTime;
            if(_cooldown > 0)
            {
                return;
            }

            _cooldown = Cooldown;



            Camera closest = Helper.GetClosestCameraInSameRoom(_player);

            //KELog.Debug("closest="+closest.Name);
            //KELog.Debug("curr="+ _comp.CurrentCamera.Name);
            //KELog.Debug("different?="+ (closest != _comp.CurrentCamera));


            if(closest != _comp.CurrentCamera)
            {
                Log.SendRaw("changing cam from " + _comp.CurrentCamera.Name + " to " + closest.Name, System.ConsoleColor.DarkRed);
                Log.SendRaw("queue size=" + _comp.JobTracker.Queue.QueueSize, System.ConsoleColor.DarkRed);


                SwitchCameraJob job = SwitchCameraJob.CreateJob(closest);
                _comp.JobTracker.AddJob(job);
                //_comp.JobTracker.AddJob(JobWait.CreateJob(Cooldown));
            }

        }



    }
}
