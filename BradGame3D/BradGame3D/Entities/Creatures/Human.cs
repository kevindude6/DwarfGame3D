using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.Entities.Creatures
{
    public class Human : LivingEntity
    {
        public Human(Vector3 pos, float thealth): base(pos, thealth)
        {
            collideable = true;
        }
        public Human(Vector3 pos) : this(pos,150)
        {
            
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
