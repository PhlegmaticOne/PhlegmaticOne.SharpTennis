using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Kicks
{
    public class KickComponent : Component
    {
        private const float Lerp = 300;
        private readonly float _tablePartHeight;

        public KickComponent(float tablePartHeight) => _tablePartHeight = tablePartHeight;

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
            var maxZ = direction.Z * k;
            var line = new Vector3(maxX, 1, maxZ);
            var lerp = MathUtil.Lerp(0f, 1f, force > Lerp ? 1 : force / Lerp);
            return Vector3.Lerp(line / 2, line, lerp);
        }
    }
}
