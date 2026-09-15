using System;
using System.Linq;
using NedoPacmanVuZ.Entities;
using NedoPacmanVuZ.Entities.Collectibles;
using NedoPacmanVuZ.FactoryPattern;

namespace NedoPacmanVuZ
{
    internal class GameCore : IGameContext
    {
        private int _collectedDotsCounter = 0;
        private Vector2 _currentDirection = Vector2.None;
        private Vector2 _nextDirection = Vector2.None;
        private int _modeTicksCounter = 0;
        private int _frightenedTicksLeft = 0;
        private int _stepsSinceStart = 0;
        private const int ScatterDurationTicks = 28;
        private const int ChaseDurationTicks = 80;
        private const int FrightenedDurationTicks = 40;
        private const int ReleaseIntervalSteps = 20;
        private const int DotsForAmmo = 20;
        private readonly List<Vector2> _cageSpawnPoints = new()
        {
            new Vector2(13, 13), new Vector2(14, 13), new Vector2(15, 13)
        };
        public GameMap World { get; private set; }
        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }
        public int AmmoCount { get; private set; }

        public Vector2 PlayerPosition => World.GetPlayer()?.Position ?? Vector2.None;
        public Vector2 PlayerDirection => _currentDirection;
        public Vector2 BlinkyPosition => World.Ghosts.FirstOrDefault(e => e.TypeId == "ghost.blinky")?.Position ?? PlayerPosition;
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
        public GameCore(GameMap world)
        {
            World = world;
            World.OnScorePointsEarned += HandleScoreEarned;
            World.OnEnergizerTriggered += HandleEnergizerTriggered;
        }
        private void HandleScoreEarned(int points)
        {
            Score += points;
            if (points == 1)
            {
                _collectedDotsCounter++;
                if (_collectedDotsCounter >= DotsForAmmo)
                {
                    AmmoCount++;
                    _collectedDotsCounter = 0;
                }
            }
        }
        public void TryFire()
        {
            if (AmmoCount <= 0 || PlayerDirection == Vector2.None) return;
            AmmoCount--;
            Vector2 spawnPos = World.WrapPosition(PlayerPosition + PlayerDirection);
            if (!World.IsWallAt(spawnPos))
            {
                var projectile = new Projectile(spawnPos, PlayerDirection);
                projectile.OnMoved += HandleProjectileMoved;
                World.AddEntity(projectile);
                EvaluateProjectileCollision(projectile);
            }
        }

        public void Update(Vector2 playerDirection, bool shootPressed)
        {
            UpdateGameModeTimer();
            ReleaseGhostsIfNeeded();

            if (IsGameOver) return;

            if (shootPressed)
                TryFire();

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
                    _currentDirection = Vector2.None;
            }

            CheckGhostCollision();
            if (IsGameOver) return;

            UpdateGhosts();
            UpdateProjectiles();
            CheckGhostCollision();

            World.RespawnDotsIfNeeded();
            CheckCageCondition();
            CheckWinCondition();
        }

        private void CheckCollectiblePickup()
        {
            if (World.Entities.FirstOrDefault(e => e.Position == PlayerPosition && e is CollectibleItem) is CollectibleItem item)
                item.OnCollect();
        }
        private void HandleEnergizerTriggered()
        {
            CurrentGhostMode = GhostMode.Frightened;
            _frightenedTicksLeft = FrightenedDurationTicks;
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
            foreach (var ghost in World.Ghosts.ToList())
            {
                if (ghost.IsInHouse || ghost.State != GhostState.Active) continue;
                ghost.Update(this);
            }
        }
        private void UpdateProjectiles()
        {
            var projectiles = World.Projectiles.ToList();
            foreach (var proj in projectiles)
            {
                bool destroyed = false;
                for (int step = 0; step < 2; step++)
                {
                    Vector2 previousProjPos = proj.Position;
                    Vector2 nextPos = World.WrapPosition(proj.Position + proj.Direction);

                    if (World.IsWallAt(nextPos))
                    {
                        DestroyProjectile(proj);
                        destroyed = true;
                        break;
                    }
                    if (EvaluateBypassCollision(previousProjPos, nextPos, proj))
                    {
                        destroyed = true;
                        break;
                    }
                    proj.Move(World);
                    if (EvaluateProjectileCollision(proj))
                    {
                        destroyed = true;
                        break;
                    }
                }

                if (destroyed) continue;
            }
        }

        private void HandleProjectileMoved(Projectile projectile)
        {
            EvaluateProjectileCollision(projectile);
        }
        private bool EvaluateBypassCollision(Vector2 currentProjPos, Vector2 nextProjPos, Projectile proj)
        {
            Ghost? встречныйПризрак = World.Ghosts.FirstOrDefault(g =>
                g.State == GhostState.Active &&
                g.Position == currentProjPos);
            if (встречныйПризрак != null)
            {
                proj.Position = currentProjPos;
                return EvaluateProjectileCollision(proj);
            }
            return false;
        }
        private bool EvaluateProjectileCollision(Projectile proj)
        {
            Ghost? hitGhost = World.Ghosts.FirstOrDefault(g => g.State != GhostState.Dead && g.Position == proj.Position);
            if (hitGhost == null) return false;

            DestroyProjectile(proj);
            hitGhost.HitCount++;
            if (hitGhost.State == GhostState.InCage || hitGhost.HitCount >= 2)
            {
                hitGhost.State = GhostState.Dead;
                World.RemoveEntity(hitGhost);
            }
            else
            {
                hitGhost.State = GhostState.InCage;
                Vector2 targetCagePos = _cageSpawnPoints[0];
                foreach (var pos in _cageSpawnPoints)
                {
                    if (!World.Ghosts.Any(g => g.Position == pos))
                    {
                        targetCagePos = pos;
                        break;
                    }
                }
                hitGhost.Position = targetCagePos;
            }
            return true;
        }

        private void DestroyProjectile(Projectile proj)
        {
            proj.OnMoved -= HandleProjectileMoved;
            World.RemoveEntity(proj);
        }
        private void CheckCageCondition()
        {
            var ghosts = World.Ghosts;
            if (ghosts.Count > 0 && ghosts.All(g => g.State == GhostState.InCage))
            {
                World.RemoveCageWalls();
                foreach (var g in ghosts)
                    g.State = GhostState.Active;
            }
        }
        private void CheckWinCondition()
        {
            if (!World.Ghosts.Any(g => g.State != GhostState.Dead))
            {
                IsGameOver = true;
                IsWin = true;
            }
        }
        private void CheckGhostCollision()
        {
            Ghost? ghostOnPlayer = null;
            for (int i = 0; i < World.Ghosts.Count; i++)
            {
                if (World.Ghosts[i].State == GhostState.Active && World.Ghosts[i].Position == PlayerPosition)
                {
                    ghostOnPlayer = World.Ghosts[i];
                    break;
                }
            }

            if (ghostOnPlayer != null)
            {
                if (CurrentGhostMode == GhostMode.Frightened)
                {
                    Score += 52;
                    ghostOnPlayer.HitCount++;
                    if (ghostOnPlayer.State == GhostState.InCage || ghostOnPlayer.HitCount >= 2)
                    {
                        ghostOnPlayer.State = GhostState.Dead;
                        World.RemoveEntity(ghostOnPlayer);
                    }
                    else
                    {
                        ghostOnPlayer.State = GhostState.InCage;
                        Vector2 targetCagePos = _cageSpawnPoints[0];
                        foreach (var pos in _cageSpawnPoints)
                        {
                            if (!World.Ghosts.Any(g => g.Position == pos)) { targetCagePos = pos; break; }
                        }
                        ghostOnPlayer.Position = targetCagePos;
                    }
                }
                else { IsGameOver = true; IsWin = false; }
            }
        }
        public bool CheckCollision(Vector2 targetPosition, Ghost checkingGhost = null) { if (World.IsWallAt(targetPosition)) return true; return World.Ghosts.Any(g => g != checkingGhost && g.State == GhostState.Active && !g.IsInHouse && g.Position == World.WrapPosition(targetPosition)); }
        public Vector2 GetScatterTarget(string ghostTypeId) => ghostTypeId switch { "ghost.blinky" => new Vector2(World.Width - 2, -2), "ghost.pinky" => new Vector2(2, -2), "ghost.inky" => new Vector2(World.Width - 2, World.Height + 2), "ghost.clyde" => new Vector2(2, World.Height + 2), _ => PlayerPosition }; private void ReleaseGhostsIfNeeded() { _stepsSinceStart++; if (_stepsSinceStart % ReleaseIntervalSteps == 0) { var ghostInHouse = World.Ghosts.FirstOrDefault(g => g.IsInHouse && g.State == GhostState.Active); if (ghostInHouse != null) { ghostInHouse.Release(new Vector2(14, 11)); } } }
    }
}