using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.MainLogic
{
    internal interface ICollisionService
    {
        /// <summary>
        /// Проверяет на наличие стены по позиции
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="world">Игровая карта</param>
        /// <returns>true если есть стена, иначе false</returns>
        bool IsWallAt(Vector2 position, GameMap world);
        /// <summary>
        /// Проверяет, может ли призрак встать на конкретную позицию
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="ghost">Ссылка на призрака</param>
        /// <param name="world">Игровая карта</param>
        /// <returns>true если есть может, иначе false</returns>
        bool CanGhostMoveTo(Vector2 position, Ghost ghost, GameMap world);
    }
}
