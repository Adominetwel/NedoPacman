using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Entities.GhostBehavior
{
    internal interface IGhostBehavior
    {
        /// <summary>
        /// Вычисляет следующий шаг, преследуя игрока по его координатам
        /// </summary>
        /// <param name="ghost">Экземпляр призрака, для которого рассчитывается ход</param>
        /// <param name="context">Контекст игры для получения позиции игрока</param>
        /// <returns>Вектор направления движения</returns>
        Vector2 CalculateNextMove(Ghost ghost, IGameContext context);
    }
}
