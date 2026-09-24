using System;
using System.Collections.Generic;
using System.Linq;
using NedoPacmanVuZ.Model.Entities;
using NedoPacmanVuZ.Model.Entities.Collectibles;

namespace NedoPacmanVuZ.Model
{
    internal class GameMap
    {
        private readonly Random _random = new();
        private readonly List<Vector2> _initialDotPositions = new();
        private readonly List<Entity> _entities = new();
        private readonly List<Ghost> _ghosts = new();
        private readonly List<Projectile> _projectiles = new();
        public List<Vector2> CagePositions { get; } = new()
        {
            new Vector2(13, 12), new Vector2(14, 12), new Vector2(15, 12),
            new Vector2(13, 13),                      new Vector2(15, 13),
            new Vector2(13, 14), new Vector2(14, 14), new Vector2(15, 14)
        };
        public IReadOnlyList<Entity> Entities => _entities;
        public IReadOnlyList<Ghost> Ghosts => _ghosts;
        public IReadOnlyList<Projectile> Projectiles => _projectiles;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public event Action<int>? OnScorePointsEarned;
        public event Action? OnEnergizerTriggered;
        public event Action? OnAllDotsCollected;
        public GameMap(int width, int height, List<Entity> entities)
        {
            Width = width;
            Height = height;
            foreach (var entity in entities)
            {
                if (entity is CollectibleItem)
                    _initialDotPositions.Add(entity.Position);
                AddEntity(entity);
            }
        }
        /// <summary>
        /// Добавить entity в игровой мир
        /// </summary>
        /// <param name="entity">Ссылка на объект</param>
        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            if (entity is Ghost ghost)
                _ghosts.Add(ghost);
            else if (entity is Projectile projectile)
                _projectiles.Add(projectile);
            else if (entity is CollectibleItem item)
            {
                item.OnCollected += HandleItemCollected;
                if (item is EnergizerDot energizer)
                    energizer.OnEnergizerActivated += HandleEnergizerActivated;
            }
        }
        /// <summary>
        /// Убрать entity из игрового мира
        /// </summary>
        /// <param name="entity">Ссылка на объект</param>
        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);

            if (entity is Ghost ghost)
                _ghosts.Remove(ghost);
            else if (entity is Projectile projectile)
                _projectiles.Remove(projectile);
            else if (entity is CollectibleItem item)
            {
                item.OnCollected -= HandleItemCollected;
                if (item is EnergizerDot energizer)
                    energizer.OnEnergizerActivated -= HandleEnergizerActivated;
            }
        }
        private void HandleItemCollected(CollectibleItem item)
        {
            OnScorePointsEarned?.Invoke(item.ScoreValue);
            RemoveEntity(item);
        }
        /// <summary>
        /// Респавнит точки, если на поле осталось мало
        /// </summary>
        public void RespawnDotsIfNeeded()
        {
            int currentDots = CountRemainingDots();
            if (currentDots > _initialDotPositions.Count * 0.1) return;
            var emptyPositions = _initialDotPositions.Where(pos => !Entities.Any(e => e.Position == pos)).ToList();// норм ли логика
            foreach (var pos in emptyPositions) 
                if (_random.Next(0, 2) == 0)
                    if (_random.Next(0, 100) < 5)
                        AddEntity(new EnergizerDot(pos));
                    else
                        AddEntity(new DefaultDot(pos));

        }
        /// <summary>
        /// Убирает стены клетки
        /// </summary>
        public void RemoveCageWalls()
        {
            var wallsToRemove = Entities.Where(e => e.TypeId == "wall" && CagePositions.Contains(e.Position)).ToList();
            foreach (var wall in wallsToRemove)
                _entities.Remove(wall);
        }
        private void HandleEnergizerActivated() => OnEnergizerTriggered?.Invoke();
        /// <summary>
        /// Возвращает ссылку на объект игрока
        /// </summary>
        /// <returns>ссылка на объект игрока</returns>
        public Player? GetPlayer() => Entities.OfType<Player>().FirstOrDefault();
        /// <summary>
        /// Возвращает кол-во оставшихся на поле точек
        /// </summary>
        /// <returns>л-во оставшихся на поле точек</returns>
        public int CountRemainingDots() => Entities.Count(e => e is CollectibleItem);
        /// <summary>
        /// Метод, позволяющий объекту выходить за пределы карты и возвращаться с другого её конца
        /// </summary>
        /// <param name="pos">Позиция объекта</param>
        /// <returns>Конечная позиция</returns>
        public Vector2 WrapPosition(Vector2 pos)
        {
            int x = (pos.X % Width + Width) % Width;
            int y = (pos.Y % Height + Height) % Height;
            return new Vector2(x, y);
        }
        /// <summary>
        /// Проверяет, есть ли на конкретной позиции какой-либо объект
        /// </summary>
        /// <param name="pos">Позиция</param>
        /// <returns>Ссылка на объект если есть иначе null</returns>
        public Entity? GetEntityAt(Vector2 pos) // костыль???
        {
            Entity? ghost = null;
            Entity? projectile = null;
            Entity? collectibleOrWall = null;

            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Position == pos)
                {
                    if (_entities[i].TypeId == "player") return _entities[i];
                    if (_entities[i].TypeId.StartsWith("ghost")) ghost = _entities[i];
                    else if (_entities[i].TypeId == "projectile") projectile = _entities[i];
                    else if (collectibleOrWall == null) collectibleOrWall = _entities[i];
                }
            }
            return ghost ?? projectile ?? collectibleOrWall;
        }

    }


}
