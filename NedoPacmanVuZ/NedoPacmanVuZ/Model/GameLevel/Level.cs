using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameLevel
{
    internal class Level
    {
        public int Id { get; }
        public string Name { get; }
        public int[,] RawMap { get; }
        public IGameMode GameMode { get; }
        public bool IsPassed { get; set; } = false;
        public Level(int id, string name, int[,] rawMap, IGameMode gameMode)
        {
            Id = id;
            Name = name;
            RawMap = rawMap;
            GameMode = gameMode;
        }
    }
}

