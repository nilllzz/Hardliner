using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hardliner.Core
{
    internal static class CommonExtensions
    {
        public static Vector3 NextUnitSphereVector(this Random rnd)
        {
            var values = new double[3];
            for (int i = 0; i < values.Length; i++)
                values[i] = rnd.NextDouble() * 2 - 1;

            return new Vector3((float)values[0], (float)values[1], (float)values[2]);
        }

        public static bool Chance(this Random rnd, int chance)
        {
            return rnd.Next(0, chance + 1) == 0;
        }
    }
}
