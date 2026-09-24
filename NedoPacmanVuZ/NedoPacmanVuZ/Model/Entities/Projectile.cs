using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NedoPacmanVuZ.Model.Entities
{
    internal class Projectile : Entity
    {
        public Vector2 Direction { get; }
        public event Action<Projectile>? OnMoved;
        public Projectile(Vector2 position, Vector2 direction) : base(position, "projectile")
        {
            Direction = direction;
        }
        /// <summary>
        /// Двигает объект
        /// </summary>
        /// <param name="world">Ссылка на игровой мир</param>
        public void Move(GameMap world)
        {
            Position = world.WrapPosition(Position + Direction);
            OnMoved?.Invoke(this);
        }
    }
}
