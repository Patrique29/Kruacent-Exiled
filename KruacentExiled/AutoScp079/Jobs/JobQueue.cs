using Exiled.API.Features;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs
{
    public class JobQueue
    {
        private readonly Queue<Job> _jobs = new Queue<Job>();
        private Job _lastadded = null;

        public int QueueSize => _jobs.Count;

        public Job Peek()
        {
            return _jobs.Peek();
        }

        public void Add(Job job)
        {
            if (QueueSize > 0)
            {

                if (job.Equals(_lastadded))
                {
                    return;
                }
                Job peek = Peek();

                if (peek is IInfiniteJob)
                {
                    Log.Info("cancelling infinite " + peek.ToString());
                    CancelCurrent();
                }
            }
            

            

            _jobs.Enqueue(job);
            _lastadded = job;
            //KELog.Debug("adding job");
        }


        public void CancelCurrent()
        {
            if (_jobs.Count == 0)
            {
                return;
            }

            Job job = _jobs.Peek();

            job.Cancel();
        }
        

        public void Update()
        {
            if (_jobs.Count == 0)
            {
                return;
            }
                

            Job job = _jobs.Peek();

            job.Tick();

            if (job.IsFinished || job.IsCancelled)
            {
                Log.Info("dequeuing : " + job.ToString() + " reason : finished? "+ job.IsFinished);
                job.Cleanup();
                _jobs.Dequeue();
            }
                
        }

        public void Clear()
        {
            _jobs.Clear();
        }
    }
}
