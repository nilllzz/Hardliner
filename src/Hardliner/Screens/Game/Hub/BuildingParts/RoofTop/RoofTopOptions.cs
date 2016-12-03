using System;

namespace Hardliner.Screens.Game.Hub.BuildingParts.RoofTop
{
    [Flags]
    internal enum RoofTopOptions
    {
        None = 0,
        Entrance = 1 << 0,
        RedWarningLights = 1 << 1,
        Fans = 1 << 2,
    }
}
