using System;
using Hardliner.Core;
using Microsoft.Xna.Framework;

internal static class Core
{
    [STAThread]
    private static void Main(string[] args)
    {
        using (GameInstance = new GameController())
            GameInstance.Run();
    }
    
    internal static GameController GameInstance { get; private set; }

    internal static T GetComponent<T>() where T : IGameComponent => GameInstance.GetComponent<T>();
}
