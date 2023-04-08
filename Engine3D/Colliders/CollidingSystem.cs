using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public class CollidingSystem : BehaviorObject
    {
        protected override void Update()
        {
            var allColliders = Scene.Current.GetComponents<Collider>().ToList();

            for (var i = 0; i < allColliders.Count; i++)
            {
                for (var j = i + 1; j < allColliders.Count; j++)
                {
                    var a = allColliders[i];
                    var b = allColliders[j];

                    if (a.Intersects(b))
                    {
                        a.OnCollisionEnter(b);
                        b.OnCollisionEnter(a);
                    }
                }
            }
        }
    }
}
