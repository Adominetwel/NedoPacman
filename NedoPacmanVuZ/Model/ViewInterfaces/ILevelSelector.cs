using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NedoPacmanVuZ.View
{
    public interface ILevelSelector
    {
        /// <summary>
        /// Выбор уровня
        /// </summary>
        /// <returns>Уровень внутри таска</returns>
        Task<Level> SelectLevel();
    }
}
