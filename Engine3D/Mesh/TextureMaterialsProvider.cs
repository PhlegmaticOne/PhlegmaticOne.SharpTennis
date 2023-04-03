using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class TextureMaterialsProvider : IDisposable
    {
        public const string DefaultTextureKey = "DefaultTextureKey";
        public const string DefaultMaterialKey = "DefaultMaterialKey";

        private readonly Dictionary<string, Texture> _textures;
        private readonly Dictionary<string, Material> _materials;

        public TextureMaterialsProvider()
        {
            _textures = new Dictionary<string, Texture>();
            _materials = new Dictionary<string, Material>();
        }

        public Texture DefaultTexture { get; private set; }
        public Material DefaultMaterial { get; private set; }

        public void SetDefaultMaterial(Material defaultMaterial)
        {
            DefaultMaterial = defaultMaterial;
            AddMaterial(DefaultMaterialKey, defaultMaterial);
        }

        public void SetDefaultTexture(Texture texture)
        {
            DefaultTexture = texture;
            AddTexture(DefaultTextureKey, texture);
        }

        public void AddMaterial(string key, Material material)
        {
            _materials.Add(key, material);
        }
        public Material GetMaterial(string key) => _materials[key];

        public void AddTexture(string key, Texture texture)
        {
            _textures.Add(key, texture);
        }

        public Texture GetTexture() => _textures[DefaultTextureKey];

        public void Dispose()
        {
            Utils.DisposeDictionaryElements(_textures);
            _materials.Clear();
        }
    }
}
