using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.Entities.Collectibles
{
    internal class EnergizerDot : CollectibleItem
    {
        public event Action? OnEnergizerActivated;
        public EnergizerDot(Vector2 position) : base(position, 10, "dot.energizer") { }

        public override void OnCollect()
        {
            OnEnergizerActivated?.Invoke();
            base.OnCollect();
        }
    }
}
