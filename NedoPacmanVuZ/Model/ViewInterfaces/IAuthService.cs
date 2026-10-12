using Model.Model.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewInterfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Авторизует пользователя
        /// </summary>
        /// <returns>Ссылка на объект пользователя</returns>
        User Authenticate();
    }
}
