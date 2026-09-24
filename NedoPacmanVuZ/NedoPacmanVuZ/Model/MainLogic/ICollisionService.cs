using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.MainLogic
{
    internal interface ICollisionService
    {
        bool IsWallAt(Vector2 position, GameMap world);
        bool CanGhostMoveTo(Vector2 position, Ghost ghost, GameMap world);
    }
}
