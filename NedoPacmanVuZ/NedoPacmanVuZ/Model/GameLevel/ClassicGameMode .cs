using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameLevel
{
    internal class ClassicGameMode : IGameMode
    {
        public string ModeId => "mode.classic";
        public bool IsShootingAllowed => false;
        public bool IsDotRespawnEnabled => false;
        public bool AreGhostsPermanentlyKillable => false;
        public bool IsTimedRespawnEnabled => true;
        public bool IsVictoryAchieved(GameMap world)
        {
            return world.CountRemainingDots() == 0;
        }
    }
}
