using NedoPacmanVuZ.Model;
using System;

namespace NedoPacmanVuZ.Model.Entities.Collectibles
{
    /// <summary>
    /// Класс, описывает точку, при сборе которой, противники начинают бояться + добавляется возможность съесть призрака
    /// </summary>
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
