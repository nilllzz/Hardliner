using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Reflection;

namespace Hardliner.Core
{
    internal static class CommonExtensions
    {
        public static Vector3 NextUnitSphereVector(this Random rnd)
        {
            var values = new double[3];
            for (var i = 0; i < values.Length; i++)
                values[i] = rnd.NextDouble() * 2 - 1;

            return new Vector3((float)values[0], (float)values[1], (float)values[2]);
        }

        public static bool Chance(this Random rnd, int chance)
        {
            return rnd.Next(0, chance + 1) == 0;
        }

        public static void SetGraphicsProfile(this GraphicsDevice graphicsDevice, GraphicsProfile profile)
        {
            // HACK
            // removes chromosomes
            var fields = typeof(GraphicsDevice).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            fields.First(f => f.Name == "_graphicsProfile").SetValue(graphicsDevice, GraphicsProfile.HiDef);
        }
    }
}
