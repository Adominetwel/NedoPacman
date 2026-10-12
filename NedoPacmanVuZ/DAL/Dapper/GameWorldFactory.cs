using Dapper;
using Model.Model.Factories;
using Model.Model.Vectors;
using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.Entities.Collectibles;
using NedoPacmanVuZ.Model.Entities.GhostBehavior;
using NedoPacmanVuZ.Model.Factories;
using NedoPacmanVuZ.Model.MainLogic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace DAL.Dapper
{
    public class GameWorldFactory : IGameWorldFactory
    {
        private readonly string _connectionString;
        private readonly ObjectFactory<CollectibleItem> _itemFactory;
        private readonly ObjectFactory<Ghost> _ghostFactory;
        private readonly Assembly _modelAssembly;
        public GameWorldFactory(string connectionString, ObjectFactory<CollectibleItem> itemFactory, ObjectFactory<Ghost> ghostFactory)
        {
            _connectionString = connectionString;
            _itemFactory = itemFactory;
            _ghostFactory = ghostFactory;
            _modelAssembly = typeof(Wall).Assembly;
        }
        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        public GameMap CreateWorld(int levelId)
        {
            using var db = CreateConnection();
            var mappingRows = db.Query<(int TileCode, string EntityTypeId, string ClassFullName)>("SELECT tile_code as TileCode, entity_type_id as EntityTypeId, class_full_name as ClassFullName FROM tile_mappings;");
            var mapping = mappingRows.ToDictionary(x => x.TileCode, x => x.EntityTypeId);
            DynamicConfigureItemFactory(mappingRows);
            string levelSql = @"
                SELECT id as Id, raw_map as RawMap, cage_exit_x as CageExitX, cage_exit_y as CageExitY, 
                       cage_positions as CagePositions, ghost_settings as GhostSettings
                FROM levels WHERE id = @levelId;";
            var dto = db.QueryFirstOrDefault<LevelDbDto>(levelSql, new { levelId });
            if (dto == null) throw new Exception($"Уровень с ID {levelId} не найден в СУБД.");
            DynamicConfigureGhostFactory(dto.GhostSettings, db);
            var entities = new List<Entity>();
            string[] lines = dto.RawMap.Split(new[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries);
            int height = lines.Length;
            int width = lines[0].Split(',').Length;
            for (int y = 0; y < height; y++)
            {
                var tokens = lines[y].Split(',');
                for (int x = 0; x < width; x++)
                {
                    int code = int.Parse(tokens[x]);
                    if (!mapping.TryGetValue(code, out var typeId)) continue;
                    var pos = new Vector2(x, y);

                    if (typeId == "wall") entities.Add(new Wall(pos));
                    else if (typeId == "player") entities.Add(new Player(pos));
                    else if (typeId.StartsWith("dot.")) entities.Add(_itemFactory.Create(typeId, pos));
                    else if (typeId.StartsWith("ghost."))
                    {
                        Ghost ghost = _ghostFactory.Create(typeId, pos);
                        if (ghost != null) entities.Add(ghost);
                    }
                }
            }
            var cagePosList = new List<Vector2>();
            if (!string.IsNullOrEmpty(dto.CagePositions))
            {
                foreach (var p in dto.CagePositions.Split(';'))
                {
                    var coords = p.Split(',');
                    cagePosList.Add(new Vector2(int.Parse(coords[0]), int.Parse(coords[1])));
                }
            }

            return new GameMap(width, height, entities, cagePosList, new Vector2(dto.CageExitX, dto.CageExitY));
        }
        private void DynamicConfigureGhostFactory(string settingsRaw, IDbConnection db)
        {
            var behaviorMapping = db.Query<(string Id, string ClassName)>(
                "SELECT behavior_id as Id, class_full_name as ClassName FROM ghost_behaviors;")
                .ToDictionary(x => x.Id, x => x.ClassName);

            var ghostRecords = settingsRaw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var record in ghostRecords)
            {
                var tokens = record.Split(':');
                string typeId = tokens[0];
                int speed = int.Parse(tokens[1]);
                bool startInHouse = bool.Parse(tokens[2]);
                if (!behaviorMapping.TryGetValue(typeId, out var behaviorClassName)) continue;
                Type behaviorType = _modelAssembly.GetType(behaviorClassName);
                IGhostBehavior behavior = (IGhostBehavior)Activator.CreateInstance(behaviorType);
                string ghostName = typeId.Replace("ghost.", "");
                string name = char.ToUpper(ghostName[0]) + ghostName.Substring(1);
                _ghostFactory.Register(typeId, (pos) =>
                    new Ghost(pos, name, speed, behavior, typeId, startInHouse));
            }
        }
        private void DynamicConfigureItemFactory(IEnumerable<(int, string EntityTypeId, string ClassFullName)> mappingRows)
        {
            foreach (var row in mappingRows)
            {
                if (!row.EntityTypeId.StartsWith("dot.")) continue;
                Type itemType = _modelAssembly.GetType(row.ClassFullName);
                if (itemType == null) continue;
                _itemFactory.Register(row.EntityTypeId, (pos) => (CollectibleItem)Activator.CreateInstance(itemType, pos));
            }
        }
    }
}
