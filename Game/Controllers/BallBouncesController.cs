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
        private readonly ScoreSystem _scoreSystem;
        private readonly GameStateViewController _gameStateView;
        private readonly PlayerRacket _playerRacket;
        private readonly EnemyRacket _enemyRacket;

        private bool _knockShowed;
        private bool _knockChecked;
        private bool _inGameChecked;
        private bool _isLose;

        public BallBouncesController(BallBounceProvider ballBounceProvider,
            ScoreSystem scoreSystem, 
            GameStateViewController gameStateView,
            PlayerRacket playerRacket,
            EnemyRacket enemyRacket)
        {
            _ballBounceProvider = ballBounceProvider;
            _scoreSystem = scoreSystem;
            _gameStateView = gameStateView;
            _playerRacket = playerRacket;
            _enemyRacket = enemyRacket;
            _ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;
        }

        public override void OnDestroy()
        {
            _ballBounceProvider.BallBounced -= BallBounceProviderOnBallBounced;
        }

        private void BallBounceProviderOnBallBounced(Component bouncedFrom, BallModel ball)
        {
            _isLose = false;

            if (ball.BallGameState == BallGameState.None)
            {
                if (_knockShowed == false)
                {
                    _gameStateView.Show(GameState.Knock, RacketType.Player.ToString());
                    _knockShowed = true;
                }

                if (bouncedFrom.GameObject.HasComponent<FloorModel>())
                {
                    _scoreSystem.AddScore(1, RacketType.Enemy);
                    ReturnToStartPositionRandom(ball, RacketType.Player);
                }

                return;
            }

            if (ball.BallGameState == BallGameState.Knocked)
            {
                _knockChecked = false;
                _isLose = CheckScoreOnBallKnocked(bouncedFrom, ball, RacketType.Player, RacketType.Enemy);
                _isLose = CheckScoreOnBallKnocked(bouncedFrom, ball, RacketType.Enemy, RacketType.Player);
            }

            if (ball.BallGameState == BallGameState.InPlay)
            {
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
                ballModel.BouncedFromTableTimes == 0 && 
                fromFloor)
            {
                return LoseRacketOnPlay(current, opposite, ballModel);
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == current &&
                ballModel.BouncedFromTableTimes >= 1)
            {
                return LoseRacketOnPlay(current, opposite, ballModel);
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == opposite &&
                ballModel.BouncedFromTableTimes >= 2)
            {
                return LoseRacketOnPlay(opposite, current, ballModel);
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == opposite &&
                fromFloor)
            {
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
                (ball.BouncedFromTableTimes == 0 || ball.BouncedFromTableTimes == 1) &&
                fromFloor)
            {
                return LoseRacketOnKnock(current, opposite, ball);
            }

            if (ball.BouncedFromRacket == current &&
                ball.BouncedFromTablePart == current &&
                ball.BouncedFromTableTimes >= 2)
            {
                return LoseRacketOnPlay(current, opposite, ball);
            }

            if (ball.BouncedFromRacket == current &&
                ball.BouncedFromTablePart == opposite &&
                ball.BouncedFromTableTimes >= 2)
            {
                return LoseRacketOnKnock(opposite, current, ball);
            }

            if (ball.BouncedFromTablePart == opposite && fromFloor)
            {
                return LoseRacketOnKnock(opposite, current, ball);
            }

            return false;
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
            _scoreSystem.AddScore(1, opposite);
            GetRacket(lose).OnLost(ball);
            ReturnToStartPositionRandom(ball, lose);
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
