using NedoPacmanVuZ.Model.GameModes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model
{
    internal class Level
    {
        public int[,] RawMap { get; }
        public IGameMode GameMode { get; }

        public Level(int[,] rawMap, IGameMode gameMode)
        {
            RawMap = rawMap;
            GameMode = gameMode;
        }
    }
}

