﻿using System;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh;
using PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Ball;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Floor
{
    public class FloorModel : MeshableObject
    {
        public BoxCollider3D Collider { get; private set; }

        public FloorModel(MeshComponent mesh) => AddMeshes(mesh);

        public override void Start() => Collider = GameObject.GetComponent<BoxCollider3D>();

        public override void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.TryGetComponent<BallModel>(out var ball))
            {
                ball.IsInGame = false;
                ball.BounceDirect(this, Vector3.Zero);
            }
        }
    }
}
