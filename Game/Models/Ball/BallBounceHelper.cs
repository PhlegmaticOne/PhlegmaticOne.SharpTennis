using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public static class BallBounceHelper
    {
        public static BallBouncedFromType GetBallBounceType(BallModel ball, Vector3 minPosition, Vector3 maxPosition)
        {
            var position = ball.Transform.Position;
            var result = BallBouncedFromType.None;

            if (position.X > minPosition.X && position.X < 0)
            {
                result = BallBouncedFromType.Enemy;
            }

            if (position.X < maxPosition.X && position.X > 0)
            {
                result = BallBouncedFromType.Player;
            }

            return result;
        }
    }
}
