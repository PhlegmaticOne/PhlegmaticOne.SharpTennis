using System;
using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Extensions;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public class BoxCollider3D : Collider
    {
        private  BoundingBox _boundingBox;
        private Vector3 _currentMin;
        private Vector3 _currentMax;
        private Vector3 _rotation;

        public BoxCollider3D(Vector3 a, Vector3 b)
        {
            _boundingBox = new BoundingBox(a, b);
            _currentMin = a;
            _currentMax = b;
        }

        public Vector3 Offset { get; set; }
        public float RotationDivider { get; set; }
        public bool IsStatic { get; set; } = true;

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

        public void Rotate(Vector3 rotation)
        {
            _rotation += rotation;
        }

        public IEnumerable<Vector3> GetVertices()
        {
            var center = _boundingBox.Center - Offset;
            var aroundZero = new BoundingBox(
                _boundingBox.Minimum - center,
                _boundingBox.Maximum - center);

            var rotation = Quaternion.RotationAxis(Vector3.UnitX, _rotation.Z / RotationDivider);
            return aroundZero.GetCorners()
                .Select(x =>
                {
                    var result = Rotate(rotation, x);
                    return result + center;
                });
        }

        public static Vector3 Rotate(Quaternion rotation, Vector3 point)
        {
            var num1 = rotation.X * 2f;
            var num2 = rotation.Y * 2f;
            var num3 = rotation.Z * 2f;
            var num4 = rotation.X * num1;
            var num5 = rotation.Y * num2;
            var num6 = rotation.Z * num3;
            var num7 = rotation.X * num2;
            var num8 = rotation.Y * num3;
            var num9 = rotation.Y * num3;
            var num10 = rotation.W * num1;
            var num11 = rotation.W * num2;
            var num12 = rotation.W * num3;
            Vector3 vector3;
            vector3.X = (1.0f - (num5 + num6)) * point.X + (num7 - num12) * point.Y + (num8 + num11) * point.Z;
            vector3.Y = (num7 + num12) * point.X + (1.0f - (num4 + num6)) * point.Y + (num9 - num10) * point.Z;
            vector3.Z = (num8 - num11) * point.X + (num9 + num10) * point.Y + (1.0f - (num4 + num5)) * point.Z;
            return vector3;
        }

        public override bool Intersects(Collider other)
        {
            if (other is BoxCollider3D boxCollider)
            {
                return _boundingBox.Intersects(boxCollider._boundingBox);
            }

            if (other is SphereCollider sphereCollider)
            {
                if (IsStatic)
                {
                    return _boundingBox.Intersects(sphereCollider.BoundingSphere);
                }

                var vertices = GetVertices().ToArray();
                var r = new Vector3(-2 * sphereCollider.BoundingSphere.Radius, 0, 0);
                var center = sphereCollider.BoundingSphere.Center;
                var maxPoint = center + r;
                //var minPoint = center - r;

                if (maxPoint.X > vertices[1].X || maxPoint.X < vertices[3].X)
                {
                    return false;
                }

                var frontFace = vertices
                    .SelectAtIndexesInOrder(1, 2, 6, 5)
                    .Select(x => new Vector2(x.Z, x.Y))
                    .ToArray();

                return OnPolygonCollision.CheckOnPolygon(frontFace, new Vector2(maxPoint.Z, maxPoint.Y));
            }

            return false;
        }
    }

    public static class OnPolygonCollision
    {
        static int OnLine(Line l1, Vector2 p)
        {
            if (p.X <= Math.Max(l1.p1.X, l1.p2.X)
                && p.X <= Math.Min(l1.p1.X, l1.p2.X)
                && (p.Y <= Math.Max(l1.p1.Y, l1.p2.Y)
                    && p.Y <= Math.Min(l1.p1.Y, l1.p2.Y)))
                return 1;

            return 0;
        }

        static int Direction(Vector2 a, Vector2 b, Vector2 c)
        {
            var val = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);

            if (val == 0)
            {
                return 0;
            }

            if (val < 0)
            {
                return 2;
            }

            return 1;
        }

        static int IsIntersect(Line l1, Line l2)
        {
            var dir1 = Direction(l1.p1, l1.p2, l2.p1);
            var dir2 = Direction(l1.p1, l1.p2, l2.p2);
            var dir3 = Direction(l2.p1, l2.p2, l1.p1);
            var dir4 = Direction(l2.p1, l2.p2, l1.p2);

            if (dir1 != dir2 && dir3 != dir4)
                return 1;

            if (dir1 == 0 && OnLine(l1, l2.p1) == 1)
                return 1;

            if (dir2 == 0 && OnLine(l1, l2.p2) == 1)
                return 1;

            if (dir3 == 0 && OnLine(l2, l1.p1) == 1)
                return 1;

            if (dir4 == 0 && OnLine(l2, l1.p2) == 1)
                return 1;

            return 0;
        }

        public static bool CheckOnPolygon(Vector2[] poly, Vector2 p)
        {
            var n = poly.Length;
            var pt = new Vector2(-9999, p.Y);

            var exline = new Line(p, pt);
            var count = 0;
            var i = 0;
            do
            {
                var side = new Line(poly[i], poly[(i + 1) % n]);
                if (IsIntersect(side, exline) == 1)
                {
                    if (Direction(side.p1, p, side.p2) == 0)
                        return OnLine(side, p) == 1;
                    count++;
                }
                i = (i + 1) % n;
            } while (i != 0);

            return count == 1;
        }

        private struct Line
        {
            public Vector2 p1, p2;
            public Line(Vector2 p1, Vector2 p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
        }
    }
}