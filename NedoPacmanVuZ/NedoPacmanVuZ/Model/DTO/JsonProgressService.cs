using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
namespace NedoPacmanVuZ.Model.DTO
{
    internal class JsonProgressService : IProgressStorage
    {
        private readonly string _filePath = "user_progress.json";
        public Dictionary<int, bool> LoadProgress()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<int, bool>();
            try
            {
                var json = File.ReadAllText(_filePath);
                var data = JsonConvert.DeserializeObject<LevelProgressData>(json);
                return data?.PassedLevels ?? new Dictionary<int, bool>();
            }
            catch
            {
                return new Dictionary<int, bool>();
            }
        }
        public void SaveProgress(Dictionary<int, bool> passedLevels)
        {
            var data = new LevelProgressData { PassedLevels = passedLevels };
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(_filePath, json);
        }
        public void ResetProgress()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}
