using Exiled.API.Features;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.AutoScp079.Jobs
{
    public abstract class Job : IEquatable<Job>
    {
        public bool Started { get; private set; }
        public bool IsFinished { get; private set; }

        public Auto079NPC Auto079NPC { get; internal set; }

        protected abstract void OnUpdate();


        protected virtual void OnStart()
        {

        }

        protected virtual void OnFinished()
        {

        }
        protected virtual void OnCleanup()
        {

        }
        public void Tick()
        {
            if (!Started)
            {
                Started = true;
                OnStart();
            }

            if (!IsFinished)
            {
                OnUpdate();
            }
        }


        public void EndJob()
        {
            IsFinished = true;
            OnFinished();
            KELog.Debug("ending job " + GetType().Name);

        }

        public void Cleanup()
        {
            OnCleanup();
            Started = false;
            IsFinished = false;
            Auto079NPC = null;
        }


        public virtual bool Equals(Job other)
        {
            return other == this;
        }


        public override abstract string ToString();

    }
}
