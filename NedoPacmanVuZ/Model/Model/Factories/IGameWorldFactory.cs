using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.Factories
{
    public interface IGameWorldFactory
    {
        /// <summary>
        /// Создаёт объект игрового мира
        /// </summary>
        /// <param name="levelId">ID уровня, который создаётся</param>
        /// <returns>Ссылка на объект игрового мира</returns>
        GameMap CreateWorld(int levelId);
    }
}
