using System;
using System.Runtime.CompilerServices;
using NedoPacmanVuZ.Entities;

namespace NedoPacmanVuZ.Entities.GhostBehavior
{
    internal class BlinkyBehavior : IGhostBehavior
    {
        private static readonly ConditionalWeakTable<Ghost, BoxedVector> _lastDirections = new();
        public Vector2 CalculateNextMove(Ghost ghost, IGameContext context)
        {
            Vector2 target = context.CurrentGhostMode == GhostMode.Scatter
                ? context.GetScatterTarget(ghost.TypeId)
                : context.PlayerPosition;
            return GhostMovementHelper.GetBestMove(ghost, target, context, _lastDirections);
        }
    }
}
