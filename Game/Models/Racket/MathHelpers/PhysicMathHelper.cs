using PhlegmaticOne.SharpTennis.Game.Common.Base;
using SharpDX;
using System;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.MathHelpers
{
    public static class PhysicMathHelper
    {
        public static Vector3 CalculateSpeedToPoint(Vector3 startPoint, Vector3 endPoint, float angle, bool isInAngles = false)
        {
            if (isInAngles)
            {
                angle = ToRadians(angle);
            }

            var g = RigidBody3D.G;
            var sin = (float)Math.Sin(angle);
            var d = endPoint - startPoint;
            var l = d.Length();
            var v = (float)Math.Sqrt(l * g / Math.Sin(2 * angle));
            var t = 2 * v * sin / g;
            var newSpeed = new Vector3(d.X / t, v * sin, d.Z / t);
            return newSpeed;
        }

        public static Vector3 CalculateSpeedToPointAngleZero(Vector3 startPoint, Vector3 endPoint, float setYSpeed = 0f)
        {
            var g = RigidBody3D.G;
            var d = endPoint - startPoint;
            var t = (float)Math.Sqrt(2 * Math.Abs(d.Y) / g);
            var newSpeed = new Vector3(d.X / t, setYSpeed, d.Z / t);
            return newSpeed;
        }

        public static Vector3 ApproximatePosition(Vector3 speed, Vector3 normal, Vector3 initialPosition)
        {
            var flyTime = CalculateFlyTime(speed, normal);
            var newX = initialPosition.X + flyTime * speed.X;
            var newZ = initialPosition.Z + flyTime * speed.Z;
            return new Vector3(newX, initialPosition.Y, newZ);
        }

        public static float CalculateFlyTime(Vector3 speed, Vector3 normal)
        {
            var g = RigidBody3D.G;
            var speedLength = speed.Length();
            var angleCos = Vector3.Dot(normal, speed.Normalized());
            var angle = Math.PI / 2 - Math.Acos(angleCos);
            var sine = Math.Sin(angle);
            return (float)(2 * speedLength * sine) / g;
        }

        public static float ToRadians(float angle) => angle * (float)Math.PI / 180;
    }
}
