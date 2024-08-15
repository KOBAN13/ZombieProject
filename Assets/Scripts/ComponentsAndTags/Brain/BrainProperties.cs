using Unity.Entities;

namespace ComponentsAndTags.Brain
{
    public struct BrainProperties : IComponentData
    {
        public float radiusBrain;
        public float MaxHealth;
        public float Value;
    }
}