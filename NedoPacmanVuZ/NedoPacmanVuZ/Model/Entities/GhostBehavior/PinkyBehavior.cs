using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities;

namespace NedoPacmanVuZ.Model.Entities.GhostBehavior
{
    /// <summary>
    /// Проверяет расстояние до игрока: если до Пакмана больше 8 клеток, он копирует логику Блинки и идет прямо на него. 
    /// Как только дистанция сокращается до 8 клеток и меньше, Клайд пугается и меняет цель на свой персональный нижний левый угол лабиринта
    /// </summary>
    internal class PinkyBehavior : IGhostBehavior
    {
        private static readonly ConditionalWeakTable<Ghost, BoxedVector> _lastDirections = new();
        public Vector2 CalculateNextMove(Ghost ghost, IGameContext context)
        {
            Vector2 playerDir = context.PlayerDirection;
            // Цель Пинки - позиция игрока + 4 клетки вперёд по его ходу
            Vector2 target = context.CurrentGhostMode == GhostMode.Scatter ? context.GetScatterTarget(ghost.TypeId) :
                new Vector2(
                context.PlayerPosition.X + (playerDir.X * 4),
                context.PlayerPosition.Y + (playerDir.Y * 4)
            );
            return GhostMovementHelper.GetBestMove(ghost, target, context, _lastDirections);
        }
    }
}
