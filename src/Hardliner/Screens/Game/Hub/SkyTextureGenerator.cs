using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game.Hub
{
    internal static class SkyTextureGenerator
    {
        private static Random _random = new Random();
        
        internal static Texture2D Generate(int width, int height, int clouds)
        {
            var texture = new Texture2D(GameInstance.GraphicsDevice, width, height);
            var data = new Color[texture.Width * texture.Height];

            var levelDefinitions = new int[]
            {
                (int)(texture.Height * 0.7f),
                (int)(texture.Height * 0.6f),
                (int)(texture.Height * 0.5f),
                (int)(texture.Height * 0.4f)
            };

            var levels = (int[])levelDefinitions.Clone();
            var targetLevels = (int[])levelDefinitions.Clone();

            for (int x = 0; x < texture.Width; x++)
            {
                for (int i = 0; i < levels.Length; i++)
                {
                    if (levels[i] == targetLevels[i])
                    {
                        if (x >= width * 0.8f)
                        {
                            targetLevels[i] = levelDefinitions[i];
                        }
                        else
                        {
                            if (_random.Chance(2))
                            {
                                if (x > texture.Width / 2)
                                    targetLevels[i] = _random.Next(-2, 6) + levels[i];
                                else
                                    targetLevels[i] = _random.Next(-5, 3) + levels[i];
                            }
                        }
                    }

                    if (levels[i] < targetLevels[i] && _random.Chance(3))
                        levels[i]++;
                    if (levels[i] > targetLevels[i] && _random.Chance(3))
                        levels[i]--;
                }

                for (int y = 0; y < texture.Height; y++)
                {
                    var index = x + y * texture.Width;
                    if (y < levels[3])
                    {
                        data[index] = Color.Black;
                    }
                    else if (y < levels[2])
                    {
                        if (!(x % 2 == 0 && y % 2 == 0))
                        {
                            data[index] = Color.Black;
                        }
                    }
                    else if (y < levels[1] && (x + (y % 2)) % 2 == 0)
                    {
                        data[index] = Color.Black;
                    }
                    else if (y < levels[0] && x % 2 == 0 && y % 2 == 0)
                    {
                        data[index] = Color.Black;
                    }
                    else
                    {
                        data[index] = Color.Transparent;
                    }
                }
            }

            for (int i = 0; i < clouds; i++)
            {
                var cloudWidth = _random.Next(20, 50);
                var cloudHeight = 0;
                var posX = _random.Next(0, width - cloudWidth);
                var posY = _random.Next(10, (int)(height * 0.7f));
                var startY = 0;

                for (int x = 0; x < cloudWidth; x++)
                {
                    if (x > cloudWidth / 2)
                    {
                        if (_random.Chance(3))
                            cloudHeight--;
                        if (_random.Chance(3))
                            startY++;
                    }
                    else
                    {
                        if (_random.Chance(2))
                            cloudHeight++;
                        if (_random.Chance(2))
                            startY--;
                    }

                    if (startY < 0)
                        startY = 0;

                    for (int y = 0; y < cloudHeight; y++)
                    {
                        var destY = startY + posY + y;
                        var destX = posX + x;
                        var index = destX + destY * texture.Width;

                        if (posY < levels[2])
                        {
                            if (!(destX % 2 == 0 && destY % 2 == 0))
                            {
                                data[index] = Color.Black;
                            }
                            else
                            {
                                data[index] = Color.Transparent;
                            }
                        }
                        else if (posY < levels[1])
                        {
                            if ((destX + (destY % 2)) % 2 == 0)
                            {
                                data[index] = Color.Black;
                            }
                            else
                            {
                                data[index] = Color.Transparent;
                            }
                        }
                        else if (posY < levels[0])
                        {
                            if (x % 2 == 0 && y % 2 == 0)
                            {
                                data[index] = Color.Black;
                            }
                            else
                            {
                                data[index] = Color.Transparent;
                            }
                        }
                    }
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
}
