using NedoPacmanVuZ.Model;
using NedoPacmanVuZ.Model.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NedoPacmanVuZ.View.ConsoleView
{
    internal class ConsoleMenu : ILevelSelector
    {
        private readonly List<Level> _levels;
        private int _selectedIndex = 0;
        private readonly LevelRepository _repository;
        public ConsoleMenu(IEnumerable<Level> levels, LevelRepository repository)
        {
            _levels = levels.ToList();
            _repository = repository;
        }
        public Level SelectLevel()
        {
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                RenderHeader();

                for (int i = 0; i < _levels.Count; i++)
                {
                    var lvl = _levels[i];
                    string progressStatus = lvl.IsPassed ? "[ПРОЙДЕН]" : "[      ]";

                    if (i == _selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"  > {progressStatus} {lvl.Name.PadRight(40)} [Режим: {lvl.GameMode.ModeId.Replace("mode.", "")}] ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = lvl.IsPassed ? ConsoleColor.Green : ConsoleColor.Gray; // Пройденные уровни подсвечиваем зеленым
                        Console.WriteLine($"    {progressStatus} {lvl.Name.PadRight(40)} [Режим: {lvl.GameMode.ModeId.Replace("mode.", "")}] ");
                    }
                }

                Console.ResetColor();
                RenderFooter();
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
                {
                    _selectedIndex = (_selectedIndex == 0) ? _levels.Count - 1 : _selectedIndex - 1;
                }
                else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
                {
                    _selectedIndex = (_selectedIndex == _levels.Count - 1) ? 0 : _selectedIndex + 1;
                }
                else if (key == ConsoleKey.R)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Вы уверены, что хотите сбросить ВЕСЬ прогресс? (Y/N)");
                    Console.ResetColor();
                    var confirm = Console.ReadKey(true).Key;
                    if (confirm == ConsoleKey.Y)
                    {
                        _repository.ResetAllProgress();
                        _levels.Clear();
                        _levels.AddRange(_repository.GetAllLevels());
                        _selectedIndex = 0;
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return _levels[_selectedIndex];
                }
            }
        }

        private void RenderHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==========================================================");
            Console.WriteLine("                NEDO PACMAN - ВЫБОР УРОВНЯ               ");
            Console.WriteLine("==========================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void RenderFooter()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine(" Навигация: [W / S] или [Стрелочки]. Выбор: [Enter]");
            Console.WriteLine("----------------------------------------------------------");
            Console.ResetColor();
        }
    }
}
