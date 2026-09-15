using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Entities
{
    internal abstract class Entity
    {
        public string TypeId { get; init; }
        public event Action<Vector2, Vector2>? OnPositionChanged;
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    Vector2 oldPos = _position;
                    _position = value;
                    OnPositionChanged?.Invoke(oldPos, _position);
                }
            }
        }

        protected Entity(Vector2 position, string typeId)
        {
            Position = position;
            TypeId = typeId;
        }
    }
}
