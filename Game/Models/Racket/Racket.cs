using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class Racket : BehaviorObject
    {
        private readonly MeshComponent _coloredComponent;
        private readonly MeshComponent _handComponent;

        public Racket(MeshComponent coloredComponent, MeshComponent handComponent)
        {
            _coloredComponent = coloredComponent;
            _handComponent = handComponent;
            Meshes = new List<MeshComponent> { _coloredComponent, _handComponent };
        }

        public List<MeshComponent> Meshes { get; }

        public override void Start()
        {
            Transform.SetPosition(_handComponent.Transform.Position);
            Transform.Moved += TransformOnMoved;
            Transform.Rotated += TransformOnRotated;
        }

        private void TransformOnRotated(Vector3 obj)
        {
            _coloredComponent.Transform.Rotate(obj);
            _handComponent.Transform.Rotate(obj);
        }

        private void TransformOnMoved(Vector3 obj)
        {
            _coloredComponent.Transform.Move(obj);
            _handComponent.Transform.Move(obj);
        }

        public void Color(Color color)
        {
            var vector = new Vector3(color.R, color.G, color.B) / 255;
            var properties = _coloredComponent.MeshObjectData.Material.MaterialProperties;
            properties.SetColor(vector);
            _coloredComponent.MeshObjectData.Material.MaterialProperties = properties;
        }
    }
}
