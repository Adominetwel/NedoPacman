using Model.DALInterfaces;
using NedoPacmanVuZ.Model.MainLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.FileManager
{
    public interface IRepository<T> : IDomainRepository<T> where T : IDomainObject, new()
    {
    }
}
