using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.Entities.Creatures
{
    public class Human : LivingEntity
    {
        new public static string SheetName = "Human";
       

        public Human(Vector3 pos, float thealth): base(pos, thealth)
        {

        }
        public Human(Vector3 pos) : base(pos,150)
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
