using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;
using PhlegmaticOne.SharpTennis.Game.Common.StateMachine;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Floor;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.States;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Wall;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class EnemyRacket : RacketBase
    {
        private const float MinX = 0;
        private const float MaxX = 30;
        private const float MinZ = -25;
        private const float MaxZ = 25;

        private readonly BallBounceProvider _ballBounceProvider;
        private readonly Vector3 _tableNormal;
        private StateComponent _stateComponent;
        private KnockComponent _knockComponent;
        private float _moveToStartLerp;
        private Vector3 _startPosition;
        private Vector3 _approximatedPosition;
        private BallModel _ballModel;

        public EnemyRacket(MeshComponent coloredComponent,
            MeshComponent handComponent, 
            BallBounceProvider ballBounceProvider,
            Vector3 tableNormal) : 
            base(coloredComponent, handComponent)
        {
            _ballBounceProvider = ballBounceProvider;
            _tableNormal = tableNormal;
            _ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;
            _moveToStartLerp = 0.01f;
        }

        protected override RacketType BallBounceType => RacketType.Enemy;


        public override void Start()
        {
            _startPosition = Transform.Position;
            _stateComponent = GameObject.GetComponent<StateComponent>();
            _knockComponent = GameObject.GetComponent<KnockComponent>();
            InitializeStates();
            base.Start();
        }

        public void Knock(BallModel ball)
        {
            var direction = (GetKnockRandomPoint() - Transform.Position).Normalized();
            var force = Random.Next(100, 200);
            var y = Random.Next(40, 50);

            ball.BouncedFromRacket = BallBounceType;
            ball.BallGameState = BallGameState.Knocked;
            _knockComponent.KnockBall(ball, direction, force, -1 * y);
            _stateComponent.Enter(States<EnemyRacketStates>.Get.MovingToStartState);
        }


        protected override void OnCollisionWithBall(BallModel ballModel)
        {
            var speed = ballModel.GetSpeed();

            if (speed.X == 0)
            {
                return;
            }

            var newSpeed = PhysicMathHelper.CalculateSpeedToPoint(ballModel.Transform.Position,
                GetRandomPoint(), GetRandomAngle());
            ballModel.BallGameState = BallGameState.InPlay;
            ballModel.BounceDirect(this, newSpeed);
        }

        private void BallBounceProviderOnBallBounced(Component bouncedFrom, BallModel ball)
        {
            _ballModel = ball;
            if (ball.BouncedFromTablePart == RacketType.None || ball.BallGameState == BallGameState.None)
            {
                return;
            }

            var state = FindNewState(bouncedFrom, ball);
            _stateComponent.Enter(state);
        }

        private void InitializeStates()
        {
            var states = States<EnemyRacketStates>.Get;
            _stateComponent.AddState(states.StayingState, () => new EmptyState());
            _stateComponent.AddState(states.FollowingBallState,
                () => new FollowingObjectState(_approximatedPosition, 0.03f, this));
            _stateComponent.AddState(states.MovingToStartState,
                () => new FollowingObjectState(_startPosition, _moveToStartLerp, this));
            _stateComponent.AddState(states.KnockState, () => new EnemyKnockState(_startPosition, 0.05f, this, _ballModel));
            _stateComponent.Exit();
        }

        private State FindNewState(Component bouncedFrom, BallModel ball)
        {
            _moveToStartLerp = 0.01f;
            var states = States<EnemyRacketStates>.Get;

            if (bouncedFrom.GameObject.HasComponent<FloorModel>())
            {
                if (IsKnockState(bouncedFrom, ball))
                {
                    return states.KnockState;
                }

                _moveToStartLerp = 0.05f;
                return states.MovingToStartState;
            }

            if (ball.BouncedFromRacket == RacketType.Player &&
                ball.BouncedFromTablePart == RacketType.Player)
            {
                return states.MovingToStartState;
            }


            //Отбивание мяча
            if ((ball.BouncedFromRacket == RacketType.Player &&
                ball.BouncedFromTablePart == RacketType.Enemy) ||
                ball.BouncedFromRacket == RacketType.Player)
            {
                var animationTime = 0.2f;
                var ballSpeed = ball.GetSpeed();
                var flyTime = PhysicMathHelper.CalculateFlyTime(ballSpeed, _tableNormal);
                _approximatedPosition = CalculateApproximatedPosition(ball);

                if (PositionMatches(_approximatedPosition))
                {
                    ShakeInTime(new Vector3(10, 0, 0), _approximatedPosition, flyTime - animationTime, animationTime);
                    return states.FollowingBallState;
                }

                return states.MovingToStartState;
            }

            return states.StayingState;
        }


        public void ShakeInTime(Vector3 shake, Vector3 finalPosition, float time, float animationTime, Action onComplete = null)
        {
            Invoke(time, () =>
            {
                _stateComponent.ChangeEnabled(false);
                Transform.DoShake(shake, finalPosition, animationTime, () =>
                {
                    _stateComponent.ChangeEnabled(true);
                    onComplete?.Invoke();
                });
            });
        }

        public override void OnDestroy() => _ballBounceProvider.BallBounced -= BallBounceProviderOnBallBounced;

        private bool IsKnockState(Component bouncedFrom, BallModel ballModel)
        {
            if(ballModel.BallGameState == BallGameState.Knocked && 
               ballModel.BouncedFromRacket == RacketType.Enemy &&
               (ballModel.BouncedFromTableTimes == 0 || ballModel.BouncedFromTableTimes == 1))
            {
                return true;
            }

            if (ballModel.BallGameState == BallGameState.InPlay)
            {
                if (ballModel.BouncedFromRacket == RacketType.Enemy &&
                    ballModel.BouncedFromTablePart == RacketType.Enemy &&
                    ballModel.BouncedFromTableTimes >= 1)
                {
                    return true;
                }

                if (ballModel.BouncedFromRacket == RacketType.Enemy && ballModel.BouncedFromTableTimes == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private Vector3 CalculateApproximatedPosition(BallModel ball)
        {
            var ballPosition = ball.Transform.Position;
            var newBallSpeed = ball.GetSpeed();
            var approximatedPosition = PhysicMathHelper.ApproximatePosition(newBallSpeed, _tableNormal, ballPosition);

            if (approximatedPosition.X <= 0)
            {
                approximatedPosition.X = 0;
            }

            return approximatedPosition;
        }


        private static float GetRandomAngle()
        {
            var rnd = Random.Next(15, 20);
            return PhysicMathHelper.ToRadians(rnd);
        }

        private static Vector3 GetKnockRandomPoint()
        {
            var x = Random.Next(10, 20);
            var z = Random.Next(-5, 5);
            return new Vector3(x, 1, z);
        }

        private static Vector3 GetRandomPoint()
        {
            var x = Random.Next(10, (int)MaxX);
            var z = Random.Next((int)MinZ / 2, (int)MaxZ / 2);
            return new Vector3(-x, 1, -z);
        }

        private static bool PositionMatches(Vector3 position) => position.X > MinX && position.Z > 2 * MinZ && position.Z < 2 * MaxZ;
    }
}
