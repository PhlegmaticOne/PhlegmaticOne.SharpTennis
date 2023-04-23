using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Table;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Controllers
{
    public class EnemyRacketController : BehaviorObject
    {
        private readonly RacketBase _enemyRacket;
        private readonly TennisTable _tennisTable;
        private readonly BallBounceProvider _ballBounceProvider;
        private readonly Vector2 _tableSize;
        private readonly float _tableY;
        private readonly TableTopPart _tableTop;

        private Vector3 _approximatedPosition;

        public EnemyRacketController(RacketBase enemyRacket, TennisTable tennisTable,
            BallBounceProvider ballBounceProvider)
        {
            _enemyRacket = enemyRacket;
            _tennisTable = tennisTable;
            _ballBounceProvider = ballBounceProvider;
            _tableSize = tennisTable.TableTopPart.Size;
            _tableY = tennisTable.TableTopPart.Transform.Position.Y;
            _tableTop = tennisTable.TableTopPart;
            _ballBounceProvider.BallBounced += BallBounceProviderOnBallBounced;
        }

        private const float Coeff = -150f;

        private void BallBounceProviderOnBallBounced(Component from, BallModel ball)
        {
            if (ball.BouncedFrom == BallBouncedFromType.Enemy || ball.BouncedFrom == BallBouncedFromType.None)
            {
                return;
            }

            var ballPosition = ball.Transform.Position;
            var newBallSpeed = ball.GetSpeed();
            var approximatedPosition = ApproximatePositionWhenYWillBeZero(newBallSpeed, ballPosition);

            if (approximatedPosition.X <= 0)
            {
                approximatedPosition.X = 0;
            }

            _approximatedPosition = approximatedPosition;
        }

        protected override void Update()
        {
            if (_approximatedPosition == Vector3.Zero)
            {
                return;
            }

            var lerp = Vector3.Lerp(_enemyRacket.Transform.Position, _approximatedPosition, 0.03f);
            _enemyRacket.Transform.SetPosition(lerp);
        }


        private Vector3 ApproximatePositionWhenYWillBeZero(Vector3 speed, Vector3 initialPosition)
        {
            var flyTime = CalculateFlyTime(speed);
            var newX = initialPosition.X + flyTime * speed.X;
            var newZ = initialPosition.Z + flyTime * speed.Z;
            return new Vector3(newX, initialPosition.Y, newZ);
        }

        private float CalculateFlyTime(Vector3 speed)
        {
            var speedLength = speed.Length();
            var angleCos = Vector3.Dot(_tableTop.Normal, speed.Normalized());
            var angle = Math.PI / 2 - Math.Acos(angleCos);
            var sine = Math.Sin(angle);

            return (float)(2 * speedLength * sine) / Coeff * RigidBody3D.GlobalAcceleration;
        }


        private float CalculateMaxHeight(Vector3 speed)
        {
            var speedLength = speed.Length();
            var angleCos = Vector3.Dot(_tableTop.Normal, speed.Normalized());
            var angle = Math.PI / 2 - Math.Acos(angleCos);
            var sine = Math.Sin(angle);

            return (float)(speedLength * speedLength * sine * sine) / (2 * RigidBody3D.GlobalAcceleration);
        }
    }
}
