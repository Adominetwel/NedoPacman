using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.View
{
    internal interface IGameView
    {
        /// <summary>
        /// Отрисовывает игровую карту
        /// </summary>
        /// <param name="model">Ссылка на модель</param>
        void Render(GameCore model);
        /// <summary>
        /// Отрисовывает экран окончания игры
        /// </summary>
        /// <param name="isWin"></param>
        void ShowGameOver(bool isWin);
    }
}
