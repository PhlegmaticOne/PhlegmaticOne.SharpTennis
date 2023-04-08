using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public class SphereCollider : Collider
    {
        private BoundingSphere _boundingSphere;

        public SphereCollider(Vector3 center, float radius)
        {
            _boundingSphere = new BoundingSphere(center, radius);
        }

        public BoundingSphere BoundingSphere => _boundingSphere;

        public override void Start()
        {
            base.Start();
            Transform.Moved += TransformOnMoved;
        }

        private void TransformOnMoved(Vector3 obj)
        {
            var center = _boundingSphere.Center;
            center += obj;
            _boundingSphere.Center = center;
        }

        public override bool Intersects(Collider other)
        {
            if (other is BoxCollider3D boxCollider)
            {
                return _boundingSphere.Intersects(boxCollider.BoundingBox);
            }


            if (other is SphereCollider sphereCollider)
            {
                return _boundingSphere.Intersects(sphereCollider.BoundingSphere);
            }

            return false;
        }
    }
}
