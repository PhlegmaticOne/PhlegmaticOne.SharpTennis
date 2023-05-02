using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks
{
    public class KickComponent : Component
    {
        private float _lerp = 300;
        private const float Z = 20f;
        private readonly float _tablePartHeight;

        public KickComponent(float tablePartHeight)
        {
            _tablePartHeight = tablePartHeight;
        }

        public void SetMaxLerp(float maxLerp)
        {
            _lerp = maxLerp;
        }

        public void KickBall(BallModel ball, Vector3 direction, float force)
        {
            var point = GetPointOnLineInDirection(direction, force);
            var lineFromBallPoint = ball.Transform.Position + point;
            lineFromBallPoint.Y = 1;
            var speed = PhysicMathHelper.CalculateSpeedToPoint(ball.Transform.Position, lineFromBallPoint, 15, true);
            ball.BounceDirect(this, speed);
        }

        private Vector3 GetPointOnLineInDirection(Vector3 direction, float force)
        {
            var maxX = _tablePartHeight * 2;
            var k = 2 * maxX / direction.X;
            var maxZ = Math.Sign(direction.Z) * Math.Min(Math.Abs(direction.Z * k), Z);
            var line = new Vector3(maxX, 1, maxZ);
            var lerp = MathUtil.Lerp(0f, 1f, force > _lerp ? 1 : force / _lerp);
            return Vector3.Lerp(line / 2, line, lerp);
        }
    }
}
