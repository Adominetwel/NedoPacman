using Model.Model.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewInterfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Авторизует пользователя
        /// </summary>
        /// <returns>Ссылка на объект пользователя (обёрнуто в таск)</returns>
        Task<User> Authenticate();
    }
}
