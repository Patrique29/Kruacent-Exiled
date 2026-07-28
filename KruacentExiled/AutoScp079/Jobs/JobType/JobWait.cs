using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.AutoScp079.Jobs.JobType
{
    internal sealed class JobWait : Job
    {

        private float _timeWaiting;

        private float _remaining;



        public static JobWait CreateJob(float timeWaiting)
        {
            JobWait job = Pool<JobWait>.Get();


            job._timeWaiting = timeWaiting;
            return job;

        }

        public JobWait()
        {
            
        }

        protected override void OnUpdate()
        {
            _remaining -= Time.deltaTime;

            if (_remaining <= 0)
            {
                EndJob();
            }
        }


        protected override void OnFinished()
        {
            KELog.Debug("finished waiting job");
        }
        protected override void OnStart()
        {
            KELog.Debug("starting waiting job : waiting for "+ _timeWaiting + " seconds");
            _remaining = _timeWaiting;
        }

        protected override void OnCleanup()
        {
            Pool<JobWait>.Return(this);
        }

        public override string ToString()
        {
            return "Wait";
        }
    }
}
