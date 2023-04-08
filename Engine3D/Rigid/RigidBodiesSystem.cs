using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Rigid
{
    public class RigidBodiesSystem : BehaviorObject
    {
        protected override void Update()
        {
            foreach (var rigidBody3D in Scene.Current.GetComponents<RigidBody3D>())
            {
                rigidBody3D.UpdateBehavior();
            }
        }
    }
}
