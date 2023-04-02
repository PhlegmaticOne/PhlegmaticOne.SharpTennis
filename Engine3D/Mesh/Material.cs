using PhlegmaticOne.SharpTennis.Game.Engine3D.Shaders;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine3D.Mesh
{
    public class Material
    {
        public MaterialProperties MaterialProperties { get; }
        public Texture Texture { get; set; }


        public Material(Texture texture, Vector3 emmisiveK, Vector3 ambientK, Vector3 diffuseK, Vector3 specularK, float specularPower)
        {
            Texture = texture;
            MaterialProperties = new MaterialProperties
            {
                emmisiveK = emmisiveK,
                ambientK = ambientK,
                diffuseK = diffuseK,
                specularK = specularK,
                specularPower = specularPower
            };
        }
    }
}
