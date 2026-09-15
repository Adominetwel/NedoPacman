//using NedoPacmanVuZ.Entities.Collectibles;

//namespace NedoPacmanVuZ.FactoryPattern
//{
//    internal class CollectibleFactory : IObjectFactory<CollectibleItem> // какая-то хрень с регистр
//    {
//        private readonly Dictionary<string, Func<Vector2, CollectibleItem>> _registry = new();
//        public event Action<string>? OnTypeRegistered;
//        public CollectibleFactory()
//        {
//            Register("dot.default", (pos) => new DefaultDot(pos));
//            Register("dot.energizer", (pos) => new DefaultDot(pos)); // тут новый объект

//        }
//        /// <summary>
//        /// Регистрирует новый тип собираемого предмета и связывающую его функцию создания в реестре фабрики, а также вызывает событие уведомления
//        /// </summary>
//        /// <param name="typeId">Строковый идентификатор типа регистрируемого предмета</param>
//        /// <param name="createFunc">Делегат принимающий позицию и возвращающий готовый объект предмета</param>
//        public void Register(string typeId, Func<Vector2, CollectibleItem> createFunc)
//        {
//            _registry[typeId] = createFunc;
//            OnTypeRegistered?.Invoke(typeId);
//        }
//        public CollectibleItem? Create(string typeId, Vector2 position)
//        {
//            return _registry.TryGetValue(typeId, out var func) ? func(position) : null;
//        }
//    }
//}
