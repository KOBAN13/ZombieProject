using Aspects;
using ComponentsAndTags;
using ComponentsAndTags.Brain;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieRiseSystem))]
    [UpdateInGroup(typeof(ZombieUpdateGroup))]
    public partial struct ZombieWalkSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var brainProperties = SystemAPI.GetSingleton<BrainProperties>();
            var brainAspect = SystemAPI.GetAspect<BrainAspect>(SystemAPI.GetSingletonEntity<BrainProperties>());
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            new ZombieWalkJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                BrainProperties = brainProperties,
                brainPosition = brainAspect.BrainTransform,
                ecb = ecb.AsParallelWriter()
            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float deltaTime;
        public BrainProperties BrainProperties;
        public float3 brainPosition;
        public EntityCommandBuffer.ParallelWriter ecb;
        [BurstCompile]
        private void Execute(ZombieWalkAspect zombieWalkAspect, [EntityIndexInQuery] int sortKey)
        {
            zombieWalkAspect.Walk(deltaTime);
            if (math.distancesq(brainPosition, zombieWalkAspect.ZombiePosition) <= BrainProperties.radiusBrain)
            {
                ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombieWalkAspect.zombiePrefab, false);
                ecb.SetComponentEnabled<ZombieEatProperties>(sortKey, zombieWalkAspect.zombiePrefab, true);
            }
        }
    }
}