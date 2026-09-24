using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities.GhostBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Entities
{

    internal enum GhostMode
    {
        Chase,
        Scatter,
        Frightened 
    }
    enum GhostState
    {
        Active,
        InCage,
        Dead
    }
    internal class Ghost : Entity
    {
        public GhostState State { get; set; } = GhostState.Active;
        public int HitCount { get; set; } = 0;
        public string Name { get; set; }
        public int Speed { get; }
        public bool IsInHouse { get; set; } = true;
        public IGhostBehavior Behavior { get; }
        public int CageTicksLeft { get; set; } = 0;
        public Ghost(Vector2 position, string name, int speed, IGhostBehavior behavior, string typeId, bool startInHouse) : base(position, typeId)
        {
            Name = name;
            Speed = speed;
            Behavior = behavior;
            IsInHouse = startInHouse;
        }
        /// <summary>
        /// Высвобождает призрака из клетки в первый раз
        /// </summary>
        /// <param name="exitPosition">Позиция выхода из клетки</param>
        public void Release(Vector2 exitPosition)
        {
            Position = exitPosition;
            IsInHouse = false;
        }
        /// <summary>
        /// Обновление состояния объекта
        /// </summary>
        /// <param name="context"></param>
        public void Update(IGameContext context)
        {
            if (State != GhostState.Active) return;
            Vector2 direction = Behavior.CalculateNextMove(this, context);
            if (direction != Vector2.None)
                Position = context.World.WrapPosition(Position + direction);
        }
    }
}
