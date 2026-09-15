namespace NedoPacmanVuZ.View
{
    internal interface IInputProvider
    {
        Vector2 GetNextDirection(out bool shootPressed);
    }
}
