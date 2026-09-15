using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.View
{
    internal interface IGameView
    {
        void Render(GameCore model);
        void ShowGameOver(bool isWin);
    }
}
