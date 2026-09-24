using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.MainLogic
{
    internal interface IGameContext
    {
        ICollisionService CollisionService { get; }
        /// <summary>
        /// Возвращает любимое место призрака (куда он идёт во время блуждания)
        /// </summary>
        /// <param name="ghostTypeId">ID призрака</param>
        /// <returns>Координаты любимого места</returns>
        Vector2 GetScatterTarget(string ghostTypeId);
        Vector2 PlayerPosition { get; }
        Vector2 PlayerDirection { get; } 
        Vector2 BlinkyPosition { get; }
        GhostMode CurrentGhostMode { get; }
        GameMap World { get; }
        event Action<GhostMode>? OnGhostModeChanged;
    }
}
