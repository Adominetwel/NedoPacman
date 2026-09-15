using System;
using System.Linq;
using NedoPacmanVuZ.Entities;
using NedoPacmanVuZ.Entities.Collectibles;
using NedoPacmanVuZ.FactoryPattern;

namespace NedoPacmanVuZ
{
    internal class GameCore : IGameContext
    {
        public GameMap World { get; private set; }
        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }
        public Vector2 PlayerPosition => World.GetPlayer()?.Position ?? Vector2.None;
        public Vector2 PlayerDirection => _currentDirection;
        public Vector2 BlinkyPosition => World.Entities.FirstOrDefault(e => e.TypeId == "ghost.blinky")?.Position ?? PlayerPosition;
        public int FrightenedTicksLeft => _frightenedTicksLeft;
        public event Action<GhostMode>? OnGhostModeChanged;
        private GhostMode _currentGhostMode = GhostMode.Scatter;
        public GhostMode CurrentGhostMode
        {
            get => _currentGhostMode;
            private set
            {
                if (_currentGhostMode != value)
                {
                    _currentGhostMode = value;
                    OnGhostModeChanged?.Invoke(_currentGhostMode);
                }
            }
        }

        private Vector2 _currentDirection = Vector2.None;
        private Vector2 _nextDirection = Vector2.None;
        private int _modeTicksCounter = 0;
        private int _frightenedTicksLeft = 0;
        private int _stepsSinceStart = 0;
        private const int ScatterDurationTicks = 28;
        private const int ChaseDurationTicks = 80;
        private const int FrightenedDurationTicks = 40;
        private const int ReleaseIntervalSteps = 20;
        public GameCore(GameMap world)
        {
            World = world;
            World.OnScorePointsEarned += (points) => Score += points;
            World.OnEnergizerTriggered += HandleEnergizerTriggered;
            World.OnAllDotsCollected += HandleWin;
        }
        public void Update(Vector2 playerDirection)
        {
            UpdateGameModeTimer();
            ReleaseGhostsIfNeeded();

            if (IsGameOver) return;

            if (playerDirection != Vector2.None)
                _nextDirection = playerDirection;

            Player? player = World.GetPlayer();
            if (player == null) return;

            if (_nextDirection != Vector2.None && !World.IsWallAt(PlayerPosition + _nextDirection))
                _currentDirection = _nextDirection;

            if (_currentDirection != Vector2.None)
            {
                Vector2 nextPlayerPos = PlayerPosition + _currentDirection;
                if (!World.IsWallAt(nextPlayerPos))
                {
                    player.Position = World.WrapPosition(nextPlayerPos);
                    CheckCollectiblePickup();
                }
                else
                {
                    _currentDirection = Vector2.None;
                }
            }

            CheckGhostCollision();
            if (IsGameOver) return;
            UpdateGhosts();
            CheckGhostCollision();
        }

        private void CheckCollectiblePickup()
        {
            if (World.Entities.FirstOrDefault(e => e.Position == PlayerPosition && e is CollectibleItem) is CollectibleItem item)
            {
                item.OnCollect();
            }
        }

        private void HandleEnergizerTriggered()
        {
            CurrentGhostMode = GhostMode.Frightened;
            _frightenedTicksLeft = FrightenedDurationTicks;
        }
        private void HandleWin()
        {
            IsGameOver = true;
            IsWin = true;
        }
        private void UpdateGameModeTimer()
        {
            if (CurrentGhostMode == GhostMode.Frightened)
            {
                _frightenedTicksLeft--;
                if (_frightenedTicksLeft <= 0)
                {
                    CurrentGhostMode = GhostMode.Chase;
                    _modeTicksCounter = 0;
                }
                return;
            }

            _modeTicksCounter++;
            if (CurrentGhostMode == GhostMode.Scatter && _modeTicksCounter >= ScatterDurationTicks)
            {
                CurrentGhostMode = GhostMode.Chase;
                _modeTicksCounter = 0;
            }
            else if (CurrentGhostMode == GhostMode.Chase && _modeTicksCounter >= ChaseDurationTicks)
            {
                CurrentGhostMode = GhostMode.Scatter;
                _modeTicksCounter = 0;
            }
        }

        private void UpdateGhosts()
        {
            foreach (var ghost in World.Entities.OfType<Ghost>().ToList())
            {
                if (ghost.IsInHouse) continue;
                ghost.Update(this);
            }
        }

        private void CheckGhostCollision()
        {
            var ghostOnPlayer = World.Entities.OfType<Ghost>().FirstOrDefault(g => g.Position == PlayerPosition);
            if (ghostOnPlayer != null)
            {
                if (CurrentGhostMode == GhostMode.Frightened)
                {
                    Score += 200;
                    ghostOnPlayer.Release(new Vector2(14, 11));
                }
                else
                {
                    IsGameOver = true;
                    IsWin = false;
                }
            }
        }

        public bool CheckCollision(Vector2 targetPosition, Ghost checkingGhost = null)
        {
            if (World.IsWallAt(targetPosition)) return true;
            return World.Entities.OfType<Ghost>().Any(g =>
                g != checkingGhost && !g.IsInHouse && g.Position == World.WrapPosition(targetPosition));
        }

        public Vector2 GetScatterTarget(string ghostTypeId) => ghostTypeId switch
        {
            "ghost.blinky" => new Vector2(World.Width - 2, -2),
            "ghost.pinky" => new Vector2(2, -2),
            "ghost.inky" => new Vector2(World.Width - 2, World.Height + 2),
            "ghost.clyde" => new Vector2(2, World.Height + 2),
            _ => PlayerPosition
        };
        private void ReleaseGhostsIfNeeded()
        {
            _stepsSinceStart++;
            if (_stepsSinceStart % ReleaseIntervalSteps == 0)
            {
                var ghostInHouse = World.Entities.OfType<Ghost>().FirstOrDefault(g => g.IsInHouse);
                if (ghostInHouse != null)
                {
                    ghostInHouse.Release(new Vector2(14, 11));
                }
            }
        }
    }
}
