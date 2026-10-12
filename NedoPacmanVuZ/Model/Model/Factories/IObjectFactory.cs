using Model.Model.Vectors;
using NedoPacmanVuZ.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.Model.Factories
{
    public interface IObjectFactory<T> where T : Entity
    {
        /// <summary>
        /// Создает экземпляр сущности по его строковому идентефикатору на указанной позиции, если данный тип зарегистрирован в конкретной фабрике
        /// </summary>
        /// <param name="typeId">Строковый идентификатор создаваемого предмета</param>
        /// <param name="position">Координаты, на которых должен быть создан предмет</param>
        /// <returns>Экземпляр созданного предмета или null, если переданный тип отсутствует в реестре конкретной фабрики</returns>
        T? Create(string typeId, Vector2 position);
        /// <summary>
        /// Регистрирует объект на фабрике
        /// </summary>
        /// <param name="typeId">ID объекта</param>
        /// <param name="createFunc">Делегат создания объекта</param>
        void Register(string typeId, Func<Vector2, T> createFunc);
    }
}
