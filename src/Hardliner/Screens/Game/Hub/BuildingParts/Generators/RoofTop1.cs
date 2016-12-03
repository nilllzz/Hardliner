using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Screens.Game.Hub.BuildingParts.RoofTop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Hardliner.Screens.Game.Hub.BuildingParts.Generators
{
    internal static class RoofTop1
    {
        internal static IEnumerable<LevelObject> Generate(Level level, ContentManager content, 
            Vector3 position, Vector2 size,
            RoofTopOptions options = RoofTopOptions.None)
        {
            var objects = new List<LevelObject>();

            var halfWidth = size.X / 2f;
            var halfDepth = size.Y / 2f;

            objects.Add(new Wall(level, content, new Vector3(-(halfWidth - 0.5f), 0, 0) + position, new Vector2(size.Y - 1f, 1), true));
            objects.Add(new Wall(level, content, new Vector3(halfWidth - 0.5f, 0, 0) + position, new Vector2(size.Y - 1f, 1), true));

            objects.Add(new Wall(level, content, new Vector3(0, 0, halfDepth) + position, new Vector2(size.X, 1), false));
            objects.Add(new Wall(level, content, new Vector3(0, 0, -halfDepth) + position, new Vector2(size.X, 1), false));

            objects.Add(new Floor(level, content, new Vector3(0, 0, 0) + position, new Vector2(size.X, size.Y)));

            if (options.HasFlag(RoofTopOptions.Entrance))
            {
                objects.Add(new Entrance(level, content, new Vector3(halfWidth - 2.5f, 0, 0) + position));
            }
            if (options.HasFlag(RoofTopOptions.RedWarningLights))
            {
                objects.Add(new RedWarningLight(level, new Vector3(-halfWidth, 0, -halfDepth - 0.5f) + position));
                objects.Add(new RedWarningLight(level, new Vector3(halfWidth, 0, -halfDepth - 0.5f) + position));
                objects.Add(new RedWarningLight(level, new Vector3(-halfWidth, 0, halfDepth + 0.5f) + position));
                objects.Add(new RedWarningLight(level, new Vector3(halfWidth, 0, halfDepth + 0.5f) + position));
            }
            if (options.HasFlag(RoofTopOptions.Fans))
            {
                objects.Add(new Fan(level, content, new Vector3(-2f, 0, -0.5f) + position));
                objects.Add(new Fan(level, content, new Vector3(-2f, 0, 0.5f) + position));
            }
            
            return objects;
        }
    }
}
