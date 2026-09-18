using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NedoPacmanVuZ.Model.Factories
{
    internal class ObjectFactory<T> : IObjectFactory<T> where T : Entity
    {
        private readonly Dictionary<string, Func<Vector2, T>> _registry = new();
        public event Action<string>? OnTypeRegistered;
        public void Register(string typeId, Func<Vector2, T> createFunc)
        {
            _registry[typeId] = createFunc;
            OnTypeRegistered?.Invoke(typeId);
        }
        public T? Create(string typeId, Vector2 position) => _registry.TryGetValue(typeId, out var func) ? func(position) : null;
    }

}
