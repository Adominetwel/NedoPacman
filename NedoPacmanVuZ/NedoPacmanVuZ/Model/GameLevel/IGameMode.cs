using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.GameLevel
{
    internal interface IGameMode
    {
        string ModeId { get; }
        bool IsShootingAllowed { get; }
        bool IsDotRespawnEnabled { get; }
        bool IsTimedRespawnEnabled { get; }
        bool AreGhostsPermanentlyKillable { get; }
        /// <summary>
        /// Проверяет, выполнены ли условия победы в данном режиме
        /// </summary>
        /// <param name="world">Ссылка на игровой мир</param>
        /// <returns>true если победа, иначе false</returns>
        bool IsVictoryAchieved(GameMap world);
    } 
}