﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.Entities.Creatures
{
    public class Human : LivingEntity
    {
        new public static string SheetName = "Human";
        public enum JOBS {Mining, Chopping};
        private static int jobsNum = 2;
        public byte[] jobProficiency = new byte[jobsNum];
        public bool[] jobEnabled = new bool[jobsNum];
        public float[] jobEXP = new float[jobsNum];

        public Human(Vector3 pos, float thealth): base(pos, thealth)
        {
            collideable = true;
        }
        public Human(Vector3 pos) : this(pos,150)
        {
            jobProficiency[(int)JOBS.Mining] = 255;
        }
        public override void initSize()
        {
            renderWidth = 0.75f;
            collideRadius = 0.4f;
            height = 0.95f;
            mass = 150;
        }
    }
}
