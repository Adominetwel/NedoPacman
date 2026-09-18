using NedoPacmanVuZ.Entities;
using NedoPacmanVuZ.Entities.Collectibles;
using NedoPacmanVuZ.Entities.GhostBehavior;
using NedoPacmanVuZ.FactoryPattern;
using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.Entities.Collectibles;
using NedoPacmanVuZ.Model.Factories;
using NedoPacmanVuZ.View;
using NedoPacmanVuZ.View.ConsoleView;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model
{
    class Program
    {
        static void Main(string[] args)
        {
            var itemFactory = new ObjectFactory<CollectibleItem>();
            var ghostFactory = new ObjectFactory<Ghost>();
            itemFactory.Register("dot.default", (pos) => new DefaultDot(pos));
            itemFactory.Register("dot.energizer", (pos) => new EnergizerDot(pos));

            ghostFactory.Register("ghost.blinky", (pos) => new Ghost(pos, "Blinky", 1, new BlinkyBehavior(), "ghost.blinky"));
            ghostFactory.Register("ghost.pinky", (pos) => new Ghost(pos, "Pinky", 1, new PinkyBehavior(), "ghost.pinky"));
            ghostFactory.Register("ghost.inky", (pos) => new Ghost(pos, "Inky", 1, new InkyBehavior(), "ghost.inky"));
            ghostFactory.Register("ghost.clyde", (pos) => new Ghost(pos, "Clyde", 1, new ClydeBehavior(), "ghost.clyde"));

        int[,] rawMap = {
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1 },
            { 1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1 },
            { 1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1 },
            { 1,4,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,4,1 },
            { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1 },
            { 1,2,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,2,1 },
            { 1,2,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,2,1 },
            { 1,2,2,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,1 },
            { 1,1,1,1,1,1,2,1,1,1,1,1,0,1,1,0,1,1,1,1,1,2,1,1,1,1,1,1 },
            { 0,0,0,0,0,1,2,1,1,1,1,1,0,1,1,0,1,1,1,1,1,2,1,0,0,0,0,0 },
            { 0,0,0,0,0,1,2,1,1,0,0,0,0,0,0,0,0,0,0,1,1,2,1,0,0,0,0,0 },
            { 0,0,0,0,0,1,2,1,1,0,1,1,1,1,1,1,1,1,0,1,1,2,1,0,0,0,0,0 },
            { 1,1,1,1,1,1,2,1,1,0,1,0,0,0,0,0,0,1,0,1,1,2,1,1,1,1,1,1 },
            { 0,0,0,0,0,0,2,0,0,0,1,0,11,12,13,0,0,1,0,0,0,2,0,0,0,0,0,0 },
            { 1,1,1,1,1,1,2,1,1,0,1,0,0,0,0,0,0,1,0,1,1,2,1,1,1,1,1,1 },
            { 0,0,0,0,0,1,2,1,1,0,1,1,1,1,1,1,1,1,0,1,1,2,1,0,0,0,0,0 },
            { 0,0,0,0,0,1,2,1,1,0,0,0,0,10,0,0,0,0,0,1,1,2,1,0,0,0,0,0 },
            { 0,0,0,0,0,1,2,1,1,0,1,1,1,1,1,1,1,1,0,1,1,2,1,0,0,0,0,0 },
            { 1,1,1,1,1,1,2,1,1,0,1,1,1,1,1,1,1,1,0,1,1,2,1,1,1,1,1,1 },
            { 1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1 },
            { 1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1 },
            { 1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1 },
            { 1,4,2,2,1,1,2,2,2,2,2,2,2,52,0,2,2,2,2,2,2,2,1,1,2,2,4,1 },
            { 1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1 },
            { 1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1 },
            { 1,2,2,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,1 },
            { 1,2,1,1,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,1,1,2,1 },
            { 1,2,1,1,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,1,1,2,1 },
            { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
        };
            GameMap world = LoadMap(rawMap, itemFactory, ghostFactory);

            var game = new GameCore(world);
            IGameView view = new ConsoleRenderer();
            IInputProvider input = new ConsoleInputProvider();

            Console.Clear();
            while (!game.IsGameOver)
            {
                Vector2 currentDir = input.GetNextDirection(out bool shootPressed);
                game.Update(currentDir, shootPressed);
                view.Render(game);
                Thread.Sleep(120);
            }
            view.Render(game);
            Thread.Sleep(1000);
            view.ShowGameOver(game.IsWin);
            Console.ReadKey(true);

        }

        private static GameMap LoadMap(int[,] rawMap, ObjectFactory<CollectibleItem> iFact, ObjectFactory<Ghost> gFact)
        {
            var entities = new List<Entity>();
            int height = rawMap.GetLength(0);
            int width = rawMap.GetLength(1);

            var mapping = new Dictionary<int, string> {
                { 1, "wall" }, { 2, "dot.default" }, { 4, "dot.energizer" },
                { 52, "player" }, { 10, "ghost.blinky" }, { 11, "ghost.pinky" },
                { 12, "ghost.inky" }, { 13, "ghost.clyde" }
            };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int code = rawMap[y, x];
                    if (!mapping.TryGetValue(code, out var typeId)) continue;

                    var pos = new Vector2(x, y);
                    if (typeId == "wall") entities.Add(new Wall(pos));
                    else if (typeId == "player") entities.Add(new Player(pos));
                    else if (typeId.StartsWith("dot.")) entities.Add(iFact.Create(typeId, pos));
                    else if (typeId.StartsWith("ghost.")) entities.Add(gFact.Create(typeId, pos));
                }
            }
            return new GameMap(width, height, entities);
        }
    }
}