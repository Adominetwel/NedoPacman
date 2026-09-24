using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.MainLogic
{
    internal class CollisionService : ICollisionService
    {
        public bool IsWallAt(Vector2 position, GameMap world)
        {
            Vector2 wrapped = world.WrapPosition(position);
            return world.Entities.Any(e => e is Wall && e.Position == wrapped);
        }
        public bool CanGhostMoveTo(Vector2 position, Ghost ghost, GameMap world)
        {
            if (IsWallAt(position, world))
                return false;
            Vector2 wrapped = world.WrapPosition(position);
            return !world.Ghosts.Any(g => g != ghost && g.State == GhostState.Active && !g.IsInHouse && g.Position == wrapped);
        }
    }
}
