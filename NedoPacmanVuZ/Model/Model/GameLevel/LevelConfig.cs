using NedoPacmanVuZ.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.GameLevel
{
    public class LevelConfig
    {
        public Vector2 CageExitPosition { get; }
        public List<Vector2> CagePositions { get; }

        public LevelConfig(Vector2 cageExitPosition, List<Vector2> cagePositions)
        {
            CageExitPosition = cageExitPosition;
            CagePositions = cagePositions ?? new List<Vector2>();
        }
    }

}
