using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace ComponentsAndTags
{
    [MaterialProperty("TombstoneOffset")]
    public struct TombstoneMaterialProperty : IComponentData
    {
        public float2 materialProperty;
    }
}