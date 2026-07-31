using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    internal class ScanZoneJob : Job
    {
        public ZoneType ZoneToScan { get; private set; }

        public static ScanZoneJob CreateJob(ZoneType zoneToScan)
        {
            ScanZoneJob job = Pool<ScanZoneJob>.Get();

            job.ZoneToScan = zoneToScan;

            return job;
        }

        private List<Camera> cameras;

        private Queue<Job> _jobs = null;

        private List<ScanResult> results = null;

        public void AddResult(ScanResult scanResult)
        {
            results.Add(scanResult);
        }

        public override void OnStart()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = true;

            cameras = Camera.Get(c => c.Zone == ZoneToScan).ToList();

            if(results == null)
            {
                results = new List<ScanResult>();
            }
            else
            {
                results.Clear();
            }

            if(_jobs == null)
            {
                _jobs = new Queue<Job>();
            }
            else
            {
                _jobs.Clear();
            }

            Log.Info("camera number "+ cameras.Count);

            foreach (Camera camera in cameras)
            {
                AddJob(SwitchCameraJob.CreateJob(camera));
                AddJob(WaitJob.CreateJob(.1f));
                AddJob(ScanRoomJob.CreateJob(this));
            }
        }
        

        private void AddJob(Job job)
        {
            job.Auto079NPC = Auto079NPC;
            _jobs.Enqueue(job);
        }


        private float i = 0;

        protected override void OnUpdate()
        {
            if (_jobs.Count == 0)
            {
                EndJob();
                return;
            }

            Job job = _jobs.Peek();
            i += Time.deltaTime;

            if(i > 2)
            {
                i = 0;
            }

            if (!job.Started)
            {

                job.Started = true;
                try
                {
                    job.OnStart();

                }
                catch(Exception e)
                {
                    Log.Error(e);
                }
                
            }



            job.Tick();

            if (job.IsFinished || job.IsCancelled)
            {
                job.Cleanup();
                _jobs.Dequeue();
            }
        }


        protected override void OnCancel()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = false;
            _jobs.Peek().Cancel();
            _jobs.Clear();
            base.OnCancel();

        }

        protected override void OnFinished()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = false;
            base.OnFinished();
        }

        protected override void OnCleanup()
        {
            foreach(ScanResult result in results)
            {
                Log.Info($"found player {result.playerDetected.Id} in {result.room.Name} at {result.playerPosition}");
            }



            _jobs.Clear();
            base.OnCleanup();
        }

        public override string ToString()
        {
            return "Scan Zone " + ZoneToScan;
        }

        
    }
}
