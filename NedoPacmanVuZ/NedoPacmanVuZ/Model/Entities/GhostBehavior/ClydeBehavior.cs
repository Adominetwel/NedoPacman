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
    /// Пытается играть на опережение и целится на 4 клетки вперед по направлению движения Пакмана
    /// </summary>
    internal class ClydeBehavior : IGhostBehavior
    {
        private static readonly ConditionalWeakTable<Ghost, BoxedVector> _lastDirections = new();
        public Vector2 CalculateNextMove(Ghost ghost, IGameContext context)
        {
            int dx = ghost.Position.X - context.PlayerPosition.X;
            int dy = ghost.Position.Y - context.PlayerPosition.Y;
            double distanceToPlayer = Math.Sqrt(dx * dx + dy * dy);

            Vector2 target;
            if (distanceToPlayer > 8)
            {
                target = context.CurrentGhostMode == GhostMode.Scatter
                    ? context.GetScatterTarget(ghost.TypeId)
                    : context.PlayerPosition;
            }
            else
            {
                target = context.GetScatterTarget(ghost.TypeId);
            }

            return GhostMovementHelper.GetBestMove(ghost, target, context, _lastDirections);
        }
    }

}
