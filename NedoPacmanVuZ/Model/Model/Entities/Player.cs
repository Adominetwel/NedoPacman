using Model.Model.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Entities
{
    public class Player : Entity
    {
        public Player(Vector2 position) : base(position, "player") { }
    }
}
