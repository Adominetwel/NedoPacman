using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.FileManager
{
    /// <summary>
    /// Класс описывает структуру файла сохранения
    /// </summary>
    public class LevelProgressData
    {
        public Dictionary<int, bool> PassedLevels { get; set; } = new();
    }
}
