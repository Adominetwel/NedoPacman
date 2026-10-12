using Model.DALInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.User
{
    public class User : IDomainObject
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }   
}
