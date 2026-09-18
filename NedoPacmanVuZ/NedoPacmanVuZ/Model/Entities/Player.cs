using NedoPacmanVuZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Entities
{
    internal class Player : Entity
    {
        public Player(Vector2 position) : base(position, "player") { }
    }
}
