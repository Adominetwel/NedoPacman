namespace NedoPacmanVuZ.View.ConsoleView
{
    internal class ConsoleInputProvider : IInputProvider
    {
        public Vector2 GetNextDirection(out bool shootPressed)
        {
            Vector2 lastValidDir = Vector2.None;
            shootPressed = false;

            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.F)
                {
                    shootPressed = true;
                    continue;
                }

                var dir = key switch
                {
                    ConsoleKey.W => Vector2.Up,
                    ConsoleKey.S => Vector2.Down,
                    ConsoleKey.A => Vector2.Left,
                    ConsoleKey.D => Vector2.Right,
                    _ => Vector2.None
                };

                if (dir != Vector2.None)
                {
                    lastValidDir = dir;
                }
            }
            return lastValidDir;
        }
    }
}
