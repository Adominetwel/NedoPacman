using Model.DALInterfaces;
using Model.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameLevel
{
    public class Level : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; }
        public IGameMode GameMode { get; }
        public bool IsPassed { get; set; } = false;
        public Level() { }
        public Level(int id, string name, IGameMode gameMode, bool isPassed)
        {
            Id = id;
            Name = name;
            GameMode = gameMode;
            IsPassed = isPassed;
        }
    }
}

