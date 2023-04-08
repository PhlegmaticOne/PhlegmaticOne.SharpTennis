using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public class BoxCollider3D : Collider
    {
        private  BoundingBox _boundingBox;
        private Vector3 _currentMin;
        private Vector3 _currentMax;

        public BoxCollider3D(Vector3 a, Vector3 b)
        {
            _boundingBox = new BoundingBox(a, b);
            _currentMin = a;
            _currentMax = b;
        }

        public override void Start()
        {
            base.Start();
            Transform.Moved += TransformOnMoved;
        }
        public BoundingBox BoundingBox => _boundingBox;

        private void TransformOnMoved(Vector3 obj)
        {
            _currentMin += obj;
            _currentMax += obj;
            _boundingBox = new BoundingBox(_currentMin, _currentMax);
        }

        public override bool Intersects(Collider other)
        {
            if (other is BoxCollider3D boxCollider)
            {
                return _boundingBox.Intersects(boxCollider._boundingBox);
            }

            if (other is SphereCollider sphereCollider)
            {
                return _boundingBox.Intersects(sphereCollider.BoundingSphere);
            }

            return false;
        }
    }
}
