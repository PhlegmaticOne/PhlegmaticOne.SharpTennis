using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Elements
{
    public class BallFlyViewController 
    {
        private readonly BallFlyView _ballFlyView;
        private TimeSpan _startTime;

        public BallFlyViewController(BallFlyView ballFlyView, BallModel ballModel)
        {
            _ballFlyView = ballFlyView;
            ballModel.Bounced += BallModelOnBounced;
            _startTime = TimeSpan.Zero;
        }

        public void Update()
        {
            _startTime += TimeSpan.FromSeconds(Time.DeltaT);
            _ballFlyView.UpdateTime(_startTime);
        }

        private int i;
        private void BallModelOnBounced(Component arg1, BallModel arg2)
        {
            if (i == 1)
            {
                return;
            }

            i++;
            _startTime = TimeSpan.Zero;
        }
    }
}
