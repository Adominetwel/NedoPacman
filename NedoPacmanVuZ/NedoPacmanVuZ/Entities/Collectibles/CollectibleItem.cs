namespace NedoPacmanVuZ.Entities.Collectibles
{
    /// <summary>
    /// Абстрактный класс, описывает любой собираемый игроком предмет
    /// </summary>
    internal abstract class CollectibleItem : Entity
    {
        public int ScoreValue { get; protected set; }
        public event Action<CollectibleItem>? OnCollected;
        protected CollectibleItem(Vector2 position, int scoreValue, string typeId) : base(position, typeId)
        {
            ScoreValue = scoreValue;
        }
        public virtual void OnCollect()
        {
            OnCollected?.Invoke(this);
        }
    }
}
