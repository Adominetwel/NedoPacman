using DAL;
using Dapper;
using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.Entities.Collectibles;
using NedoPacmanVuZ.Model.Entities.GhostBehavior;
using NedoPacmanVuZ.Model.Factories;
using NedoPacmanVuZ.Model.MainLogic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Model.Model.Factories
{
    public class GameWorldFactory : IGameWorldFactory
    {
        private readonly string _connectionString;
        private readonly ObjectFactory<CollectibleItem> _itemFactory;
        private readonly ObjectFactory<Ghost> _ghostFactory;

        public GameWorldFactory(
            string connectionString,
            ObjectFactory<CollectibleItem> itemFactory,
            ObjectFactory<Ghost> ghostFactory)
        {
            _connectionString = connectionString;
            _itemFactory = itemFactory;
            _ghostFactory = ghostFactory;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public GameMap CreateWorld(int levelId)
        {
            using var db = CreateConnection();
            var mappingRows = db.Query<(int TileCode, string EntityTypeId)>(
                "SELECT tile_code, entity_type_id FROM tile_mappings;");
            var mapping = mappingRows.ToDictionary(x => x.TileCode, x => x.EntityTypeId);
            string levelSql = @"
                SELECT id as Id, raw_map as RawMap, cage_exit_x as CageExitX, cage_exit_y as CageExitY, 
                       cage_positions as CagePositions, ghost_settings as GhostSettings
                FROM levels WHERE id = @levelId;";
            var dto = db.QueryFirstOrDefault<LevelDbDto>(levelSql, new { levelId });
            if (dto == null) throw new Exception($"Уровень с ID {levelId} не найден в СУБД.");
            ConfigureGhostFactory(dto.GhostSettings);
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

        private void ConfigureGhostFactory(string settingsRaw)
        {
            var ghostRecords = settingsRaw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var record in ghostRecords)
            {
                var tokens = record.Split(':');
                string typeId = tokens[0];
                int speed = int.Parse(tokens[1]);
                bool startInHouse = bool.Parse(tokens[2]);

                IGhostBehavior behavior = typeId switch
                {
                    "ghost.blinky" => new BlinkyBehavior(),
                    "ghost.pinky" => new PinkyBehavior(),
                    "ghost.inky" => new InkyBehavior(),
                    "ghost.clyde" => new ClydeBehavior(),
                    _ => new BlinkyBehavior()
                };

                string name = char.ToUpper(typeId[6]) + typeId.Substring(7);
                _ghostFactory.Register(typeId, (pos) => new Ghost(pos, name, speed, behavior, typeId, startInHouse));
            }
        }
    }
}
