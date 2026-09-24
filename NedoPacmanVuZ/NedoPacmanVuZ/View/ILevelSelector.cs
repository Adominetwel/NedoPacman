using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.View
{
    internal interface ILevelSelector
    {
        /// <summary>
        /// Метод запрашивает у пользователя выбор уровня и возвращает его.
        /// Для консоли это будет отрисовка стрелочками, для Unity — UI на Canvas.
        /// </summary>
        Level SelectLevel();
    }
}
