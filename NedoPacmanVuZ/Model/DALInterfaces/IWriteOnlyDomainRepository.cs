using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DALInterfaces
{
    public interface IWriteOnlyDomainRepository<T> where T : IDomainObject, new()
    {
        /// <summary>
        /// Добавляет новый доменный объект в репозиторий
        /// </summary>
        /// <param name="obj">Доменный объект, который необходимо сохранить в источнике данных</param>
        void Create(T obj);
        /// <summary>
        /// Обновляет данные существующего доменного объекта в репозитории
        /// </summary>
        /// <param name="obj">Доменный объект с обновленными свойствами</param>
        void Update(T obj);
        /// <summary>
        /// Удаляет указанный доменный объект из репозитория
        /// </summary>
        /// <param name="obj">Доменный объект, который необходимо удалить из источника данных</param>
        void Delete(T obj);
    }

}
