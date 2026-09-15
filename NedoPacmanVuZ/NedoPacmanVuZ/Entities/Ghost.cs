using NedoPacmanVuZ.Entities.GhostBehavior;

namespace NedoPacmanVuZ.Entities
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
        private readonly IGhostBehavior _behavior;
        public bool IsInHouse { get; set; } = true;

        public Ghost(Vector2 position, string name, int speed, IGhostBehavior behavior, string typeId) : base(position, typeId)
        {
            Name = name;
            Speed = speed;
            _behavior = behavior;
            if (base.TypeId == "ghost.blinky") // костыль
                IsInHouse = false;
        }
        public void Release(Vector2 exitPosition)
        {
            Position = exitPosition;
            IsInHouse = false;
        }
        public void Update(IGameContext context)
        {
            if (State != GhostState.Active) return;
            Vector2 direction = _behavior.CalculateNextMove(this, context);
            if (direction != Vector2.None)
                Position = context.World.WrapPosition(Position + direction);
        }
    }
}
