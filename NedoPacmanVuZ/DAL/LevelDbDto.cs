using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class LevelDbDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GameModeId { get; set; }
        public string RawMap { get; set; }
        public int CageExitX { get; set; }
        public int CageExitY { get; set; }
        public string CagePositions { get; set; }
        public string GhostSettings { get; set; }
        public bool IsPassed { get; set; }
    }
}
