using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using SharpDX;
using System;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers
{
    public static class PhysicMathHelper
    {
        public static Vector3 CalculateSpeedToPoint(Vector3 startPoint, Vector3 endPoint, float angle, float g)
        {
            var sin = (float)Math.Sin(angle);
            var d = endPoint - startPoint;
            var l = d.Length();
            var v = (float)Math.Sqrt(l * g / Math.Sin(2 * angle));
            var t = 2 * v * sin / g;
            var newSpeed = new Vector3(d.X / t, v * sin, d.Z / t);
            return newSpeed;
        }

        public static Vector3 ApproximatePosition(Vector3 speed, Vector3 normal, Vector3 initialPosition, float g)
        {
            var flyTime = CalculateFlyTime(speed, normal, g);
            var newX = initialPosition.X + flyTime * speed.X;
            var newZ = initialPosition.Z + flyTime * speed.Z;
            return new Vector3(newX, initialPosition.Y, newZ);
        }

        public static float CalculateFlyTime(Vector3 speed, Vector3 normal, float g)
        {
            var speedLength = speed.Length();
            var angleCos = Vector3.Dot(normal, speed.Normalized());
            var angle = Math.PI / 2 - Math.Acos(angleCos);
            var sine = Math.Sin(angle);
            return (float)(2 * speedLength * sine) / g;
        }

        public static float ToRadians(float angle) => angle * (float)Math.PI / 180;
    }
}
