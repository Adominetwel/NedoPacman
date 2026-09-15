namespace NedoPacmanVuZ.Entities
{
    internal abstract class Entity
    {
        public string TypeId { get; init; }
        public Vector2 Position { get; set; }

        protected Entity(Vector2 position, string typeId)
        {
            Position = position;
            TypeId = typeId;
        }
    }
}
