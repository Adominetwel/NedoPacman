using NedoPacmanVuZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.View
{
    internal interface IInputProvider
    {
        /// <summary>
        /// Создаёт вектор направления движения
        /// </summary>
        /// <param name="shootPressed">Был ли шаг игрока выстрелом</param>
        /// <returns>вектор направления движения</returns>
        Vector2 GetNextDirection(out bool shootPressed);
    }
}
