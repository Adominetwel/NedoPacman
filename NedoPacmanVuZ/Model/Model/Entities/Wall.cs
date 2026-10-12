using Model.Model.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Entities
{
    public class Wall : Entity
    {
        public Wall(Vector2 position) : base(position, "wall") { }
    }
}
