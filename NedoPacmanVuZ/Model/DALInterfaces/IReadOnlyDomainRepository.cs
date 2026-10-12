using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DALInterfaces
{
    public interface IReadOnlyDomainRepository<T> where T : IDomainObject, new()
    {
        /// <summary>
        /// Возвращает всю коллекцию доменных объектов из репозитория
        /// </summary>
        /// <returns>Перечисляемая коллекция, содержащая все объекты из источника данных</returns>
        IEnumerable<T> ReadAll();
        /// <summary>
        /// Находит и возвращает конкретный доменный объект по его уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный числовой идентификатор искомого объекта.</param>
        /// <returns>Найденный доменный объект</returns>
        T ReadById(int id);
    }
}
