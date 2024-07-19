using Aspects;
using ComponentsAndTags;
using Tags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializationZombieSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            using var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var zombie in SystemAPI.Query<ZombieAspect>().WithAll<ZombieTag>())
            {
                ecb.RemoveComponent<ZombieTag>(zombie.zombiePrefab);
                ecb.SetComponentEnabled<ZombieWalkProperties>(zombie.zombiePrefab, false);
            }
            ecb.Playback(state.EntityManager);
        }
    }
}