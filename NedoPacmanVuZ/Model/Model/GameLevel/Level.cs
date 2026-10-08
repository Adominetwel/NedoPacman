using Model.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameLevel
{
    public class Level
    {
        public int Id { get; }
        public string Name { get; }
        public int[,] RawMap { get; }
        public IGameMode GameMode { get; }
        public LevelConfig Config { get; }
        public bool IsPassed { get; set; } = false;
        public Level() { }
        public Level(int id, string name, int[,] rawMap, IGameMode gameMode, LevelConfig config)
        {
            Id = id;
            Name = name;
            RawMap = rawMap;
            GameMode = gameMode;
            Config = config;
        }
    }
}

