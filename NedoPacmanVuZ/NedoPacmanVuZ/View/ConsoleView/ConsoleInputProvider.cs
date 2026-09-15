namespace NedoPacmanVuZ.View.ConsoleView
{
    internal class ConsoleInputProvider : IInputProvider
    {
        public Vector2 GetNextDirection()
        {
            Vector2 lastValidDir = Vector2.None;
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                var dir = key switch
                {
                    ConsoleKey.UpArrow => Vector2.Up,
                    ConsoleKey.DownArrow => Vector2.Down,
                    ConsoleKey.LeftArrow => Vector2.Left,
                    ConsoleKey.RightArrow => Vector2.Right,
                    _ => Vector2.None
                };

                if (dir != Vector2.None)
                    lastValidDir = dir;
            }
            return lastValidDir;
        }
    }
}
