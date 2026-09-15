using System;
using System.Collections.Generic;
using System.Linq;
namespace NedoPacmanVuZ.View
{
    internal interface IInputProvider
    {
        Vector2 GetNextDirection(out bool shootPressed);
    }
}
