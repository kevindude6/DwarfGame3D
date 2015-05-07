using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.Entities.Creatures
{
    public class Turtle : LivingEntity
    {
        new public static string SheetName = "Turtle";

        public Turtle(Vector3 pos, float thealth): base(pos, thealth)
        {
            collideable = true;
        }
        public Turtle(Vector3 pos) : this(pos,25)
        {
            
        }
        public override void initSize()
        {
            renderWidth = 0.5f;
            collideRadius = 0.4f;
            height = 0.5f;
            mass = 50;
        }
    }
}
