﻿using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class RacketFactory : IFactory<Racket>
    {
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;

        public RacketFactory(MeshLoader meshLoader, TextureMaterialsProvider textureMaterialsProvider)
        {
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
        }


        public Racket Create(Transform transform)
        {
            var racket = _meshLoader.LoadFbx("assets\\models\\racket.fbx", _textureMaterialsProvider.DefaultTexture);

            foreach (var component in racket)
            {
                component.GameObject.Name = "Racket";
                component.Transform.SetPosition(transform.Position);
                component.Transform.SetRotation(transform.Rotation);
                component.Transform.SetScale(transform.Scale);
            }

            var go = new GameObject("Racket");
            var model = new Racket(racket[0], racket[1])
            {
                Normal = Vector3.Right
            };
            go.Transform.SetPosition(transform.Position);
            go.AddComponent(model);
            go.AddComponent(new RigidBody3D(Vector3.Zero, RigidBodyType.Kinematic));
            go.AddComponent(CreateCollider(transform.Position));
            return model;
        }

        private BoxCollider3D CreateCollider(Vector3 position)
        {
            var collider = new BoxCollider3D(position - new Vector3(1f, 0.5f, 1.5f),
                position + new Vector3(1f, 5, 3.5f))
            {
                Offset = new Vector3(0, 3, 1),
                RotationDivider = -60,
                IsStatic = true
            };
            return collider;
        }
    }
}
