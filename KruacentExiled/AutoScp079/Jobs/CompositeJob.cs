using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.AutoScp079.Jobs
{
    public abstract class CompositeJob : Job
    {
        private Queue<Job> _jobs = null;


        protected Queue<Job> Jobs => _jobs;


        public int NumberJobDone
        {
            get
            {
                if(Jobs == null || _totalJob < 0)
                {
                    return -1;
                }

                return TotalJobs - Jobs.Count;
            }
        }

        public int TotalJobs => _totalJob;

        private int _totalJob = -1;

        public override void OnStart()
        {
            if (_jobs == null)
            {
                _jobs = new Queue<Job>();
            }
            else
            {
                _jobs.Clear();
            }
            AttributeJobs();
            _totalJob = _jobs.Count;
        }


        protected abstract void AttributeJobs();

        protected override void OnUpdate()
        {
            if (_jobs.Count == 0)
            {
                EndJob();
                return;
            }

            Job job = _jobs.Peek();

            if (!job.Started)
            {

                job.Started = true;
                try
                {
                    job.OnStart();

                }
                catch (Exception e)
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

        protected void AddJob(Job job)
        {
            job.Auto079NPC = Auto079NPC;
            _jobs.Enqueue(job);
        }

        protected override void OnCancel()
        {
            
            _jobs.Peek().Cancel();
            _jobs.Clear();
            base.OnCancel();

        }

        protected override void OnCleanup()
        {
            _jobs.Clear();
            _totalJob = -1;
            base.OnCleanup();
        }

        public string GetJobCompletion()
        {
            return $"{NumberJobDone}/{TotalJobs}";
        }

    }
}
