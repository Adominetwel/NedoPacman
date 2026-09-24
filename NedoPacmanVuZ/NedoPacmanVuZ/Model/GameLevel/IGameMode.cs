using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameModes
{
    internal interface IGameMode
    {
        string ModeId { get; }
        bool IsShootingAllowed { get; }
        bool IsDotRespawnEnabled { get; }
        bool AreGhostsPermanentlyKillable { get; }
        /// <summary>
        /// Проверяет, выполнены ли условия победы в данном режиме.
        /// </summary>
        bool IsVictoryAchieved(GameMap world);
    } 
}