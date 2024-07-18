using Unity.Entities;
using Unity.Mathematics;

namespace ComponentsAndTags
{
    public struct ZombieWalkProperties : IComponentData
    {
        public float walkSpeed;
        public float walkAmplitude;
        public float walkFrequency;
    }
}