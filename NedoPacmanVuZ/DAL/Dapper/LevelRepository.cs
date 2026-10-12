using DAL.Repository;
using Dapper;
using Model.Model.GameLevel;
using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.Entities.Collectibles;
using NedoPacmanVuZ.Model.Entities.GhostBehavior;
using NedoPacmanVuZ.Model.Factories;
using NedoPacmanVuZ.Model.GameLevel;
using NedoPacmanVuZ.Model.GameLevel.NedoPacmanVuZ.Model.GameModes;
using NedoPacmanVuZ.Model.MainLogic;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.Dapper
{
    public class LevelRepository : ILevelRepository
    {
        private readonly string _connectionString;
        private readonly int _currentUserId;
        public LevelRepository(string connectionString, int currentUserId)
        {
            _connectionString = connectionString;
            _currentUserId = currentUserId;
        }
        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        public IEnumerable<Level> ReadAll()
        {
            using var db = CreateConnection();
            string sql = @"
                SELECT l.id as Id, l.name as Name, l.game_mode_id as GameModeId, COALESCE(p.is_passed, FALSE) as IsPassed
                FROM levels l
                LEFT JOIN user_progress p ON l.id = p.level_id AND p.user_id = @userId;";
            var dtos = db.Query<LevelDbDto>(sql, new { userId = _currentUserId });
            return dtos.Select(dto => {
                IGameMode mode = dto.GameModeId == "mode.annihilation" ? new AnnihilationGameMode() : new ClassicGameMode();
                var level = new Level(dto.Id, dto.Name, mode, dto.IsPassed);
                return level;
            }).ToList();
        }
        public LevelDbDto GetRawLevelData(int levelId)
        {
            using var db = CreateConnection();
            string sql = @"
                SELECT id as Id, raw_map as RawMap, cage_exit_x as CageExitX, cage_exit_y as CageExitY, 
                       cage_positions as CagePositions, ghost_settings as GhostSettings
                FROM levels WHERE id = @levelId;";
            return db.QueryFirstOrDefault<LevelDbDto>(sql, new { levelId });
        }
        public void Update(Level obj)
        {
            using var db = CreateConnection();
            string sql = @"
                INSERT INTO user_progress (user_id, level_id, is_passed) 
                VALUES (@userId, @levelId, @isPassed)
                ON CONFLICT (user_id, level_id) 
                DO UPDATE SET is_passed = EXCLUDED.is_passed;";
            db.Execute(sql, new { userId = _currentUserId, levelId = obj.Id, isPassed = obj.IsPassed });
        }
        public void ResetAllProgress()
        {
            using var db = CreateConnection();
            db.Execute("DELETE FROM user_progress WHERE user_id = @userId;", new { userId = _currentUserId });
        }
        public Level ReadById(int id) => ReadAll().FirstOrDefault(l => l.Id == id);
    }

}

