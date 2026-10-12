using Model.FileManager;
using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.FileManager
{
    public interface ILevelRepository : IRepository<Level>
    {
        void ResetAllProgress();
    }
}
