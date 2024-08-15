using Unity.Entities;
using Unity.Mathematics;

namespace ComponentsAndTags
{
    public struct ZombieEatProperties : IComponentData, IEnableableComponent
    {
        public float zombieEatDamage;
        public float zombieEatAmplitude;
        public float zombieEatFrequency;
    }
}