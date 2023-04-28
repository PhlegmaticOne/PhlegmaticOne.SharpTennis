using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public static class BallBounceHelper
    {
        public static RacketType GetBallBounceType(BallModel ball, Vector3 minPosition, Vector3 maxPosition)
        {
            var position = ball.Transform.Position;
            var result = RacketType.None;

            if (position.X > minPosition.X && position.X < 0)
            {
                result = RacketType.Enemy;
            }

            if (position.X < maxPosition.X && position.X > 0)
            {
                result = RacketType.Player;
            }

            return result;
        }
    }
}
