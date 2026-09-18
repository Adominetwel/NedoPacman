using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model
{
    internal interface IGameContext
    {
        /// <summary>
        /// Проверяет столкновение объектов
        /// </summary>
        /// <param name="targetPosition">Куда движется объект</param>
        /// <param name="checkingGhost">Если призрак, проеряет столкновение с другими призраками</param>
        /// <returns>true если столкнулся, иначе false</returns>
        bool CheckCollision(Vector2 targetPosition, Ghost checkingGhost = null);
        Vector2 PlayerPosition { get; }
        Vector2 PlayerDirection { get; } 
        Vector2 BlinkyPosition { get; }
        GhostMode CurrentGhostMode { get; }
        GameMap World { get; }
        /// <summary>
        /// Возвращает любимое место призрака (куда он идёт во время блуждания)
        /// </summary>
        /// <param name="ghostTypeId">ID призрака</param>
        /// <returns>Координаты любимого места</returns>
        Vector2 GetScatterTarget(string ghostTypeId);
        event Action<GhostMode>? OnGhostModeChanged;
    }
}
