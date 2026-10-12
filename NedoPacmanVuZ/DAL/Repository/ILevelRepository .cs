using Model.DALInterfaces;
using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repository
{
    public interface ILevelRepository : IReadOnlyDomainRepository<Level>
    {
        /// <summary>
        /// Обновляет данные существующего доменного объекта в репозитории
        /// </summary>
        /// <param name="obj">Доменный объект с обновленными свойствами</param>
        void Update(Level obj);
        /// <summary>
        /// Сбрасывает игровой прогресс
        /// </summary>
        void ResetAllProgress();
    }
}
