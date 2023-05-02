using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using SharpDX;
using System;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class BallBouncesController : BehaviorObject
    {
        private readonly BallBounceProvider _ballBounceProvider;
        private PlayerRacket _playerRacket;
        private EnemyRacket _enemyRacket;

        private bool _knockChecked;
        private bool _inGameChecked;
        private bool _isLose;

        public event Action<RacketType> Losed;
        public event Action<GameState, RacketType> StateChanged;
        public event Action Restarted;

        public BallBouncesController(BallBounceProvider ballBounceProvider)
        {
            _ballBounceProvider = ballBounceProvider;
            _ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;
        }

        public void Restart()
        {
            _inGameChecked = false;
            _isLose = false;
            _knockChecked = false;
            Restarted?.Invoke();
        }

        public void SetupRackets(PlayerRacket playerRacket, EnemyRacket enemyRacket)
        {
            _playerRacket = playerRacket;
            _enemyRacket = enemyRacket;
        }


        private void BallBounceProviderOnBallBounced(Component bouncedFrom, BallModel ball)
        {
            _isLose = false;

            if (ball.BallGameState == BallGameState.None)
            {
                if (bouncedFrom.GameObject.HasComponent<FloorModel>())
                {
                    OnStateChanged(GameState.DidntKnock, RacketType.Player);
                    OnLose(RacketType.Player);
                    ReturnToStartPositionRandom(ball, RacketType.Player);
                }

                return;
            }

            if (ball.BallGameState == BallGameState.Knocked)
            {
                OnStateChanged(GameState.Knock, GetRacketType(bouncedFrom));
                _knockChecked = false;
                _isLose = CheckScoreOnBallKnocked(bouncedFrom, ball, RacketType.Player, RacketType.Enemy);
                _isLose = CheckScoreOnBallKnocked(bouncedFrom, ball, RacketType.Enemy, RacketType.Player);
            }

            if (ball.BallGameState == BallGameState.InPlay)
            {
                OnStateChanged(GameState.Kicked, GetRacketType(bouncedFrom));
                _inGameChecked = false;
                _isLose = CheckScoreOnBallInPlay(bouncedFrom, ball, RacketType.Player, RacketType.Enemy);
                _isLose = CheckScoreOnBallInPlay(bouncedFrom, ball, RacketType.Enemy, RacketType.Player);
            }

            if (_isLose == false)
            {
                GetRacket(RacketType.Enemy).OnBallBounced(ball);
                GetRacket(RacketType.Player).OnBallBounced(ball);
            }
        }

        private bool CheckScoreOnBallInPlay(Component bouncedFrom, BallModel ballModel,
            RacketType current, RacketType opposite)
        {
            if (_inGameChecked)
            {
                return _isLose;
            }

            var fromFloor = bouncedFrom.GameObject.HasComponent<FloorModel>();

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTableTimes == 0 && fromFloor)
            {
                OnStateChanged(GameState.DidntHitTable, current);
                return LoseRacketOnPlay(current, opposite, ballModel);
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == current &&
                ballModel.BouncedFromTableTimes >= 1)
            {
                OnStateChanged(GameState.TooManyBouncesFromTable, current);
                return LoseRacketOnPlay(current, opposite, ballModel);
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == opposite &&
                ballModel.BouncedFromTableTimes >= 2)
            {
                OnStateChanged(GameState.TooManyBouncesFromTable, opposite);
                return LoseRacketOnPlay(opposite, current, ballModel);
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == opposite && fromFloor)
            {
                OnStateChanged(GameState.KickSucceed, current);
                return LoseRacketOnPlay(opposite, current, ballModel);
            }

            return false;
        }


        private bool CheckScoreOnBallKnocked(Component bouncedFrom, BallModel ball, 
            RacketType current, RacketType opposite)
        {
            if (_knockChecked)
            {
                return _isLose;
            }

            var fromFloor = bouncedFrom.GameObject.HasComponent<FloorModel>();

            if (ball.BouncedFromRacket == current &&
                ball.BouncedFromTablePart == current &&
                fromFloor)
            {
                if (ball.BouncedFromTableTimes == 0)
                {
                    OnStateChanged(GameState.DidntHitTable, current);
                    return LoseRacketOnKnock(current, opposite, ball);
                }

                if (ball.BouncedFromTableTimes == 1)
                {
                    OnStateChanged(GameState.DidntHitOppositeTable, current);
                    return LoseRacketOnKnock(current, opposite, ball);
                }
            }

            if (ball.BouncedFromRacket == current &&
                ball.BouncedFromTablePart == current &&
                ball.BouncedFromTableTimes >= 2)
            {
                OnStateChanged(GameState.TooManyBouncesFromTable, current);
                return LoseRacketOnPlay(current, opposite, ball);
            }

            if (ball.BouncedFromRacket == current &&
                ball.BouncedFromTablePart == opposite &&
                ball.BouncedFromTableTimes >= 2)
            {
                OnStateChanged(GameState.TooManyBouncesFromTable, opposite);
                return LoseRacketOnKnock(opposite, current, ball);
            }

            if (ball.BouncedFromTablePart == opposite && fromFloor)
            {
                OnStateChanged(GameState.KnockSucceed, current);
                return LoseRacketOnKnock(opposite, current, ball);
            }

            return false;
        }

        private RacketType GetRacketType(Component bouncedFrom)
        {
            if (bouncedFrom.GameObject.HasComponent<PlayerRacket>())
            {
                return RacketType.Player;
            }

            if (bouncedFrom.GameObject.HasComponent<EnemyRacket>())
            {
                return RacketType.Enemy;
            }

            return RacketType.None;
        }

        private void OnLose(RacketType racketType) => Losed?.Invoke(racketType);
        private void OnStateChanged(GameState gameState, RacketType racketType)
        {
            if (racketType == RacketType.None)
            {
                return;
            }
            StateChanged?.Invoke(gameState, racketType);
        }

        private bool LoseRacketOnPlay(RacketType lose, RacketType opposite, BallModel ball)
        {
            _inGameChecked = true;
            return Lose(lose, opposite, ball);
        }


        private bool LoseRacketOnKnock(RacketType lose, RacketType opposite, BallModel ball)
        {
            _knockChecked = true;
            return Lose(lose, opposite, ball);
        }

        private bool Lose(RacketType lose, RacketType opposite, BallModel ball)
        {
            GetRacket(lose).OnLost(ball);
            ReturnToStartPositionRandom(ball, lose);
            Losed?.Invoke(lose);
            return true;
        }

        private void ReturnToStartPositionRandom(BallModel ball, RacketType racketType)
        {
            var isRandom = racketType == RacketType.Player;
            var position = racketType == RacketType.Player ? -50 : 50;
            var z = isRandom ? new Random().Next(-20, 20) : 0;
            ball.RigidBody.EnableGravity();
            ball.SetSpeed(Vector3.Zero);
            ball.BallGameState = BallGameState.None;
            ball.BouncedFromTableTimes = 0;
            ball.BouncedFromTablePart = RacketType.None;
            ball.BouncedFromRacket = RacketType.None;
            ball.Transform.SetPosition(new Vector3(position, 20, z));
        }

        private RacketBase GetRacket(RacketType racketType) =>
            racketType == RacketType.Player ? (RacketBase)_playerRacket : _enemyRacket;
    }
}
