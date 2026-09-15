using System;
using System.Collections.Generic;
using System.Linq;
using NedoPacmanVuZ.Entities;
using NedoPacmanVuZ.Entities.Collectibles;
using NedoPacmanVuZ.FactoryPattern;

namespace NedoPacmanVuZ
{
    internal class GameMap
    {
        public List<Entity> Entities { get; } = new();
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
                AddEntity(entity);
            }
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            if (entity is CollectibleItem item)
            {
                item.OnCollected += HandleItemCollected;

                if (item is EnergizerDot energizer)
                {
                    energizer.OnEnergizerActivated += HandleEnergizerActivated;
                }
            }
        }
        public void RemoveEntity(Entity entity)
        {
            if (entity is CollectibleItem item)
            {
                item.OnCollected -= HandleItemCollected;

                if (item is EnergizerDot energizer)
                {
                    energizer.OnEnergizerActivated -= HandleEnergizerActivated;
                }
            }
            Entities.Remove(entity);
        }

        private void HandleItemCollected(CollectibleItem item)
        {
            OnScorePointsEarned?.Invoke(item.ScoreValue);
            RemoveEntity(item);

            if (CountRemainingDots() == 0)
            {
                OnAllDotsCollected?.Invoke();
            }
        }

        private void HandleEnergizerActivated()
        {
            OnEnergizerTriggered?.Invoke();
        }

        public Player? GetPlayer() => Entities.OfType<Player>().FirstOrDefault();
        public int CountRemainingDots() => Entities.Count(e => e is CollectibleItem);

        public bool IsWallAt(Vector2 pos)
        {
            Vector2 wrappedPos = WrapPosition(pos);
            return Entities.Any(e => e.TypeId == "wall" && e.Position == wrappedPos);
        }

        public Vector2 WrapPosition(Vector2 pos)
        {
            int x = (pos.X % Width + Width) % Width;
            int y = (pos.Y % Height + Height) % Height;
            return new Vector2(x, y);
        }

        public Entity? GetEntityAt(Vector2 pos)
        {
            var cellEntities = Entities.Where(e => e.Position == pos).ToList();
            if (cellEntities.Any(e => e.TypeId == "player")) return cellEntities.First(e => e.TypeId == "player");
            return cellEntities.FirstOrDefault(e => e.TypeId.StartsWith("ghost")) ?? cellEntities.FirstOrDefault();
        }
    }


}
