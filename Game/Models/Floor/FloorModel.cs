using System;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Floor
{
    public class FloorModel : MeshableObject
    {
        private readonly ISoundManager<GameSounds> _soundManager;
        public BoxCollider3D Collider { get; private set; }

        public FloorModel(MeshComponent mesh, ISoundManager<GameSounds> soundManager)
        {
            _soundManager = soundManager;
            AddMeshes(mesh);
        }

        public override void Start() => Collider = GameObject.GetComponent<BoxCollider3D>();

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                _soundManager.Play(GameSounds.FloorBounce);
                ball.BounceDirect(this, Vector3.Zero);
                ball.BallGameState = BallGameState.None;
            }
        }
    }
}
