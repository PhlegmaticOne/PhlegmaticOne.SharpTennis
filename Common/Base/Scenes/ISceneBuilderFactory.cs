using System;

namespace PhlegmaticOne.SharpTennis.Game.Common.Base.Scenes
{
    public struct SceneType
    {
        public SceneType(string name) => Name = name;

        public string Name { get; }
    }

    public interface IGameScenes
    {
        SceneType Default { get; }
    }

    public interface ISceneBuilderFactory<out T> where T : IGameScenes
    {
        T Scenes { get; }
        ISceneBuilder CreateSceneBuilder(SceneType sceneType);
    }
}
