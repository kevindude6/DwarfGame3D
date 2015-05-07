using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D
{
    class RandomFunctions
    {
        public static float Normal(Random r)
        {
            return Normal(r, 1, 0);
        }
        public static float Normal(Random r, float stdDev, float mean)
        {

            double u1 = r.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = r.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return (float)(mean + stdDev * randStdNormal); //random normal(mean,stdDev^2)
        }
        public static Vector3 vecBetweenMinMax(Random r, Vector3 min, Vector3 max)
        {
            Vector3 newVec= new Vector3(0,0,0);
            newVec.X = (float)r.NextDouble() * (max.X - min.X) + min.X;
            newVec.Y = (float)r.NextDouble() * (max.Y - min.Y) + min.Y;
            newVec.Z = (float)r.NextDouble() * (max.Z - min.Z) + min.Z;
            return newVec;
        }
        public static Color randSaturatedColor(Random r)
        {
            
            //http://stackoverflow.com/questions/8476841/how-to-random-generate-fully-saturated-colors-only
             int[] rgb = new int[3];
             rgb[0] = r.Next(256);  // red
             rgb[1] = r.Next(256);  // green
             rgb[2] = r.Next(256);  // blue

            // Console.WriteLine("{0},{1},{2}", rgb[0], rgb[1], rgb[2]);

             // find max and min indexes.
            int max, min;

            if (rgb[0] > rgb[1])
            {
                max = (rgb[0] > rgb[2]) ? 0 : 2;
                min = (rgb[1] < rgb[2]) ? 1 : 2;
            }
            else
            {
                max = (rgb[1] > rgb[2]) ? 1 : 2;
                int notmax = 1 + max % 2;
                min = (rgb[0] < rgb[notmax]) ? 0 : notmax;
            }
            rgb[max] = 255;
            rgb[min] = 0;
            return new Color(rgb[0],rgb[1],rgb[2]);
             
            /*
            int a = r.Next(0, 4);
            switch (a)
            {
                case 0: return Color.Orange;
                case 1: return Color.OrangeRed;
                case 2: return Color.Red;
                case 3: return Color.Yellow;
            }
            return Color.OrangeRed;
             */
        }
    }
}
