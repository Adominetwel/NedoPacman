namespace NedoPacmanVuZ
{
    internal record struct Vector2(int X, int Y)
    {
        public static readonly Vector2 Right = new(1, 0);
        public static readonly Vector2 Left = new(-1, 0);
        public static readonly Vector2 Up = new(0, -1);
        public static readonly Vector2 Down = new(0, 1);
        public static readonly Vector2 None = new(0, 0);
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
    }

}
