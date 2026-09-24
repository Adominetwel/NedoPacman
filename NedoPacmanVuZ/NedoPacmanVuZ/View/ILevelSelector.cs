using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.View
{
    internal interface ILevelSelector
    {
        /// <summary>
        /// Выбор уровня
        /// </summary>
        /// <returns>Уровень внутри таска</returns>
        Task<Level> SelectLevel();
    }
}
