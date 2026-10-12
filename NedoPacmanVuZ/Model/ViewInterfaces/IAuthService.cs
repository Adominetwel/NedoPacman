using Model.Model.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewInterfaces
{
    public interface IAuthService
    {
        User Authenticate();
    }
}
