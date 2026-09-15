//using NedoPacmanVuZ.Entities;
//using NedoPacmanVuZ.Entities.GhostBehavior;

//namespace NedoPacmanVuZ.FactoryPattern
//{
//    internal class GhostFactory : IObjectFactory<Ghost>
//    {
//        private readonly Dictionary<string, Func<Vector2, Ghost>> _registry = new();
//        public event Action<string>? OnTypeRegistered;
//        public GhostFactory()
//        {
//            Register("ghost.blinky", (pos) => new Ghost(pos, "Blinky", 1, new BlinkyBehavior(), "ghost.blinky"));
//            Register("ghost.pinky", (pos) => new Ghost(pos, "Pinky", 1, new PinkyBehavior(), "ghost.pinky"));
//            Register("ghost.inky", (pos) => new Ghost(pos, "Inky", 1, new InkyBehavior(), "ghost.inky"));
//            Register("ghost.clyde", (pos) => new Ghost(pos, "Clyde", 1, new ClydeBehavior(), "ghost.clyde"));
//        }
//        /// <summary>
//        /// Регистрирует в фабрике функцию создания призрака под определенным строковым идентификатором типа и оповещает об этом через событие
//        /// </summary>
//        /// <param name="typeId">Строковый идентификатор типа регистрируемого призрака</param>
//        /// <param name="createFunc">Функция, принимающая координаты и конструирующая объект призрака</param>
//        public void Register(string typeId, Func<Vector2, Ghost> createFunc)
//        {
//            _registry[typeId] = createFunc;
//            OnTypeRegistered?.Invoke(typeId);
//        }
//        public Ghost? Create(string typeId, Vector2 position)
//        {
//            return _registry.TryGetValue(typeId, out var func) ? func(position) : null;
//        }
//    }
//}
