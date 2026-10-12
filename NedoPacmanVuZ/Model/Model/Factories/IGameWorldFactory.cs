using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.Factories
{
    public interface IGameWorldFactory
    {
        GameMap CreateWorld(int levelId);
    }
}
