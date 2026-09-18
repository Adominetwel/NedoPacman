using NedoPacmanVuZ.Model;

namespace NedoPacmanVuZ.Model.Entities.Collectibles
{
    /// <summary>
    /// Класс, описывает простейшую точку
    /// </summary>
    internal class DefaultDot : CollectibleItem
    {
        public DefaultDot(Vector2 position) : base(position, 1, "dot.default") { }
    }

}
