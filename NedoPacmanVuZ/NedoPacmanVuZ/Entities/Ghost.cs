using NedoPacmanVuZ.Entities.GhostBehavior;

namespace NedoPacmanVuZ.Entities
{

    internal enum GhostMode
    {
        Chase,
        Scatter,
        Frightened 
    }
    internal class Ghost : Entity
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        private readonly IGhostBehavior _behavior;
        public bool IsInHouse { get; private set; }

        public Ghost(Vector2 position, string name, int speed, IGhostBehavior behavior, string typeId) : base(position, typeId)
        {
            Name = name;
            Speed = speed;
            _behavior = behavior;
            IsInHouse = (position.Y == 14);
        }
        public void Release(Vector2 exitPosition)
        {
            Position = exitPosition;
            IsInHouse = false;
        }
        public void Update(IGameContext context)
        {
            Vector2 direction = _behavior.CalculateNextMove(this, context);
            if (direction == Vector2.None) return;
            Vector2 nextPosition = Position + direction;
            if (context is GameCore core)
                Position = core.World.WrapPosition(nextPosition);
            else
                Position = nextPosition;
        }
    }
}
