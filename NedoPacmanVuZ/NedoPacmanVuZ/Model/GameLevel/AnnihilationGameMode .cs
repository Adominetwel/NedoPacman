using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameLevel
{
    namespace NedoPacmanVuZ.Model.GameModes
    {
        internal class AnnihilationGameMode : IGameMode
        {
            public string ModeId => "mode.annihilation";
            public bool IsShootingAllowed => true;
            public bool IsDotRespawnEnabled => true;
            public bool AreGhostsPermanentlyKillable => true;
            public bool IsVictoryAchieved(GameMap world)
            {
                return !world.Ghosts.Any(g => g.State != GhostState.Dead);
            }
        }
    }
}
