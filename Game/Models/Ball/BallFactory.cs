﻿using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Ball
{
    public class BallFactory : IFactory<BallModel>
    {
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;

        public BallFactory(MeshLoader meshLoader, TextureMaterialsProvider textureMaterialsProvider)
        {
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
        }

        public BallModel Create(Transform transform)
        {
            var ball = _meshLoader.LoadFbx("assets\\models\\ball.fbx", _textureMaterialsProvider.DefaultTexture)[0];
            var radius = ball.MeshObjectData.Vertices.Max(x => x.position.X);
            ball.Transform.SetPosition(transform.Position);

            var go = new GameObject("Ball");
            var model = new BallModel(ball);
            go.AddComponent(new RigidBody3D(20 * Vector3.Left, RigidBodyType.Dynamic));
            go.AddComponent(new SphereCollider(transform.Position, radius));
            go.AddComponent(model);
            return model;
        }
    }
}
