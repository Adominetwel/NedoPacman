using ConsoleView.ConsoleView;
using DAL;
using DAL.Dapper;
using DAL.EntityFramework;
using Model.Model.Factories;
using Model.Model.User;
using Model.Model.Vectors;
using Model.ViewInterfaces;
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
        private static readonly string ConnectionString = "Host=localhost;Port=5432;Database=nedo_pacman_db;Username=postgres;Password=6752;";
        /// <summary>
        /// Бесполезный метод
        /// </summary>
        /// <param name="args">хз чё это</param>
        static void Main(string[] args)
        {
            var itemFactory = new ObjectFactory<CollectibleItem>();
            var ghostFactory = new ObjectFactory<Ghost>();
            IGameView view = new ConsoleRenderer();
            IInputProvider input = new ConsoleInputProvider();
            ICollisionService collisionService = new CollisionService();
            var userRepo = new EfUserRepository(ConnectionString);
            IAuthService authService = new ConsoleAuthService(userRepo);
            User currentUser = authService.Authenticate();
            var levelRepository = new LevelRepository(ConnectionString, currentUser.Id);
            IGameWorldFactory worldFactory = new GameWorldFactory(ConnectionString, itemFactory, ghostFactory);
            while (true)
            {
                ILevelSelector levelSelector = new ConsoleMenu(levelRepository.ReadAll(), levelRepository);
                Level selectedLevel = levelSelector.SelectLevel().Result;
                GameMap world = worldFactory.CreateWorld(selectedLevel.Id);

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
                    levelRepository.Update(selectedLevel);
                }
                Console.ReadKey(true);
            }
        }
    }
}