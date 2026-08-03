using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Features;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    internal class ScanZoneJob : CompositeJob
    {
        public ZoneType ZoneToScan { get; private set; }

        public static ScanZoneJob CreateJob(ZoneType zoneToScan)
        {
            ScanZoneJob job = Pool<ScanZoneJob>.Get();

            job.ZoneToScan = zoneToScan;

            return job;
        }

        private List<Camera> cameras;

        public const float CooldownBetweenCams = .5f;

        private List<ScanResult> results = null;

        public void AddResult(ScanResult scanResult)
        {
            results.Add(scanResult);
        }

        protected override void AttributeJobs()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = true;

            cameras = Camera.Get(c => c.Zone == ZoneToScan).ToList();

            if (results == null)
            {
                results = new List<ScanResult>();
            }
            else
            {
                results.Clear();
            }
            Log.Info("camera number " + cameras.Count);

            foreach (Camera camera in cameras)
            {
                AddJob(SwitchCameraJob.CreateJob(camera));
                AddJob(WaitJob.CreateJob(CooldownBetweenCams));
                AddJob(ScanRoomJob.CreateJob(this));
            }
        }


        protected override void OnCleanup()
        {
            Auto079NPC.GetClosestCameraComponent.Paused = false;
            foreach (ScanResult result in results)
            {
                Log.Info($"found player {result.playerDetected.Id} in {result.room.Name} at {result.playerPosition}");
            }
            base.OnCleanup();
        }


        public override string ToString()
        {
            return "Scan Zone " + ZoneToScan + " " + GetJobCompletion();
        }

        
    }
}
