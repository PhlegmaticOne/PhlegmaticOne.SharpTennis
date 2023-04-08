using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Base
{
    public interface IFactory<out T> where T : Component
    {
        T Create(Transform transform);
    }
}