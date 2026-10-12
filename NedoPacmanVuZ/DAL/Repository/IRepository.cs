using Model.DALInterfaces;
using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repository
{
    /// <summary>
    /// Универсальный интерфейс репозитория для управления доменными объектами в источнике данных.
    /// Предоставляет базовый набор CRUD-операций (Создание, Чтение, Обновление, Удаление)
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующий интерфейс IDomainObject</typeparam>
    public interface IRepository<T> : IReadOnlyDomainRepository<T>, IWriteOnlyDomainRepository<T> where T : IDomainObject, new()
    {

    }
}
