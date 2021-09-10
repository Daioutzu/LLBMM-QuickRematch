using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuickRematch
{
    class Timer
    {
        public Timer(float _waitTime)
        {
            waitTime = _waitTime;
            timePrev = Time.realtimeSinceStartup;
        }

        public bool keepWaiting
        {
            get
            {
                if (timeCanceled == false)
                {
                    waitTime -= Time.realtimeSinceStartup - timePrev;
                    timePrev = Time.realtimeSinceStartup;
                    return waitTime > 0f;
                } else { return true; }
            }
        }

        public float waitTime { get; private set; }
        private float timePrev;
        public bool timeCanceled;

    }
}
