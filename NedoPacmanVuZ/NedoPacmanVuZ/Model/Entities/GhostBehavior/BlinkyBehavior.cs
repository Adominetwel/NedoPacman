using System;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities;

namespace NedoPacmanVuZ.Model.Entities.GhostBehavior
{
    /// <summary>
    /// Нацелен на ту клетку лабиринта, на которой Пакман находится в текущий момент
    /// </summary>
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
