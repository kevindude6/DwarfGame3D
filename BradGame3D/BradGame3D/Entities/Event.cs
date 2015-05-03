using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BradGame3D.Entities
{
    public class Event
    {
        //public long startTime;
        public float timeDelay;
        public delegate void Task();
        Task mTask = null;

        public Event(float d, Task t)
        {
            mTask = t;
            timeDelay = d;

        }
        public void updateEvent(ref List<Event> l, float gameTime)
        {
            timeDelay -= gameTime;
            //Debug.WriteLine(timeDelay);
            if (timeDelay <= 0)
            {
                mTask();
                l.Remove(this);
            }
            
        }
    }
}
