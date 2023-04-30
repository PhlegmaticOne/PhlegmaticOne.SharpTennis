using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using SharpDX;
using System;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class BallBouncesController : BehaviorObject
    {
        private readonly BallBounceProvider _ballBounceProvider;
        private readonly ScoreSystem _scoreSystem;
        private readonly GameStateViewController _gameStateView;

        private bool _knockShowed;
        private bool _knockChecked;
        private bool _inGameChecked;

        public BallBouncesController(BallBounceProvider ballBounceProvider,
            ScoreSystem scoreSystem, 
            GameStateViewController gameStateView)
        {
            _ballBounceProvider = ballBounceProvider;
            _scoreSystem = scoreSystem;
            _gameStateView = gameStateView;
            _ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;
        }

        public override void OnDestroy()
        {
            _ballBounceProvider.BallBounced -= BallBounceProviderOnBallBounced;
        }

        private void BallBounceProviderOnBallBounced(Component bouncedFrom, BallModel ball)
        {
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
                CheckScoreOnBallKnocked(bouncedFrom, ball, RacketType.Player, RacketType.Enemy);
                CheckScoreOnBallKnocked(bouncedFrom, ball, RacketType.Enemy, RacketType.Player);
                return;
            }

            if (ball.BallGameState == BallGameState.InPlay)
            {
                _inGameChecked = false;
                CheckScoreOnBallInPlay(bouncedFrom, ball, RacketType.Player, RacketType.Enemy);
                CheckScoreOnBallInPlay(bouncedFrom, ball, RacketType.Enemy, RacketType.Player);
            }
        }

        private void CheckScoreOnBallInPlay(Component bouncedFrom, BallModel ballModel,
            RacketType current, RacketType opposite)
        {
            if (_inGameChecked)
            {
                return;
            }

            var fromFloor = bouncedFrom.GameObject.HasComponent<FloorModel>();

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTableTimes == 0 && fromFloor)
            {
                _inGameChecked = true;
                ReturnToStartPositionRandom(ballModel, current);
                _scoreSystem.AddScore(1, opposite);
                return;
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == current &&
                ballModel.BouncedFromTableTimes >= 1)
            {
                _inGameChecked = true;
                _scoreSystem.AddScore(1, opposite);
                ReturnToStartPositionRandom(ballModel, current);
                return;
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == opposite && fromFloor)
            {
                _inGameChecked = true;
                ReturnToStartPositionRandom(ballModel, opposite);
                _scoreSystem.AddScore(1, current);
            }
        }


        private void CheckScoreOnBallKnocked(Component bouncedFrom, BallModel ball, 
            RacketType current, RacketType opposite)
        {
            if (_knockChecked || bouncedFrom.GameObject.HasComponent<FloorModel>() == false)
            {
                return;
            }

            if (ball.BouncedFromRacket == current &&
                (ball.BouncedFromTableTimes == 0 || ball.BouncedFromTableTimes == 1))
            {
                ReturnToStartPositionRandom(ball, current);
                _scoreSystem.AddScore(1, opposite);
                _knockChecked = true;
                return;
            }

            if (ball.BouncedFromTablePart == opposite)
            {
                _scoreSystem.AddScore(1, current);
                ReturnToStartPositionRandom(ball, opposite);
            }
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
    }
}
