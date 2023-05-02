using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using SharpDX;
using PhlegmaticOne.SharpTennis.Game.Common.StateMachine;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Difficulty;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.States;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class EnemyRacket : RacketBase
    {
        private const float MoveToStartLerp = 0.03f;
        private const float MinX = 0;
        private const float MaxX = 30;
        private const float MinZ = -25;
        private const float MaxZ = 25;

        private readonly IDifficultyService<EnemyRacketDifficulty> _difficultyService;
        private EnemyRacketDifficulty _difficulty;
        private readonly Vector3 _tableNormal;
        private StateComponent _stateComponent;
        private KnockComponent _knockComponent;
        private BoxCollider3D _boxCollider;
        private Vector3 _startPosition;
        private Vector3 _approximatedPosition;
        private BallModel _ballModel;

        public EnemyRacket(MeshComponent coloredComponent,
            MeshComponent handComponent,
            ISoundManager<GameSounds> soundManager,
            IDifficultyService<EnemyRacketDifficulty> difficultyService,
            Vector3 tableNormal) : 
            base(coloredComponent, handComponent, soundManager)
        {
            _difficultyService = difficultyService;
            _tableNormal = tableNormal;
        }

        protected override RacketType BallBounceType => RacketType.Enemy;

        public override void SetupDifficulty(DifficultyType difficultyType)
        {
            _difficulty = _difficultyService.GetDifficulty(difficultyType);
        }

        public override void Reset()
        {
            _stateComponent.Enter(State.None);
            base.Reset();
        }

        public void ChangeActive(bool active) => _stateComponent.ChangeEnabled(active);

        public override void Start()
        {
            _startPosition = Transform.Position;
            _stateComponent = GameObject.GetComponent<StateComponent>();
            _knockComponent = GameObject.GetComponent<KnockComponent>();
            _boxCollider = GameObject.GetComponent<BoxCollider3D>();
            InitializeStates();

            base.Start();
        }

        public void Knock(BallModel ball)
        {
            var direction = (GetKnockRandomPoint() - Transform.Position).Normalized();
            var force = Random.Next(150, 200);
            var y = _difficulty.GenerateKnockAngle();

            ball.BouncedFromRacket = BallBounceType;
            ball.BallGameState = BallGameState.Knocked;
            _boxCollider.DisableOnTime(0.3f);
            _knockComponent.KnockBall(ball, direction, force, -1 * y);
        }


        protected override void OnCollisionWithBall(BallModel ballModel)
        {
            var speed = ballModel.GetSpeed();

            if (speed.X == 0)
            {
                return;
            }

            var newSpeed = PhysicMathHelper.CalculateSpeedToPoint(ballModel.Transform.Position,
                GetRandomPoint(),
                PhysicMathHelper.ToRadians(_difficulty.GenerateKickAngle()));
            ballModel.BallGameState = BallGameState.InPlay;
            ballModel.BounceDirect(this, newSpeed);
        }


        public override void OnBallBounced(BallModel ball)
        {
            _ballModel = ball;

            if (ball.BouncedFromTablePart == RacketType.None ||
                ball.BallGameState == BallGameState.None ||
                ball.BouncedFromRacket == RacketType.None)
            {
                return;
            }

            var state = FindNewState(ball);
            _stateComponent.Enter(state);
        }

        public override void OnLost(BallModel ball)
        {
            _stateComponent.Enter(States<EnemyRacketStates>.Get.KnockState);
        }


        private void InitializeStates()
        {
            var states = States<EnemyRacketStates>.Get;
            _stateComponent.AddState(states.StayingState, () => new EmptyState());
            _stateComponent.AddState(states.FollowingBallState,
                () => new FollowingObjectState(_approximatedPosition, _difficulty.GetFollowingLerp(), this));
            _stateComponent.AddState(states.MovingToStartState,
                () => new FollowingObjectState(_startPosition, MoveToStartLerp, this));
            _stateComponent.AddState(states.KnockState, 
                () => new EnemyKnockState(_startPosition, MoveToStartLerp, this, _ballModel));
            _stateComponent.Exit();
        }

        private State FindNewState(BallModel ball)
        {
            var states = States<EnemyRacketStates>.Get;


            if (ball.BouncedFromRacket == RacketType.Player &&
                ball.BouncedFromTablePart == RacketType.Player ||
                ball.BouncedFromRacket == RacketType.Enemy)
            {
                return states.MovingToStartState;
            }


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
                    ShakeInTime(true, new Vector3(10, 0, 0), _approximatedPosition, flyTime - animationTime, animationTime);
                    return states.FollowingBallState;
                }

                return states.MovingToStartState;
            }

            return states.StayingState;
        }


        public void ShakeInTime(bool comparePosition, Vector3 shake, Vector3 finalPosition, float time, float animationTime, Action onComplete = null)
        {
            Invoke(time, () =>
            {
                if (comparePosition && (Transform.Position - _approximatedPosition).Length() >= _difficulty.GetMaxPositionToKick())
                {
                    return;
                }

                _stateComponent.ChangeEnabled(false);
                Transform.DoShake(shake, finalPosition, animationTime, () =>
                {
                    _stateComponent.ChangeEnabled(true);
                    onComplete?.Invoke();
                });
            });
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

        private static bool PositionMatches(Vector3 position) => position.X > MinX;
    }
}
