using ComponentsAndTags;
using ComponentsAndTags.Brain;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Aspects
{
    public readonly partial struct ZombieAspectEat : IAspect
    {
        public readonly Entity zombiePrefab;
        private readonly RefRO<ZombieEatProperties> _zombieProperties;
        private readonly RefRW<ZombieHeading> _zombieHeading;
        private readonly RefRW<ZombieTimer> _zombieTimer;
        private readonly RefRW<LocalTransform> _zombieTransform;

        public float ZombieEatDamage => _zombieProperties.ValueRO.zombieEatDamage;
        public float ZombieEatAmplitude => _zombieProperties.ValueRO.zombieEatAmplitude;
        public float ZombieEatFrequency => _zombieProperties.ValueRO.zombieEatFrequency;
        
        private float ZombieTimer
        {
            get => _zombieTimer.ValueRO.timer;
            set => _zombieTimer.ValueRW.timer = value;
        }

        public void EatBrain(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brain)
        {
            ZombieTimer += deltaTime;
            var eatAngle = ZombieEatAmplitude * math.sin(ZombieEatFrequency * ZombieTimer);
            _zombieTransform.ValueRW.Rotation = quaternion.Euler(eatAngle, _zombieHeading.ValueRO.angle, 0f);

            var eatDamage = ZombieEatDamage * deltaTime;
            var curBrainDamage = new BrainDamageBufferElement { damage = eatDamage };
            
            ecb.AppendToBuffer(sortKey, brain, curBrainDamage);
        }

        public bool IsInZombieEat(float3 brainPosition, float brainRadius)
        {
            return math.distancesq(_zombieTransform.ValueRO.Position, brainPosition) <= brainRadius - 1;
        }
    }
}