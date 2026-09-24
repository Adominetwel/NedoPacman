using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.View
{
    internal interface ILevelSelector
    {
        Task<Level> SelectLevel();
    }
}
