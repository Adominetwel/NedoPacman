using NedoPacmanVuZ.Entities;

namespace NedoPacmanVuZ
{
    internal interface IGameContext
    {
        bool CheckCollision(Vector2 targetPosition, Ghost checkingGhost = null);
        Vector2 PlayerPosition { get; }
        Vector2 PlayerDirection { get; } 
        Vector2 BlinkyPosition { get; }
        GhostMode CurrentGhostMode { get; }
        GameMap World { get; }
        Vector2 GetScatterTarget(string ghostTypeId);
        event Action<GhostMode>? OnGhostModeChanged;
    }
}
