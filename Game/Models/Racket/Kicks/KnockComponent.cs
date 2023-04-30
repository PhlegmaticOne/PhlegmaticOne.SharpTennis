using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks
{
    public class KnockComponent : Component
    {
        private const float Lerp = 300;
        private readonly float _tablePartHeight;
        private readonly bool _inverseX;

        public KnockComponent(float tablePartHeight, bool inverseX)
        {
            _tablePartHeight = tablePartHeight;
            _inverseX = inverseX;
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
            var lerp = MathUtil.Lerp(0f, 1f, force > Lerp ? 1 : force / Lerp);
            return Vector3.Lerp(Vector3.Zero, line, lerp);
        }
    }
}
