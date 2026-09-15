using System;
using System.Runtime.CompilerServices;
using NedoPacmanVuZ.Entities;

namespace NedoPacmanVuZ.Entities.GhostBehavior
{
    internal class InkyBehavior : IGhostBehavior
    {
        private static readonly ConditionalWeakTable<Ghost, BoxedVector> _lastDirections = new();
        public Vector2 CalculateNextMove(Ghost ghost, IGameContext context)
        {
            Vector2 playerPos = context.CurrentGhostMode == GhostMode.Scatter
                ? context.GetScatterTarget(ghost.TypeId)
                : context.PlayerPosition;
            Vector2 playerDir = context.PlayerDirection;
            Vector2 blinkyPos = context.BlinkyPosition;
            // 1. Промежуточная точка: на 2 шага впереди Пакмана
            Vector2 offsetPoint = new Vector2(
                playerPos.X + (playerDir.X * 2),
                playerPos.Y + (playerDir.Y * 2)
            );
            // 2. Вектор от Блинки до этой промежуточной точки
            int vecX = offsetPoint.X - blinkyPos.X;
            int vecY = offsetPoint.Y - blinkyPos.Y;
            // 3. Удваиваем этот вектор относительно Блинки
            Vector2 target = new Vector2(
                blinkyPos.X + (vecX * 2),
                blinkyPos.Y + (vecY * 2)
            );
            return GhostMovementHelper.GetBestMove(ghost, target, context, _lastDirections);
        }
    }
}
