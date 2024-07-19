using Unity.Entities;

namespace ComponentsAndTags
{
    public struct ZombieWalkProperties : IComponentData, IEnableableComponent
    {
        public float walkSpeed;
        public float walkAmplitude;
        public float walkFrequency;
    }
}