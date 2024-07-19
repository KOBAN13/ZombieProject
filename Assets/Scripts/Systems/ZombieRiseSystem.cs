using Aspects;
using ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(ZombieUpdateGroup))]
    [UpdateAfter(typeof(ZombieSpawnSystem))]
    public partial struct ZombieRiseSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ZombieRiseTime>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            new ZombieRiseJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = ecb.AsParallelWriter()
            }
                .ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        [BurstCompile]
        private void Execute(ZombieAspect zombieAspect, [EntityIndexInQuery] int sortKey)
        {
            zombieAspect.RiseToGround(deltaTime);
            if (zombieAspect.IsZombieRiseToGround == false) return;
            
            zombieAspect.ZombieOnGround();
            ecb.RemoveComponent<ZombieRiseTime>(sortKey, zombieAspect.zombiePrefab);
            ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombieAspect.zombiePrefab, true);
        }
    }

    public partial class ZombieUpdateGroup : ComponentSystemGroup
    {
    }
}