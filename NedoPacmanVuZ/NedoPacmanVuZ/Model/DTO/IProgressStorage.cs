using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Model.DTO
{
    internal interface IProgressStorage
    {
        /// <summary>
        /// Загружает карту прогресса (ID уровня -> пройден или нет)
        /// </summary>
        Dictionary<int, bool> LoadProgress();
        /// <summary>
        /// Сохраняет всю карту прогресса
        /// </summary>
        /// <param name="passedLevels">Словарь вида ID уровня -> пройден или нет</param>
        void SaveProgress(Dictionary<int, bool> passedLevels);
        /// <summary>
        /// Полностью очищает прогресс в данном хранилище
        /// </summary>
        void ResetProgress();
    }
}
