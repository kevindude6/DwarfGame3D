using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.PlayerInteraction
{
    public class Selection
    {
        public BoundingBox bounds;
        public SelectionManager.JOBTYPE mJobType;
        public struct Job
        {
            public int x;
            public int y;
            public int z;
            public SelectionManager.JOBTYPE jobType;
            public MouseIndicator m;
        }
        public List<Job> jobsInSelection = new List<Job>();
        public Selection(BoundingBox b, SelectionManager.JOBTYPE tempjob)
        {
            bounds = b;
            mJobType = tempjob;
        }
        public void addJob(Job j)
        {
            jobsInSelection.Add(j);
        }
    }
}
