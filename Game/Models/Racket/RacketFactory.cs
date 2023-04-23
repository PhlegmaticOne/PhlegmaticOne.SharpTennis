using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket
{
    public class RacketFactoryData : IFactoryData
    {
        public bool IsPlayer { get; set; }
        public Color Color { get; set; }
        public Vector3 Normal { get; set; }
    }

    public class RacketFactory : IFactory<RacketBase, RacketFactoryData>
    {
        private readonly MeshLoader _meshLoader;
        private readonly TextureMaterialsProvider _textureMaterialsProvider;
        private readonly BallBounceProvider _ballBounceProvider;

        public RacketFactory(MeshLoader meshLoader, TextureMaterialsProvider textureMaterialsProvider,
            BallBounceProvider ballBounceProvider)
        {
            _meshLoader = meshLoader;
            _textureMaterialsProvider = textureMaterialsProvider;
            _ballBounceProvider = ballBounceProvider;
        }


        public RacketBase Create(Transform transform, RacketFactoryData racketFactoryData)
        {
            var racket = _meshLoader.LoadFbx("assets\\models\\racket.fbx", _textureMaterialsProvider.DefaultTexture);

            foreach (var component in racket)
            {
                component.GameObject.Name = "Racket";
                component.Transform.InitializeFromTransform(transform);
            }

            var go = new GameObject("Racket: { " + (racketFactoryData.IsPlayer ? "Player" : "Enemy") + " }");
            var model = CreateRacket(racketFactoryData.IsPlayer, racket);
            model.Normal = racketFactoryData.Normal;
            model.Color(racketFactoryData.Color);
            go.Transform.SetPosition(transform.Position);
            go.AddComponent(model);
            go.AddComponent(new RigidBody3D(Vector3.Zero, RigidBodyType.Kinematic));
            go.AddComponent(CreateCollider(transform.Position, racketFactoryData.IsPlayer));
            return model;
        }

        private RacketBase CreateRacket(bool isPlayer, List<MeshComponent> meshes)
        {
            return isPlayer
                ? (RacketBase)new PlayerRacket(meshes[0], meshes[1])
                : new EnemyRacket(meshes[0], meshes[1], _ballBounceProvider);
        }

        private BoxCollider3D CreateCollider(Vector3 position, bool isPlayer)
        {
            var c1 = isPlayer ? 3 : 0;
            var c2 = isPlayer ? 0 : 3;

            var collider = new BoxCollider3D(position - new Vector3(c1, 3f, 3f),
                position + new Vector3(c1, 7f, 6f))
            {
                Offset = new Vector3(0, 3, 1),
                RotationDivider = -60,
                IsStatic = true
            };
            return collider;
        }


        //Was : var collider = new BoxCollider3D(position - new Vector3(1f, 0.5f, 1.5f), position + new Vector3(1f, 5, 3.5f))
    }
}
