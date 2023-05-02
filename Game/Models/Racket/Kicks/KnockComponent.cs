using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks
{
    public class KnockComponent : Component
    {
        private float _lerp = 300;
        private readonly float _tablePartHeight;

        public KnockComponent(float tablePartHeight)
        {
            _tablePartHeight = tablePartHeight;
        }

        public void SetMaxLerp(float maxLerp)
        {
            _lerp = maxLerp;
        }

        public void KnockBall(BallModel ball, Vector3 direction, float force, float speedY = -20)
        {
            var point = GetPointOnLineInDirection(direction, force);
            var lineFromBallPoint = ball.Transform.Position + point;
            lineFromBallPoint.Y = 1;
            var speed = PhysicMathHelper.CalculateSpeedToPointAngleZero(ball.Transform.Position, lineFromBallPoint, speedY);
            ball.BounceDirect(this, speed);
        }

        private Vector3 GetPointOnLineInDirection(Vector3 direction, float force)
        {
            var k = _tablePartHeight / direction.X;
            var maxZ = direction.Z * k;
            var maxX = Math.Abs(k) * direction.X;
            var line = new Vector3(maxX, 1, maxZ);
            var lerp = MathUtil.Lerp(0f, 1f, force > _lerp ? 1 : force / _lerp);
            return Vector3.Lerp(Vector3.Zero, line, lerp);
        }
    }
}
