using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.DTO
{
    /// <summary>
    /// Класс описывает структуру файла сохранения
    /// </summary>
    internal class LevelProgressData
    {
        public Dictionary<int, bool> PassedLevels { get; set; } = new();
    }
}
