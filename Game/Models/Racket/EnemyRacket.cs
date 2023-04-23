using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;
using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.StateMachine;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.States;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    internal interface IEnemyRacketState
    {
        void Update();
    }

    public class EnemyRacket : RacketBase
    {
        private static readonly Vector3 CompareNormal = new Vector3(0, 1, 0); 
        private static readonly float Coeff = -140f;

        private readonly BallBounceProvider _ballBounceProvider;
        private readonly Dictionary<State, Func<IEnemyRacketState>> _states;
        private float _moveToStartLerp;
        private Vector3 _startPosition;
        private Vector3 _approximatedPosition;
        private IEnemyRacketState _state;

        public EnemyRacket(MeshComponent coloredComponent,
            MeshComponent handComponent, 
            BallBounceProvider ballBounceProvider) : 
            base(coloredComponent, handComponent)
        {
            _ballBounceProvider = ballBounceProvider;
            _ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;

            _moveToStartLerp = 0.01f;
            var states = States<EnemyRacketStates>.Get;
            _states = new Dictionary<State, Func<IEnemyRacketState>>
            {
                { State.None, () => new StayingState() },
                { states.StayingState, () => new StayingState() },
                { states.FollowingBallState, () => new FollowingState(_approximatedPosition, 0.03f, this) },
                { states.MovingToStartState, () => new FollowingState(_startPosition, _moveToStartLerp, this) }
            };
            Exit();
        }



        public override void Start()
        {
            _startPosition = Transform.Position;
            base.Start();
        }

        protected override BallBouncedFromType BallBounceType => BallBouncedFromType.Enemy;

        protected override void Update() => _state.Update();

        protected override void OnCollisionWithBall(BallModel ballModel)
        {
            var speed = ballModel.GetSpeed();

            if (speed.X == 0)
            {
                return;
            }

            var newSpeed = new Vector3(-speed.X, speed.Y + 30, speed.Z);
            ballModel.BounceDirect(this, newSpeed);
        }

        public override void OnDestroy()
        {
            _ballBounceProvider.BallBounced -= BallBounceProviderOnBallBounced;
        }


        private void BallBounceProviderOnBallBounced(Component bouncedFrom, BallModel ball)
        {
            if (ball.BouncedFrom == BallBouncedFromType.None || ball.IsInGame == false)
            {
                return;
            }

            var state = FindNewState(bouncedFrom, ball);
            EnterState(state);
        }

        private State FindNewState(Component bouncedFrom, BallModel ball)
        {
            _moveToStartLerp = 0.01f;
            var states = States<EnemyRacketStates>.Get;

            if (bouncedFrom.GameObject.HasComponent<FloorModel>())
            {
                _moveToStartLerp = 0.05f;
                return states.MovingToStartState;
            }


            if (ball.BouncedFrom == BallBouncedFromType.Player)
            {
                _approximatedPosition = CalculateApproximatedPosition(ball);
                return states.FollowingBallState;
            }


            if (ball.BouncedFrom == BallBouncedFromType.Enemy)
            {
                return states.MovingToStartState;
            }

            return states.StayingState;
        }

        private static Vector3 CalculateApproximatedPosition(BallModel ball)
        {
            var ballPosition = ball.Transform.Position;
            var newBallSpeed = ball.GetSpeed();
            var approximatedPosition = ApproximatePosition(newBallSpeed, ballPosition);

            if (approximatedPosition.X <= 0)
            {
                approximatedPosition.X = 0;
            }

            return approximatedPosition;
        }

        private void EnterState(State state) => _state = _states[state]();

        private void Exit() => EnterState(State.None);

        private static Vector3 ApproximatePosition(Vector3 speed, Vector3 initialPosition)
        {
            var flyTime = CalculateFlyTime(speed);
            var newX = initialPosition.X + flyTime * speed.X;
            var newZ = initialPosition.Z + flyTime * speed.Z;
            return new Vector3(newX, initialPosition.Y, newZ);
        }


        private static float CalculateFlyTime(Vector3 speed)
        {
            var speedLength = speed.Length();
            var angleCos = Vector3.Dot(CompareNormal, speed.Normalized());
            var angle = Math.PI / 2 - Math.Acos(angleCos);
            var sine = Math.Sin(angle);
            return (float)(2 * speedLength * sine) / Coeff * RigidBody3D.GlobalAcceleration;
        }




        private class StayingState : IEnemyRacketState
        {
            public void Update() { }
        }

        private class FollowingState : IEnemyRacketState 
        {
            private readonly Vector3 _followToPosition;
            private readonly float _lerp;
            private readonly EnemyRacket _racket;

            public FollowingState(Vector3 followToPosition, float lerp, EnemyRacket racket)
            {
                _followToPosition = followToPosition;
                _lerp = lerp;
                _racket = racket;
            }

            public void Update()
            {
                if (_followToPosition == Vector3.Zero)
                {
                    return;
                }

                var positionLerp = Vector3.Lerp(_racket.Transform.Position, _followToPosition, _lerp);
                _racket.Transform.SetPosition(positionLerp);
            }
        }
    }
}
