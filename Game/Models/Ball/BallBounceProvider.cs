using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public class BallBounceProvider
    {
        public event Action<Component, BallModel> BallBounced;

        public void OnBallBounced(Component from, BallModel model) => BallBounced?.Invoke(from, model);
    }
}
