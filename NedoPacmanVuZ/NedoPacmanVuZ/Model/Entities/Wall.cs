using NedoPacmanVuZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Entities
{
    internal class Wall : Entity
    {
        public Wall(Vector2 position) : base(position, "wall") { }
    }
}
