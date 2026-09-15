using NedoPacmanVuZ.Entities;

namespace NedoPacmanVuZ.FactoryPattern
{
    internal interface IObjectFactory<out T> where T : Entity
    {
        /// <summary>
        /// Создает экземпляр сущности по его строковому идентефикатору на указанной позиции, если данный тип зарегистрирован в конкретной фабрике
        /// </summary>
        /// <param name="typeId">Строковый идентификатор создаваемого предмета</param>
        /// <param name="position">Координаты, на которых должен быть создан предмет</param>
        /// <returns>Экземпляр созданного предмета или null, если переданный тип отсутствует в реестре конкретной фабрики</returns>
        T? Create(string typeId, Vector2 position);
    }
}
