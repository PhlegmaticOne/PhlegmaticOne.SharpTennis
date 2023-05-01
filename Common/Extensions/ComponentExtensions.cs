using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.Extensions
{
    public static class ComponentExtensions
    {
        public static T WrapWithGameObject<T>(this T component) where T : Component
        {
            var go = new GameObject();
            go.AddComponent(component);
            return component;
        }
    }
}
