using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Colliders
{
    public class CollidingSystem : BehaviorObject
    {
        //private string File = "C:\\Users\\lolol\\OneDrive\\Рабочий стол\\TEST.txt";
        //private StreamWriter _fileStream;

        //public CollidingSystem()
        //{
        //    var s = new FileStream(File, FileMode.OpenOrCreate);
        //    _fileStream = new StreamWriter(s);
        //}

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
                        if (a.IsColliding == false)
                        {
                            a.Collision(b);
                            b.Collision(a);
                        }
                    }
                    else
                    {
                        if (a.IsColliding)
                        {
                            a.CollisionExit(b);
                            b.CollisionExit(a);
                        }
                    }
                }
            }
        }
    }
}
