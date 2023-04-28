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
            if (ball.BallGameState == BallGameState.None && _knockShowed == false)
            {
                _gameStateView.Show(GameState.Knock, RacketType.Player.ToString());
                _knockShowed = true;
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
                _scoreSystem.AddScore(1, opposite);
                return;
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == current &&
                ballModel.BouncedFromTableTimes >= 1)
            {
                _inGameChecked = true;
                _scoreSystem.AddScore(1, opposite);
                ReturnToStartPositionRandom(ballModel);
                return;
            }

            if (ballModel.BouncedFromRacket == current &&
                ballModel.BouncedFromTablePart == opposite && fromFloor)
            {
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
                _scoreSystem.AddScore(1, opposite);
                _knockChecked = true;
                return;
            }

            if (ball.BouncedFromTablePart == opposite)
            {
                _scoreSystem.AddScore(1, current);
            }
        }

        private void ReturnToStartPositionRandom(BallModel ball)
        {
            var z = new Random().Next(-20, 20);
            ball.RigidBody.EnableGravity();
            ball.SetSpeed(Vector3.Zero);
            ball.Transform.SetPosition(new Vector3(-50, 20, z));
        }
    }
}
