using Aspects;
using ComponentsAndTags;
using ComponentsAndTags.Brain;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieWalkSystem))]
    [UpdateInGroup(typeof(ZombieUpdateGroup))]
    public partial struct ZombieEatSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BrainProperties>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var brainAspect = SystemAPI.GetAspect<BrainAspect>(SystemAPI.GetSingletonEntity<BrainProperties>());

            var scale = brainAspect.Scale * 5f + 1f;
            
            new ZombieEatJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                brain = SystemAPI.GetSingletonEntity<BrainProperties>(),
                brainPosition = brainAspect.BrainTransform,
                scale = scale
            }.ScheduleParallel();
        }
    }
    
    public partial struct ZombieEatJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public Entity brain;
        public float scale;
        public float3 brainPosition;
        
        private void Execute(ZombieAspectEat aspectEat, [EntityIndexInQuery] int sortKey)
        {
            if (aspectEat.IsInZombieEat(brainPosition, scale))
            {
                aspectEat.EatBrain(deltaTime, ecb, sortKey, brain);
            }
            else
            {
                ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, aspectEat.zombiePrefab, true);
                ecb.SetComponentEnabled<ZombieEatProperties>(sortKey, aspectEat.zombiePrefab, false);
            }
        }
    }
}