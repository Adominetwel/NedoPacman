using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace NedoPacmanVuZ.Model.DTO
{
    internal class JsonProgressService : IProgressStorage
    {
        private readonly string _filePath = "user_progress.json";

        /// <summary>
        /// Загружает прогресс из JSON-файла. Если файла нет - создает пустой прогресс
        /// </summary>
        public Dictionary<int, bool> LoadProgress()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<int, bool>();
            try
            {
                var data = JsonSerializer.Deserialize<LevelProgressData>(File.ReadAllText(_filePath));
                return data?.PassedLevels ?? new Dictionary<int, bool>();
            }
            catch
            {
                return new Dictionary<int, bool>();
            }
        }
        /// <summary>
        /// Сохраняет переданный прогресс в JSON-файл.
        /// </summary>
        public void SaveProgress(Dictionary<int, bool> passedLevels)
        {
            var data = new LevelProgressData { PassedLevels = passedLevels };
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, json);
        }
        /// <summary>
        /// Стирает файл сохранения
        /// </summary>
        public void ResetProgress()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}
