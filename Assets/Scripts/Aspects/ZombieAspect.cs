using ComponentsAndTags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Aspects
{
    public readonly partial struct ZombieAspect : IAspect
    {
        public readonly Entity zombiePrefab;
        private readonly RefRO<ZombieRiseTime> _zombieRiseTime;
        private readonly RefRW<LocalTransform> _zombieTransform;
        
        public float ZombieRiseTime => _zombieRiseTime.ValueRO.timeToRise;

        public bool IsZombieRiseToGround =>_zombieTransform.ValueRO.Position.y >= 0;
        public void ZombieOnGround() => _zombieTransform.ValueRW.Position.y = 0;

        public void RiseToGround(float deltaTime)
        {
            _zombieTransform.ValueRW.Position += math.up() * ZombieRiseTime * deltaTime;
        }
    }
}