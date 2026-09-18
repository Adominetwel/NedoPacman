using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities;
using System.Runtime.InteropServices;
using System.Text;

namespace NedoPacmanVuZ.View.ConsoleView
{
    internal class ConsoleRenderer : IGameView
    {
        private const int BlockWidth = 3;
        private const int BlockHeight = 2;
        private int _animationFrame = 0;

        private readonly Dictionary<string, (ConsoleColor Color, string TopSymbol, string BottomSymbol)> _renderConfig = new()
        {
            { "wall", (ConsoleColor.Blue, "███", "███") },
            { "player", (ConsoleColor.Yellow, " ☺ ", "   ") },
            { "dot.default", (ConsoleColor.DarkGray, " · ", "   ") },
            { "dot.energizer", (ConsoleColor.DarkGray, " ● ", "   ") },
            { "projectile", (ConsoleColor.Red, " ¤ ", "   ") },
            { "ghost.blinky", (ConsoleColor.Red, " █B", " m ") },
            { "ghost.pinky", (ConsoleColor.Magenta, " █P", " m ") },
            { "ghost.inky", (ConsoleColor.Cyan, " █I", " m ") },
            { "ghost.clyde", (ConsoleColor.DarkYellow, " █C", " m ") }
        };

        public ConsoleRenderer()
        {
            if (OperatingSystem.IsWindows())
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                static extern IntPtr GetConsoleWindow();
                [DllImport("user32.dll", SetLastError = true)]
                static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
                [DllImport("user32.dll", SetLastError = true)]
                static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
                const uint GW_OWNER = 4;
                const int SW_MAXIMIZE = 3; 
                IntPtr hWnd = GetConsoleWindow();
                if (hWnd != IntPtr.Zero)
                {
                    IntPtr hTerminalWnd = GetWindow(hWnd, GW_OWNER);
                    IntPtr targetWnd = hTerminalWnd != IntPtr.Zero ? hTerminalWnd : hWnd;
                    ShowWindow(targetWnd, SW_MAXIMIZE);
                }
            }
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void Render(GameCore model)
        {
            _animationFrame++;
            Console.SetCursorPosition(0, 0);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"СЧЕТ: {model.Score.ToString().PadRight(8)} | ЗАРЯДЫ: {model.AmmoCount.ToString().PadRight(3)} | ");

            if (model.CurrentGhostMode == GhostMode.Frightened)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("FRIGHTENED MODE ");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("[");
                int progress = (int)Math.Ceiling(model.FrightenedTicksLeft / 4.0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string('█', progress));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(new string('░', Math.Max(0, 10 - progress)));
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("]");
            }
            else if (model.CurrentGhostMode == GhostMode.Scatter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("SCATTER MODE ".PadRight(27));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("CHASE MODE ".PadRight(27));
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('─', model.World.Width * BlockWidth));

            for (int y = 0; y < model.World.Height; y++)
            {
                for (int subY = 0; subY < BlockHeight; subY++)
                {
                    for (int x = 0; x < model.World.Width; x++)
                    {
                        Entity? entity = model.World.GetEntityAt(new Vector2(x, y));

                        if (entity == null)
                        {
                            Console.Write("   ");
                            continue;
                        }

                        if (entity is Ghost ghost)
                        {
                            if (ghost.State == GhostState.InCage)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(subY == 0 ? " █░" : " m ");
                            }
                            else if (model.CurrentGhostMode == GhostMode.Frightened)
                            {
                                if (model.FrightenedTicksLeft <= 12 && _animationFrame % 2 == 0)
                                    Console.ForegroundColor = ConsoleColor.White;
                                else
                                    Console.ForegroundColor = ConsoleColor.Blue;

                                Console.Write(subY == 0 ? " █F" : " m ");
                            }
                            else
                            {
                                if (_renderConfig.TryGetValue(entity.TypeId, out var config))
                                {
                                    Console.ForegroundColor = config.Color;
                                    Console.Write(subY == 0 ? config.TopSymbol : config.BottomSymbol);
                                }
                            }
                        }
                        else if (_renderConfig.TryGetValue(entity.TypeId, out var config))
                        {
                            Console.ForegroundColor = config.Color;
                            Console.Write(subY == 0 ? config.TopSymbol : config.BottomSymbol);
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    Console.WriteLine();
                }
            }
            Console.ResetColor();
        }

        public void ShowGameOver(bool isWin)
        {
            Console.Clear();
            Console.ForegroundColor = isWin ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine("\n\n");
            if (isWin)
            {
                Console.WriteLine("  ██████╗ ██████╗ ██████╗ ███████╗██████╗  █████╗ ");
                Console.WriteLine("  ██╔══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔══██╗");
                Console.WriteLine("  ██████╔╝██║  ██║██████╔╝█████╗  ██║  ██║███████║");
                Console.WriteLine("  ██╔═══╝ ██║  ██║██╔══██╗██╔══╝  ██║  ██║██╔══██║");
                Console.WriteLine("  ██║     ██████╔╝██████╔╝███████╗██████╔╝██║  ██║");
                Console.WriteLine("  ╚═╝     ╚═════╝ ╚═════╝ ╚══════╝╚═════╝ ╚═╝  ╚═╝");
            }
            else
            {
                Console.WriteLine("  I G R A    O K O N C H E N A");
            }
            Console.ResetColor();
        }
    }

}
