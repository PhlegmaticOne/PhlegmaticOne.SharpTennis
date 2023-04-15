using System;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public readonly struct CollisionContainer : IEquatable<CollisionContainer>
    {
        public Collider A { get; }
        public Collider B { get; }

        public CollisionContainer(Collider a, Collider b)
        {
            A = a;
            B = b;
        }

        public bool DontCollideAnymore() => A.Intersects(B) == false;

        public void CollideObjects()
        {
            A.Collision(B);
            B.Collision(A);
        }

        public bool Equals(CollisionContainer other) => Equals(A, other.A) && Equals(B, other.B);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((CollisionContainer)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((A != null ? A.GetHashCode() : 0) * 397) ^ (B != null ? B.GetHashCode() : 0);
            }
        }
    }
}