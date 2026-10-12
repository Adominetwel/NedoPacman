using Model;
using Model.FileManager;
using Model.Model.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.EntityFramework
{
    public class EfUserRepository : IRepository<User>
    {
        private readonly string _connectionString;

        public EfUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(User obj)
        {
            using var context = new ApplicationContext(_connectionString);
            context.Users.Add(obj);
            context.SaveChanges();
        }

        public IEnumerable<User> ReadAll()
        {
            using var context = new ApplicationContext(_connectionString);
            return context.Users.ToList();
        }

        public User ReadById(int id)
        {
            using var context = new ApplicationContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public void Update(User obj)
        {
            using var context = new ApplicationContext(_connectionString);
            context.Users.Update(obj);
            context.SaveChanges();
        }

        public void Delete(User obj)
        {
            using var context = new ApplicationContext(_connectionString);
            context.Users.Remove(obj);
            context.SaveChanges();
        }
    }
}
