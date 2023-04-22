using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Base
{
    public interface IFactory<out T> where T : Component
    {
        T Create(Transform transform);
    }

    public interface IFactory<out T, in TData> where T : Component where TData : IFactoryData
    {
        T Create(Transform transform, TData data);
    }

    public interface IFactoryData { }
}