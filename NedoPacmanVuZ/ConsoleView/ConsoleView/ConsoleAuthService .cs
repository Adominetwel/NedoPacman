using DAL.EntityFramework;
using Model.Model.User;
using Model.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleView.ConsoleView
{
    public class ConsoleAuthService : IAuthService
    {
        private readonly EfUserRepository _repo;
        public ConsoleAuthService(EfUserRepository repo)
        {
            _repo = repo;
        }
        public Task<User> Authenticate()
        {
            Console.CursorVisible = true;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==========================================================");
                Console.WriteLine("                NEDO PACMAN - ВХОД ИГРОКА                 ");
                Console.WriteLine("==========================================================");
                Console.ResetColor();
                var users = _repo.ReadAll().ToList();
                Console.WriteLine(" Доступные игровые профили:");
                for (int i = 0; i < users.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {users[i].Username}");
                }
                Console.WriteLine("\n  0. Создать новый игровой профиль");
                Console.Write("\n Ваш выбор: ");
                string input = Console.ReadLine();
                if (input == "0")
                {
                    Console.Write(" Введите имя нового игрока: ");
                    string name = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        var newUser = new User { Username = name };
                        _repo.Create(newUser);
                        return Task.FromResult(newUser);
                    }
                }
                else if (int.TryParse(input, out int idx) && idx > 0 && idx <= users.Count)
                {
                    return Task.FromResult(users[idx - 1]);
                }
            }
        }
    }

}
