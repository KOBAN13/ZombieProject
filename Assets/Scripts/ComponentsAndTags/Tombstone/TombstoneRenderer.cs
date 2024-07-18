using Unity.Entities;

namespace ComponentsAndTags
{
    public struct TombstoneRenderer : IComponentData
    {
        public Entity prefab;
    }
}