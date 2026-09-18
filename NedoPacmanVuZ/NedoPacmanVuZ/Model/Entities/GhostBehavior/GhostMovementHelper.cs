using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities;
namespace NedoPacmanVuZ.Model.Entities.GhostBehavior
{
    internal static class GhostMovementHelper
    {
        private static readonly Random _random = new();
        /// <summary>
        /// Основной алгоритм выбора лучшего направления движения к целевой точке
        /// </summary>
        /// <param name="ghost">Объект призрака, выполняющего перемещение</param>
        /// <param name="target">Координаты целевой точки, к которой стремится призрак</param>
        /// <param name="context">Контекст игры для проверки столкновений со стенами</param>
        /// <param name="table">Хранилище последних выбранных направлений для каждого призрака (нужно для запрета разворота на 180)</param>
        /// <returns></returns>
        public static Vector2 GetBestMove(Ghost ghost, Vector2 target, IGameContext context, ConditionalWeakTable<Ghost, BoxedVector> table)
        {
            var boxedDir = table.GetOrCreateValue(ghost);
            Vector2 currentDir = boxedDir.Value;
            var directions = new List<Vector2> { Vector2.Up, Vector2.Down, Vector2.Right, Vector2.Left };
            Vector2 bestDirection = Vector2.None;
            if (context.CurrentGhostMode == GhostMode.Frightened)
            {
                var validDirections = new List<Vector2>();
                foreach (var dir in directions)
                {
                    Vector2 nextPosition = ghost.Position + dir;
                    if (context.CheckCollision(nextPosition, ghost)) continue;
                    if (currentDir != Vector2.None && (dir.X == -currentDir.X && dir.Y == -currentDir.Y)) continue;

                    validDirections.Add(dir);
                }
                if (validDirections.Count > 0)
                    bestDirection = validDirections[_random.Next(validDirections.Count)];
            }
            else
            {
                double minDistance = double.MaxValue;
                foreach (var dir in directions)
                {
                    Vector2 nextPosition = ghost.Position + dir;
                    if (context.CheckCollision(nextPosition, ghost)) continue;
                    if (currentDir != Vector2.None && (dir.X == -currentDir.X && dir.Y == -currentDir.Y)) continue;
                    int dx = nextPosition.X - target.X;
                    int dy = nextPosition.Y - target.Y;
                    double distance = (dx * dx) + (dy * dy);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        bestDirection = dir;
                    }
                }
            }
            if (bestDirection == Vector2.None)
            {
                Vector2 reverseDir = new Vector2(-currentDir.X, -currentDir.Y);
                if (currentDir != Vector2.None && !context.CheckCollision(ghost.Position + reverseDir, ghost))
                    bestDirection = reverseDir;
            }
            boxedDir.Value = bestDirection;
            return bestDirection;
        }
    }

}
