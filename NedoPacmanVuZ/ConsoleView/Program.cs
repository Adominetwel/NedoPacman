using NedoPacmanVuZ.FileManager;
using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.Entities.Collectibles;
using NedoPacmanVuZ.Model.Entities.GhostBehavior;
using NedoPacmanVuZ.Model.Factories;
using NedoPacmanVuZ.Model.GameLevel;
using NedoPacmanVuZ.Model.MainLogic;
using NedoPacmanVuZ.View;
using NedoPacmanVuZ.View.ConsoleView;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model
{
    class Program
    {
        /// <summary>
        /// Бесполезный метод
        /// </summary>
        /// <param name="args">хз чё это</param>
        static void Main(string[] args)
        {
            var itemFactory = new ObjectFactory<CollectibleItem>();
            var ghostFactory = new ObjectFactory<Ghost>();
            itemFactory.Register("dot.default", (pos) => new DefaultDot(pos));
            itemFactory.Register("dot.energizer", (pos) => new EnergizerDot(pos));

            ghostFactory.Register("ghost.blinky", (pos) => new Ghost(pos, "Blinky", 1, new BlinkyBehavior(), "ghost.blinky", false));
            ghostFactory.Register("ghost.pinky", (pos) => new Ghost(pos, "Pinky", 1, new PinkyBehavior(), "ghost.pinky", true));
            ghostFactory.Register("ghost.inky", (pos) => new Ghost(pos, "Inky", 1, new InkyBehavior(), "ghost.inky", true));
            ghostFactory.Register("ghost.clyde", (pos) => new Ghost(pos, "Clyde", 1, new ClydeBehavior(), "ghost.clyde", true));

            IGameView view = new ConsoleRenderer();
            IInputProvider input = new ConsoleInputProvider();
            IProgressStorage progressStorage = new JsonProgressService();
            ICollisionService collisionService = new CollisionService();
            var levelRepository = new LevelRepository(progressStorage);

            while (true)
            {
                // Запрашиваем данные через унифицированный метод чтения всей коллекции
                ILevelSelector levelSelector = new ConsoleMenu(levelRepository.ReadAll(), levelRepository);

                Level selectedLevel = levelSelector.SelectLevel().Result;

                GameMap world = LoadMap(selectedLevel, itemFactory, ghostFactory);
                var game = new GameCore(world, selectedLevel.GameMode, collisionService);

                Console.Clear();
                while (!game.IsGameOver)
                {
                    Vector2 currentDir = input.GetNextDirection(out bool shootPressed);
                    game.Update(currentDir, shootPressed);
                    view.Render(game);
                    Thread.Sleep(150);
                }
                view.Render(game);
                Thread.Sleep(1000);
                view.ShowGameOver(game.IsWin);
                if (game.IsWin)
                {
                    selectedLevel.IsPassed = true;
                    levelRepository.Update(selectedLevel); // Сохранение через метод Update репозитория
                }
                Console.ReadKey(true);
            }
        }

        private static GameMap LoadMap(Level level, ObjectFactory<CollectibleItem> iFact, ObjectFactory<Ghost> gFact)
        {
            var entities = new List<Entity>();
            int[,] rawMap = level.RawMap;

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
                    else if (typeId.StartsWith("ghost."))
                    {
                        Ghost ghost = gFact.Create(typeId, pos);
                        if (ghost != null) entities.Add(ghost);
                    }
                }
            }

            // Полностью чистый проброс параметров без хардкода по ширине
            return new GameMap(width, height, entities, level.Config.CagePositions, level.Config.CageExitPosition);
        }
    }
}